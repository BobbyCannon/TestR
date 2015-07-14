#region References

using System;

#endregion

namespace TestR.Models
{
	/// <summary>
	/// Represents an entity.
	/// </summary>
	public abstract class Entity
	{
		#region Properties

		/// <summary>
		/// Gets or sets the date and time the entity was created.
		/// </summary>
		public DateTime CreatedOn { get; set; }

		/// <summary>
		/// Gets or sets the unique ID.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the date and time the entity was last modified on. If this matches the created on then the entity
		/// has never been modified.
		/// </summary>
		public DateTime ModifiedOn { get; set; }

		#endregion
	}
}