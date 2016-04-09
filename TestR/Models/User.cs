namespace TestR.Models
{
	/// <summary>
	/// Represents a user in the TestR domain.
	/// </summary>
	public class User : Entity
	{
		#region Properties

		/// <summary>
		/// Gets or sets the email address.
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the hash for the password.
		/// </summary>
		public string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets the salt for the password. Used during the password hashing.
		/// </summary>
		public string PasswordSalt { get; set; }

		/// <summary>
		/// Gets or sets a comma delimited list of user roles.
		/// </summary>
		public string Roles { get; set; }

		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		public string UserName { get; set; }

		#endregion
	}
}