#region References

using System;
using System.Text;
using TestR.Extensions;

#endregion

namespace TestR
{
	/// <summary>
	/// Represents an automation element.
	/// </summary>
	public abstract class BaseElement
	{
		#region Constructors

		/// <summary>
		/// Instantiates an instance of an element.
		/// </summary>
		/// <param name="id"> </param>
		/// <param name="name"> </param>
		/// <param name="timeout"> </param>
		/// <param name="parent"> </param>
		protected BaseElement(string id, string name, TimeSpan timeout, BaseElement parent)
		{
			Children = new ElementCollection<BaseElement>();
			Id = id;
			Name = name;
			Timeout = timeout;
			Parent = parent;
		}

		#endregion

		#region Properties

		/// <summary>
		/// </summary>
		public ElementCollection<BaseElement> Children { get; set; }

		/// <summary>
		/// The full id of the element which include all parent ids prefixed to the id.
		/// </summary>
		/// <summary>
		/// Gets the ID of this element in the application. Includes full application namespace.
		/// </summary>
		public string FullId
		{
			get
			{
				var builder = new StringBuilder();
				var element = this;
				do
				{
					builder.Insert(0, new[] { element.Id, element.Name, " " }.FirstValue() + ",");
					element = element.Parent;
				} while (element != null);

				builder.Remove(builder.Length - 1, 1);
				return builder.ToString();
			}
		}

		/// <summary>
		/// The id of the element.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// The name of the element.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The parent element of this element.
		/// </summary>
		public BaseElement Parent { get; private set; }

		/// <summary>
		/// The timeout of the element.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public BaseElement Get(string id, bool recursive = true, bool wait = true)
		{
			return Get<BaseElement>(id, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="id"> An ID of the element to get. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the ID. </returns>
		public T Get<T>(string id, bool recursive = true, bool wait = true) where T : BaseElement
		{
			return Get<T>(x => (x.FullId == id) || (x.Id == id) || (x.Name == id), recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public BaseElement Get(Func<BaseElement, bool> condition, bool recursive = true, bool wait = true)
		{
			return Get<BaseElement>(condition, recursive, wait);
		}

		/// <summary>
		/// Get the child from the children.
		/// </summary>
		/// <param name="condition"> A function to test each element for a condition. </param>
		/// <param name="recursive"> Flag to determine to include descendants or not. </param>
		/// <param name="wait"> Wait for the child to be available. Will auto refresh on each pass. </param>
		/// <returns> The child element for the condition. </returns>
		public T Get<T>(Func<T, bool> condition, bool recursive = true, bool wait = true) where T : BaseElement
		{
			T response = null;

			Utility.Wait(() =>
			{
				try
				{
					response = Children.Get(condition, recursive);
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
			}, Timeout.TotalMilliseconds);

			return response;
		}

		public string ToDetailString()
		{
			return string.Empty;
		}

		public void UpdateChildren()
		{
		}

		#endregion
	}
}