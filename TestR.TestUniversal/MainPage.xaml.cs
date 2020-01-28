﻿#region References

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#endregion

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TestR.TestUniversal
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		#region Constructors

		public MainPage()
		{
			InitializeComponent();
		}

		#endregion

		#region Methods

		private void ButtonOnClick(object sender, RoutedEventArgs e)
		{
			TextBlock.Text = "Button";
		}

		#endregion
	}
}