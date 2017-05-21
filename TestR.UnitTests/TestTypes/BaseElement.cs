#region References

using System;
using System.Drawing;

#endregion

namespace TestR.UnitTests.TestTypes
{
	public class BaseElement : Element
	{
		#region Constructors

		public BaseElement(string id, string name, ElementHost host)
			: base(host.Application, host)
		{
			Id = id;
		}

		#endregion

		#region Properties

		public override bool Enabled { get; }

		public override bool Focused { get; }

		public override Element FocusedElement { get; }

		public override int Height { get; }

		public override string Id { get; }

		public override string this[string id]
		{
			get => throw new NotImplementedException();
			set => throw new NotImplementedException();
		}

		public override Point Location { get; }

		public override int Width { get; }

		#endregion

		#region Methods

		public override Element CaptureSnippet(string filePath)
		{
			throw new NotImplementedException();
		}

		public override Element Click(int x = 0, int y = 0)
		{
			throw new NotImplementedException();
		}

		public override Element Focus()
		{
			throw new NotImplementedException();
		}

		public override Element LeftClick(int x = 0, int y = 0)
		{
			throw new NotImplementedException();
		}

		public override Element MiddleClick(int x = 0, int y = 0)
		{
			throw new NotImplementedException();
		}

		public override Element MoveMouseTo(int x = 0, int y = 0)
		{
			throw new NotImplementedException();
		}

		public override ElementHost Refresh()
		{
			return this;
		}

		public override Element RightClick(int x = 0, int y = 0)
		{
			throw new NotImplementedException();
		}

		public override string ToDetailString()
		{
			throw new NotImplementedException();
		}

		public override ElementHost WaitForComplete(int minimumDelay = 0)
		{
			return this;
		}

		protected override void Dispose(bool disposing)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}