// (c) Copyright Microsoft Corporation, 2010.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using UIAutomationClient;

#endregion

namespace UIAComWrapper
{
	[Flags]
	public enum ClientSideProviderMatchIndicator
	{
		None,
		AllowSubstringMatch,
		DisallowBaseClassNameMatch
	}

	// Defines a client-side provider creation function
	// I would prefer not to use UIAutomationClient types in the API surface
	// In the original API, this was a System.Windows.Automation.Providers type.
	// But I cannot pass anything but a UIAutomationClient type into 
	// the COM API's return parameter.
	public delegate IRawElementProviderSimple ClientSideProviderFactoryCallback(IntPtr hwnd, int idChild, int idObject);

	[StructLayout(LayoutKind.Sequential)]
	public struct ClientSideProviderDescription
	{
		#region Fields

		private readonly string _className;
		private readonly ClientSideProviderMatchIndicator _flags;
		private readonly string _imageName;
		private readonly ClientSideProviderFactoryCallback _proxyFactoryCallback;

		#endregion

		#region Constructors

		public ClientSideProviderDescription(ClientSideProviderFactoryCallback clientSideProviderFactoryCallback, string className)
		{
			_className = (className != null) ? className.ToLower(CultureInfo.InvariantCulture) : null;
			_flags = ClientSideProviderMatchIndicator.None;
			_imageName = null;
			_proxyFactoryCallback = clientSideProviderFactoryCallback;
		}

		public ClientSideProviderDescription(ClientSideProviderFactoryCallback clientSideProviderFactoryCallback, string className, string imageName, ClientSideProviderMatchIndicator flags)
		{
			_className = (className != null) ? className.ToLower(CultureInfo.InvariantCulture) : null;
			_imageName = (imageName != null) ? imageName.ToLower(CultureInfo.InvariantCulture) : null;
			_flags = flags;
			_proxyFactoryCallback = clientSideProviderFactoryCallback;
		}

		#endregion

		#region Properties

		public string ClassName
		{
			get { return _className; }
		}

		public ClientSideProviderFactoryCallback ClientSideProviderFactoryCallback
		{
			get { return _proxyFactoryCallback; }
		}

		public ClientSideProviderMatchIndicator Flags
		{
			get { return _flags; }
		}

		public string ImageName
		{
			get { return _imageName; }
		}

		#endregion
	}

	internal class ProxyFactoryCallbackWrapper : IUIAutomationProxyFactory
	{
		#region Fields

		private readonly ClientSideProviderFactoryCallback _callback;
		private readonly int _serialNumber;
		private static int _staticSerialNumber = 1;

		#endregion

		#region Constructors

		public ProxyFactoryCallbackWrapper(ClientSideProviderFactoryCallback callback)
		{
			Debug.Assert(callback != null);
			_callback = callback;
			_serialNumber = _staticSerialNumber;
			Interlocked.Increment(ref _staticSerialNumber);
		}

		#endregion

		#region Properties

		string IUIAutomationProxyFactory.ProxyFactoryId
		{
			get { return "ProxyFactory" + _serialNumber; }
		}

		#endregion

		#region Methods

		IRawElementProviderSimple IUIAutomationProxyFactory.CreateProvider(IntPtr hwnd, int idObject, int idChild)
		{
			var provider = _callback(hwnd, idChild, idObject);
			return provider;
		}

		#endregion
	}

	public static class ClientSettings
	{
		#region Methods

		// Methods
		public static void RegisterClientSideProviderAssembly(AssemblyName assemblyName)
		{
			Utility.ValidateArgumentNonNull(assemblyName, "assemblyName");

			// Load the assembly
			Assembly assembly = null;
			try
			{
				assembly = Assembly.Load(assemblyName);
			}
			catch (FileNotFoundException)
			{
				throw new ProxyAssemblyNotLoadedException(string.Format("Assembly {0} not found", assemblyName));
			}

			// Find the official type
			var name = assemblyName.Name + ".UIAutomationClientSideProviders";
			var type = assembly.GetType(name);
			if (type == null)
			{
				throw new ProxyAssemblyNotLoadedException(string.Format("Could not find type {0} in assembly {1}", name, assemblyName));
			}

			// Find the descriptor table
			var field = type.GetField("ClientSideProviderDescriptionTable", BindingFlags.Public | BindingFlags.Static);
			if ((field == null) || (field.FieldType != typeof (ClientSideProviderDescription[])))
			{
				throw new ProxyAssemblyNotLoadedException(string.Format("Could not find register method on type {0} in assembly {1}", name, assemblyName));
			}

			// Get the table value
			var clientSideProviderDescription = field.GetValue(null) as ClientSideProviderDescription[];

			// Write it through
			if (clientSideProviderDescription != null)
			{
				RegisterClientSideProviders(clientSideProviderDescription);
			}
		}

		public static void RegisterClientSideProviders(ClientSideProviderDescription[] clientSideProviderDescription)
		{
			Utility.ValidateArgumentNonNull(clientSideProviderDescription, "clientSideProviderDescription ");

			// Convert providers to native code representation
			var entriesList =
				new List<IUIAutomationProxyFactoryEntry>();
			foreach (var provider in clientSideProviderDescription)
			{
				// Construct a wrapper for the proxy factory callback
				Utility.ValidateArgumentNonNull(provider.ClientSideProviderFactoryCallback, "provider.ClientSideProviderFactoryCallback");
				var wrapper = new ProxyFactoryCallbackWrapper(provider.ClientSideProviderFactoryCallback);

				// Construct a factory entry
				var factoryEntry =
					Automation.Factory.CreateProxyFactoryEntry(wrapper);
				factoryEntry.AllowSubstringMatch = ((provider.Flags & ClientSideProviderMatchIndicator.AllowSubstringMatch) != 0) ? 1 : 0;
				factoryEntry.CanCheckBaseClass = ((provider.Flags & ClientSideProviderMatchIndicator.DisallowBaseClassNameMatch) != 0) ? 0 : 1;
				factoryEntry.ClassName = provider.ClassName;
				factoryEntry.ImageName = provider.ImageName;

				// Add it to the list
				entriesList.Add(factoryEntry);
			}

			// Get the proxy map from Automation and restore the default table
			var map = Automation.Factory.ProxyFactoryMapping;
			map.RestoreDefaultTable();

			// Decide where to insert
			// MSDN recommends inserting after non-control and container proxies
			uint insertBefore;
			var count = map.count;
			for (insertBefore = 0; insertBefore < count; ++insertBefore)
			{
				var proxyFactoryId = map.GetEntry(insertBefore).ProxyFactory.ProxyFactoryId;
				if (!proxyFactoryId.Contains("Non-Control") && !proxyFactoryId.Contains("Container"))
				{
					break;
				}
			}

			// Insert our new entries
			map.InsertEntries(insertBefore, entriesList.ToArray());
		}

		#endregion
	}
}