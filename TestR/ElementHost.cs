#region References

using System;
using System.Collections.Generic;

#endregion

namespace TestR
{
	/// <summary>
	/// Represents a host for a set of elements.
	/// </summary>
	public abstract class ElementHost : IDisposable
	{
		#region Constructors

		/// <summary>
		/// Instantiates an instance of an element host.
		/// </summary>
		protected internal ElementHost(Application application, ElementHost parent)
		{
			Application = application;
			Children = new ElementCollection(this);
			Parent = parent;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the application for this element host.
		/// </summary>
		public Application Application { get; protected set; }

		/// <summary>
		/// Gets a hierarchical list of elements.
		/// </summary>
		public ElementCollection Children { get; }

		/// <summary>
		/// Gets the current focused element.
		/// </summary>
		public abstract Element FocusedElement { get; }

		/// <summary>
		/// Gets the ID of this element host.
		/// </summary>
		public abstract string Id { get; }

		/// <summary>
		/// Gets or sets the name of the element.
		/// </summary>
		public virtual string Name => Id;

		/// <summary>
		/// Gets the parent element of this element.
		/// </summary>
		public ElementHost Parent { get; }

		#endregion

		#region Methods

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// First the first child of the specified type.
		/// </summary>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The first child element of the type or null if no child found. </returns>
		public T First<T>(bool recursive = true, bool wait = true) where T : Element
		{
			return First<T>(x => true, recursive, wait);
		}

		/// <summary>
		/// First the first child from the children.
		/// </summary>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The first child element or null if no child found. </returns>
		public Element First(bool recursive = true, bool wait = true)
		{
			return First<Element>(x => true, recursive, wait);
		}

		/// <summary>
		/// First the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID or null if no child found. </returns>
		public Element First(string id, bool recursive = true, bool wait = true)
		{
			return First<Element>(id, recursive, wait);
		}

		/// <summary>
		/// First the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition or null if no child found. </returns>
		public Element First(Func<Element, bool> condition, bool recursive = true, bool wait = true)
		{
			return First<Element>(condition, recursive, wait);
		}

		/// <summary>
		/// First the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID or null if no child found. </returns>
		public T First<T>(string id, bool recursive = true, bool wait = true) where T : Element
		{
			return First<T>(x => (x.FullId == id) || (x.Id == id) || (x.Name == id), recursive, wait);
		}

		/// <summary>
		/// First the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition or null if no child found. </returns>
		public T First<T>(Func<T, bool> condition, bool recursive = true, bool wait = true) where T : Element
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = Children.First(condition, recursive);
					if ((response != null) || !wait)
					{
						return true;
					}

					UpdateChildren();
					return false;
				}
				catch (Exception)
				{
					return !wait;
				}
			}, Application.Timeout.TotalMilliseconds);

			return response;
		}

		/// <summary>
		/// Get all the children.
		/// </summary>
		/// <returns> The child elements. </returns>
		public IEnumerable<Element> Descendants()
		{
			return Children.Descendants(x => true);
		}
		
		/// <summary>
		/// Get all the children that match the optional condition. If a condition is not provided
		/// then all children of the type will be returned.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <returns> The child elements for the condition. </returns>
		public IEnumerable<Element> Descendants(Func<Element, bool> condition)
		{
			return Children.Descendants(condition);
		}

		/// <summary>
		/// Get all the children of a specific type.
		/// </summary>
		/// <returns> The child elements of the specific type. </returns>
		public IEnumerable<T> Descendants<T>() where T : Element
		{
			return Descendants<T>(x => true);
		}

		/// <summary>
		/// Get all the children of a specific type that matches the condition.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <returns> The child elements of the specific type for the condition. </returns>
		public IEnumerable<T> Descendants<T>(Func<T, bool> condition) where T : Element
		{
			return Children.Descendants(condition);
		}

		/// <summary>
		/// Refresh the list of children for this host.
		/// </summary>
		public abstract ElementHost Refresh();

		/// <summary>
		/// Update the children for this element.
		/// </summary>
		public ElementHost UpdateChildren()
		{
			Refresh();
			OnChildrenUpdated();
			return this;
		}

		/// <summary>
		/// Waits for the host to complete the work. Will wait until no longer busy.
		/// </summary>
		/// <param name="minimumDelay"> The minimum delay in milliseconds to wait. Defaults to 0 milliseconds. </param>
		public abstract ElementHost WaitForComplete(int minimumDelay = 0);

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <param name="disposing"> True if disposing and false if otherwise. </param>
		protected abstract void Dispose(bool disposing);

		/// <summary>
		/// Invoke this method when the children changes.
		/// </summary>
		protected void OnChildrenUpdated()
		{
			ChildrenUpdated?.Invoke();
		}

		/// <summary>
		/// Invoke this method when the host is closing.
		/// </summary>
		protected virtual void OnClosed()
		{
			Closed?.Invoke();
		}

		/// <summary>
		/// Handles the excited event.
		/// </summary>
		/// <param name="sender"> </param>
		/// <param name="e"> </param>
		private void OnExited(object sender, EventArgs e)
		{
			Exited?.Invoke();
		}

		#endregion

		#region Events

		/// <summary>
		/// Occurs when the children are updated.
		/// </summary>
		public event Action ChildrenUpdated;

		/// <summary>
		/// Event called when the element host closes.
		/// </summary>
		public event Action Closed;

		/// <summary>
		/// Occurs when the element host exits.
		/// </summary>
		public event Action Exited;

		#endregion
	}
}