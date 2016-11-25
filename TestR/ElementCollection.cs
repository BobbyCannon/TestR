#region References

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TestR.Extensions;

#endregion

namespace TestR
{
    /// <summary>
    /// Represents a collection of elements.
    /// </summary>
    public class ElementCollection<T> : ObservableCollection<T>
        where T : BaseElement
    {
        #region Constructors

        /// <summary>
        /// Initializes an instance of the ElementCollection class.
        /// </summary>
        public ElementCollection()
        {
        }

        /// <summary>
        /// Initializes an instance of the ElementCollection class.
        /// </summary>
        /// <param name="collection"> The collection of elements to add to the new collection. </param>
        public ElementCollection(IEnumerable<T> collection)
        {
            this.AddRange(collection);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Access an element by the Application ID, ID, or Name.
        /// </summary>
        /// <param name="id"> The ID of the element. </param>
        /// <returns> The element if found or null if not found. </returns>
        public T this[string id] => Get<T>(id, false);

        /// <summary>
        /// Gets the parent for this collection of elements.
        /// </summary>
        public BaseElement Parent { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Check to see if this collection contains an element.
        /// </summary>
        /// <param name="id"> The id to search for. </param>
        /// <returns> True if the id is found, false if otherwise. </returns>
        public bool Contains(string id)
        {
            return this[id] != null;
        }

        /// <summary>
        /// Get an element from the collection using the provided condition.
        /// </summary>
        /// <param name="condition"> A function to test each element for a condition. </param>
        /// <param name="includeDescendants"> The flag that determines to include descendants or not. </param>
        /// <returns> The element matching the condition. </returns>
        public BaseElement Get(Func<BaseElement, bool> condition, bool includeDescendants = true)
        {
            return Get<BaseElement>(condition, includeDescendants);
        }

        /// <summary>
        /// Get an element from the collection using the provided ID.
        /// </summary>
        /// <param name="id"> An ID of the element to get. </param>
        /// <param name="includeDescendants"> The flag that determines to include descendants or not. </param>
        /// <returns> The child element for the condition. </returns>
        public T1 Get<T1>(string id, bool includeDescendants = true) where T1 : BaseElement
        {
            return Get<T1>(x => (x.FullId == id) || (x.Id == id) || (x.Name == id), includeDescendants);
        }

        /// <summary>
        /// Get an element from the collection using the provided condition.
        /// </summary>
        /// <param name="condition"> A function to test each element for a condition. </param>
        /// <param name="includeDescendants"> The flag that determines to include descendants or not. </param>
        /// <returns> The child element for the condition. </returns>
        public T1 Get<T1>(Func<T1, bool> condition, bool includeDescendants = true) where T1 : BaseElement
        {
            var children = OfType<T1>().ToList();
            var response = children.FirstOrDefault(condition);

            if (!includeDescendants)
            {
                return response;
            }

            if (response != null)
            {
                return response;
            }

            foreach (var child in this)
            {
                response = child.Get(condition, true, false);
                if (response != null)
                {
                    return response;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a collection of element of the provided type.
        /// </summary>
        /// <typeparam name="T1"> The type of the element for the collection. </typeparam>
        /// <returns> The collection of elements of the provided type. </returns>
        public ElementCollection<T1> OfType<T1>() where T1 : BaseElement
        {
            return new ElementCollection<T1>(this.Where(x => (x.GetType() == typeof(T1)) || x is T1).Cast<T1>());
        }

        /// <summary>
        /// Prints out all children as a debug string.
        /// </summary>
        /// <param name="prefix"> Prefix to the debug information. </param>
        /// <param name="verbose"> Option to print verbose information. </param>
        public ElementCollection<T> PrintDebug(string prefix = "", bool verbose = true)
        {
            foreach (var item in this)
            {
                if (verbose)
                {
                    Console.WriteLine(prefix + item.ToDetailString().Replace(Environment.NewLine, ", "));
                }
                else
                {
                    Console.WriteLine(prefix + item.FullId);
                }

                item.Children.PrintDebug(prefix + "    ", verbose);
            }

            return this;
        }

        /// <summary>
        /// Add the element to the collection.
        /// </summary>
        /// <param name="element"> The type of the element. </param>
        /// <returns> Returns the element that was added. </returns>
        private T AddElement(T element)
        {
            Add(element);
            return element;
        }

        #endregion
    }
}