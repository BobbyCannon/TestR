#region References

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using TestR.Internal;

#endregion

namespace TestR.Desktop
{
	/// <summary>
	/// Represents a service that handling processes.
	/// </summary>
	public static class ProcessService
	{
		#region Fields

		private static readonly string[] _extensions;
		private static readonly string _query;

		#endregion

		#region Constructors

		static ProcessService()
		{
			_extensions = new[] { ".exe", ".com" };
			_query = "SELECT ProcessID, Name, CommandLine, ExecutablePath, Handle FROM Win32_Process";
		}

		#endregion

		#region Methods

		/// <summary>
		/// Get the safe process for the current process.
		/// </summary>
		/// <returns> The safe process for the current process or null if an issue occurs. </returns>
		public static SafeProcess GetCurrentProcess()
		{
			var process = new SafeProcess(Process.GetCurrentProcess());
			return !PopulateProcess(process) ? null : process;
		}

		/// <summary>
		/// Start an application and return the safe process representing it.
		/// </summary>
		/// <param name="filePath"> The file path for the application to start. </param>
		/// <param name="arguments"> The optional arguments for the application. </param>
		/// <returns> The safe process for the current process or null if an issue occurs. </returns>
		public static SafeProcess Start(string filePath, string arguments = null)
		{
			var info = new ProcessStartInfo { FileName = filePath, Arguments = arguments ?? string.Empty, UseShellExecute = true };
			var process = Process.Start(info);
			var response = new SafeProcess(process);

			if (PopulateProcess(response))
			{
				return response;
			}

			return Wait(filePath, arguments);
		}

		/// <summary>
		/// Creates a new instance of the universal application.
		/// </summary>
		/// <param name="packageFamilyName"> The application package family name. </param>
		/// <returns> The instance that represents the application. </returns>
		public static SafeProcess StartUniversal(string packageFamilyName)
		{
			return Start($@"shell:appsFolder\{packageFamilyName}!App");
		}

		/// <summary>
		/// Gets a list of safe processes by executable path.
		/// </summary>
		/// <param name="executablePathOrName"> The executable file path or name of the processes to load. </param>
		/// <param name="arguments"> The optional arguments the process was started with. </param>
		/// <returns> The processes for the executable path. </returns>
		public static IEnumerable<SafeProcess> Where(string executablePathOrName, string arguments = null)
		{
			using (var searcher = new ManagementObjectDisposer())
			{
				var hasExtension = _extensions.Any(x => executablePathOrName.EndsWith(x, StringComparison.OrdinalIgnoreCase));
				var query = _query + (hasExtension ? $" WHERE ExecutablePath LIKE '%{executablePathOrName.FormatForInnerString()}%'" : $" WHERE Name LIKE '{executablePathOrName}%'");

				if (!string.IsNullOrWhiteSpace(arguments))
				{
					query += $" AND CommandLine LIKE '%{arguments.FormatForInnerString()}%'";
				}

				foreach (var item in searcher.Search(query))
				{
					if (!ProcessItem(item, out SafeProcess safeProcess, x => true))
					{
						continue;
					}

					yield return safeProcess;
				}
			}
		}

		/// <summary>
		/// Gets a list of safe processes filtered by provided filter.
		/// </summary>
		/// <param name="filter"> The filter to reduce collection. </param>
		/// <returns> The processes that match the filter. </returns>
		public static IEnumerable<SafeProcess> Where(Func<SafeProcess, bool> filter)
		{
			using (var searcher = new ManagementObjectDisposer())
			{
				foreach (var item in searcher.Search(_query))
				{
					if (!ProcessItem(item, out SafeProcess safeProcess, filter))
					{
						continue;
					}

					yield return safeProcess;
				}
			}
		}

		/// <summary>
		/// Gets a list of safe processes filtered by provided filter.
		/// </summary>
		/// <param name="name"> The name of the process. </param>
		/// <param name="filter"> The filter to reduce collection. </param>
		/// <returns> The processes that match the filter. </returns>
		public static IEnumerable<SafeProcess> WhereByName(string name, Func<SafeProcess, bool> filter = null)
		{
			using (var searcher = new ManagementObjectDisposer())
			{
				foreach (var item in searcher.Search($"{_query} WHERE Name LIKE '%{name}%'"))
				{
					if (!ProcessItem(item, out SafeProcess safeProcess, filter ?? (x => true)))
					{
						continue;
					}

					yield return safeProcess;
				}
			}
		}

		internal static SafeProcess Wait(string name, Func<SafeProcess, bool> func, int timeoutInMilliseconds = 2000, int waitDelay = 10)
		{
			SafeProcess response = null;

			var result = Utility.Wait(() => (response = Where(name).FirstOrDefault(func)) != null, timeoutInMilliseconds, waitDelay);
			if (!result || response == null)
			{
				throw new Exception("Failed to find the process...");
			}

			return response;
		}

		private static bool PopulateProcess(SafeProcess safeProcess)
		{
			using (var searcher = new ManagementObjectDisposer())
			{
				var item = searcher.Search($"{_query} WHERE ProcessID = {safeProcess.Id}").FirstOrDefault();
				return item != null && PopulateProcess(item, safeProcess);
			}
		}

		private static bool PopulateProcess(ManagementBaseObject item, SafeProcess response)
		{
			var pName = item["Name"].ToString();
			var pArguments = item["CommandLine"]?.ToString() ?? string.Empty;
			var pFilePath = item["ExecutablePath"]?.ToString() ?? string.Empty;

			if (pArguments.StartsWith($"{pName} "))
			{
				pArguments = pArguments.Substring(pName.Length + 1);
			}
			else if (pArguments.StartsWith($"\"{pName}\" "))
			{
				pArguments = pArguments.Substring(pName.Length + 3);
			}
			else if (pArguments.StartsWith($"{pFilePath} "))
			{
				pArguments = pArguments.Substring(pFilePath.Length + 1);
			}
			else if (pArguments.StartsWith($"\"{pFilePath}\" "))
			{
				pArguments = pArguments.Substring(pFilePath.Length + 3);
			}

			response.Arguments = pArguments;
			response.FilePath = pFilePath;
			response.FileName = Path.GetFileName(pFilePath);
			response.Name = pName;
			return true;
		}

		private static bool ProcessItem(ManagementBaseObject item, out SafeProcess safeProcess, Func<SafeProcess, bool> filter)
		{
			safeProcess = null;
			var id = int.Parse(item["ProcessId"].ToString());

			try
			{
				var process = Process.GetProcessById(id);
				safeProcess = new SafeProcess(process);

				if (!PopulateProcess(item, safeProcess))
				{
					safeProcess.Dispose();
					return false;
				}

				if (!filter(safeProcess))
				{
					safeProcess.Dispose();
					return false;
				}
			}
			catch
			{
				safeProcess?.Dispose();
				return false;
			}

			return true;
		}

		private static SafeProcess Wait(string name, string arguments, int timeoutInMilliseconds = 2000, int waitDelay = 10)
		{
			SafeProcess response = null;

			var result = Utility.Wait(() => (response = Where(name, arguments).FirstOrDefault()) != null, timeoutInMilliseconds, waitDelay);
			if (!result || response == null)
			{
				throw new Exception("Failed to find the process...");
			}

			return response;
		}

		#endregion
	}
}