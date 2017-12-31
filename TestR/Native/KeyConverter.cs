#region References

using System.Windows.Input;

#endregion

namespace TestR.Native
{
	/// <summary>
	/// Converts key to and from ascii values.
	/// </summary>
	public static class KeyConverter
	{
		#region Methods

		/// <summary>
		/// Converts the ascii value to key.
		/// </summary>
		/// <param name="value"> The ascii value to convert. </param>
		/// <returns> The key value converted from value. </returns>
		public static Key AsciiToKeyValue(int value)
		{
			switch (value)
			{
				case 0x30:
					return Key.D0;

				case 0x31:
					return Key.D1;

				case 0x32:
					return Key.D2;

				case 0x33:
					return Key.D3;

				case 0x34:
					return Key.D4;

				case 0x35:
					return Key.D5;

				case 0x36:
					return Key.D6;

				case 0x37:
					return Key.D7;

				case 0x38:
					return Key.D8;

				case 0x39:
					return Key.D9;

				case 0x41:
				case 0x61:
					return Key.A;

				case 0x42:
				case 0x62:
					return Key.B;

				case 0x43:
				case 0x63:
					return Key.C;

				case 0x44:
				case 0x64:
					return Key.D;

				case 0x45:
				case 0x65:
					return Key.E;

				case 0x46:
				case 0x66:
					return Key.F;

				case 0x47:
				case 0x67:
					return Key.G;

				case 0x48:
				case 0x68:
					return Key.H;

				case 0x49:
				case 0x69:
					return Key.I;

				case 0x4A:
				case 0x6A:
					return Key.J;

				case 0x4B:
				case 0x6B:
					return Key.K;

				case 0x4C:
				case 0x6C:
					return Key.L;

				case 0x4D:
				case 0x6D:
					return Key.M;

				case 0x4E:
				case 0x6E:
					return Key.N;

				case 0x4F:
				case 0x6F:
					return Key.O;

				case 0x50:
				case 0x70:
					return Key.P;

				case 0x51:
				case 0x71:
					return Key.Q;

				case 0x52:
				case 0x72:
					return Key.R;

				case 0x53:
				case 0x73:
					return Key.S;

				case 0x54:
				case 0x74:
					return Key.T;

				case 0x55:
				case 0x75:
					return Key.U;

				case 0x56:
				case 0x76:
					return Key.V;

				case 0x57:
				case 0x77:
					return Key.W;

				case 0x58:
				case 0x78:
					return Key.X;

				case 0x59:
				case 0x79:
					return Key.Y;

				case 0x5A:
				case 0x7A:
					return Key.Z;

				case 0x08:
					return Key.Back;

				case 0x18:
					return Key.Cancel;

				case 0x1B:
					return Key.Escape;

				case 0x7F:
					return Key.Delete;

				case 0x0A:
					return Key.LineFeed;

				case 0x0D:
					return Key.Return;

				case 0x20:
					return Key.Space;

				case 0x09:
					return Key.Tab;

				case 0x22:
					return Key.OemQuotes;

				case 0x7B:
					return Key.OemOpenBrackets;

				case 0x7C:
					return Key.OemPipe;
				
				case 0x7D:
					return Key.OemCloseBrackets;
				
				case 0x7E:
					return Key.OemTilde;

				case 0x00:
				case 0x01:
				case 0x02:
				case 0x03:
				case 0x04:
				case 0x05:
				case 0x06:
				case 0x07:
				case 0x0B:
				case 0x0C:
				case 0x0E:
				case 0x0F:
				case 0x10:
				case 0x11:
				case 0x12:
				case 0x13:
				case 0x14:
				case 0x15:
				case 0x16:
				case 0x17:
				case 0x19:
				case 0x1A:
				case 0x1C:
				case 0x1D:
				case 0x1E:
				case 0x1F:
				case 0x21:
				case 0x24:
				case 0x25:
				case 0x26:
				case 0x27:
				case 0x28:
				case 0x29:
				case 0x2A:
				case 0x2B:
				case 0x2C:
				case 0x2D:
				case 0x2E:
				case 0x5B:
				case 0x5C:
				case 0x5D:
				case 0x5E:
				case 0x5F:
				case 0x60:
				case 0x80:
				case 0x81:
				case 0x82:
				case 0x83:
				case 0x84:
				case 0x85:
				case 0x86:
				case 0x87:
				case 0x88:
				case 0x89:
				case 0x8A:
				case 0x8B:
				case 0x8C:
				case 0x8D:
				case 0x8E:
				case 0x8F:
				case 0x90:
				case 0x91:
				case 0x92:
				case 0x93:
				case 0x94:
				case 0x95:
				case 0x96:
				case 0x97:
				case 0x98:
				case 0x99:
				case 0x9A:
				case 0x9B:
				case 0x9C:
				case 0x9D:
				case 0x9E:
				case 0x9F:
				case 0xA0:
				case 0xA1:
				case 0xA2:
				case 0xA3:
				case 0xA4:
				case 0xA5:
				case 0xA6:
				case 0xA7:
				case 0xA8:
				case 0xA9:
				case 0xAA:
				case 0xAB:
				case 0xAC:
				case 0xAD:
				case 0xAE:
				case 0xAF:
				case 0xB0:
				case 0xB1:
				case 0xB2:
				case 0xB3:
				case 0xB4:
				case 0xB5:
				case 0xB6:
				case 0xB7:
				case 0xB8:
				case 0xB9:
				case 0xBA:
				case 0xBB:
				case 0xBC:
				case 0xBD:
				case 0xBE:
				case 0xBF:
				case 0xC0:
				case 0xC1:
				case 0xC2:
				case 0xC3:
				case 0xC4:
				case 0xC5:
				case 0xC6:
				case 0xC7:
				case 0xC8:
				case 0xC9:
				case 0xCA:
				case 0xCB:
				case 0xCC:
				case 0xCD:
				case 0xCE:
				case 0xCF:
				case 0xD0:
				case 0xD1:
				case 0xD2:
				case 0xD3:
				case 0xD4:
				case 0xD5:
				case 0xD6:
				case 0xD7:
				case 0xD8:
				case 0xD9:
				case 0xDA:
				case 0xDB:
				case 0xDC:
				case 0xDD:
				case 0xDE:
				case 0xDF:
				case 0xE0:
				case 0xE1:
				case 0xE2:
				case 0xE3:
				case 0xE4:
				case 0xE5:
				case 0xE6:
				case 0xE7:
				case 0xE8:
				case 0xE9:
				case 0xEA:
				case 0xEB:
				case 0xEC:
				case 0xED:
				case 0xEE:
				case 0xEF:
				case 0xF0:
				case 0xF1:
				case 0xF2:
				case 0xF3:
				case 0xF4:
				case 0xF5:
				case 0xF6:
				case 0xF7:
				case 0xF8:
				case 0xF9:
				case 0xFA:
				case 0xFB:
				case 0xFC:
				case 0xFD:
				case 0xFE:
				case 0xFF:
				default:
					return Key.DeadCharProcessed;
			}
		}

		/// <summary>
		/// Converts a key to the ascii value.
		/// </summary>
		/// <param name="key"> The key to convert. </param>
		/// <param name="isShiftPressed"> The flag to indicate the shift button is pressed. </param>
		/// <returns> The ascii value of the key. </returns>
		public static int KeyToAsciiValue(Key key, bool isShiftPressed)
		{
			switch (key)
			{
				case Key.D0:
					return 0x30;

				case Key.D1:
					return 0x31;

				case Key.D2:
					return 0x32;

				case Key.D3:
					return 0x33;

				case Key.D4:
					return 0x34;

				case Key.D5:
					return 0x35;

				case Key.D6:
					return 0x36;

				case Key.D7:
					return 0x37;

				case Key.D8:
					return 0x38;

				case Key.D9:
					return 0x39;

				case Key.A:
					return isShiftPressed ? 0x41 : 0x61;

				case Key.B:
					return isShiftPressed ? 0x42 : 0x62;

				case Key.C:
					return isShiftPressed ? 0x43 : 0x63;

				case Key.D:
					return isShiftPressed ? 0x44 : 0x64;

				case Key.E:
					return isShiftPressed ? 0x45 : 0x65;

				case Key.F:
					return isShiftPressed ? 0x46 : 0x66;

				case Key.G:
					return isShiftPressed ? 0x47 : 0x67;

				case Key.H:
					return isShiftPressed ? 0x48 : 0x68;

				case Key.I:
					return isShiftPressed ? 0x49 : 0x69;

				case Key.J:
					return isShiftPressed ? 0x4A : 0x6A;

				case Key.K:
					return isShiftPressed ? 0x4B : 0x6B;

				case Key.L:
					return isShiftPressed ? 0x4C : 0x6C;

				case Key.M:
					return isShiftPressed ? 0x4D : 0x6D;

				case Key.N:
					return isShiftPressed ? 0x4E : 0x6E;

				case Key.O:
					return isShiftPressed ? 0x4F : 0x6F;

				case Key.P:
					return isShiftPressed ? 0x50 : 0x70;

				case Key.Q:
					return isShiftPressed ? 0x51 : 0x71;

				case Key.R:
					return isShiftPressed ? 0x52 : 0x72;

				case Key.S:
					return isShiftPressed ? 0x53 : 0x73;

				case Key.T:
					return isShiftPressed ? 0x54 : 0x74;

				case Key.U:
					return isShiftPressed ? 0x55 : 0x75;

				case Key.V:
					return isShiftPressed ? 0x56 : 0x76;

				case Key.W:
					return isShiftPressed ? 0x57 : 0x77;

				case Key.X:
					return isShiftPressed ? 0x58 : 0x78;

				case Key.Y:
					return isShiftPressed ? 0x59 : 0x79;

				case Key.Z:
					return isShiftPressed ? 0x5A : 0x7A;

				case Key.Back:
					return 0x08;

				case Key.Cancel:
					return 0x18;

				case Key.Escape:
					return 0x1B;

				case Key.Delete:
					return 0x7F;

				case Key.LineFeed:
					return 0x0A;

				case Key.Return:
					return 0x0D;

				case Key.Space:
					return 0x20;

				case Key.Tab:
					return 0x09;

				case Key.OemQuotes:
					return 0x22;

				case Key.OemOpenBrackets:
					return 0x7B;

				case  Key.OemPipe:
					return 0x7C;
				
				case Key.OemCloseBrackets:
					return 0x7D;
				
				case Key.OemTilde:
					return 0x7E;

				case Key.None:
				case Key.Clear:
				case Key.Pause:
				case Key.Capital:
				case Key.KanaMode:
				case Key.JunjaMode:
				case Key.FinalMode:
				case Key.HanjaMode:
				case Key.ImeConvert:
				case Key.ImeNonConvert:
				case Key.ImeAccept:
				case Key.ImeModeChange:
				case Key.PageUp:
				case Key.Next:
				case Key.End:
				case Key.Home:
				case Key.Left:
				case Key.Up:
				case Key.Right:
				case Key.Down:
				case Key.Select:
				case Key.Print:
				case Key.Execute:
				case Key.Snapshot:
				case Key.Insert:
				case Key.Help:
				case Key.LWin:
				case Key.RWin:
				case Key.Apps:
				case Key.Sleep:
				case Key.NumPad0:
				case Key.NumPad1:
				case Key.NumPad2:
				case Key.NumPad3:
				case Key.NumPad4:
				case Key.NumPad5:
				case Key.NumPad6:
				case Key.NumPad7:
				case Key.NumPad8:
				case Key.NumPad9:
				case Key.Multiply:
				case Key.Add:
				case Key.Separator:
				case Key.Subtract:
				case Key.Decimal:
				case Key.Divide:
				case Key.F1:
				case Key.F2:
				case Key.F3:
				case Key.F4:
				case Key.F5:
				case Key.F6:
				case Key.F7:
				case Key.F8:
				case Key.F9:
				case Key.F10:
				case Key.F11:
				case Key.F12:
				case Key.F13:
				case Key.F14:
				case Key.F15:
				case Key.F16:
				case Key.F17:
				case Key.F18:
				case Key.F19:
				case Key.F20:
				case Key.F21:
				case Key.F22:
				case Key.F23:
				case Key.F24:
				case Key.NumLock:
				case Key.Scroll:
				case Key.LeftShift:
				case Key.RightShift:
				case Key.LeftCtrl:
				case Key.RightCtrl:
				case Key.LeftAlt:
				case Key.RightAlt:
				case Key.BrowserBack:
				case Key.BrowserForward:
				case Key.BrowserRefresh:
				case Key.BrowserStop:
				case Key.BrowserSearch:
				case Key.BrowserFavorites:
				case Key.BrowserHome:
				case Key.VolumeMute:
				case Key.VolumeDown:
				case Key.VolumeUp:
				case Key.MediaNextTrack:
				case Key.MediaPreviousTrack:
				case Key.MediaStop:
				case Key.MediaPlayPause:
				case Key.LaunchMail:
				case Key.SelectMedia:
				case Key.LaunchApplication1:
				case Key.LaunchApplication2:
				case Key.Oem1:
				case Key.OemPlus:
				case Key.OemComma:
				case Key.OemMinus:
				case Key.OemPeriod:
				case Key.OemQuestion:
				case Key.AbntC1:
				case Key.AbntC2:
				case Key.Oem8:
				case Key.OemBackslash:
				case Key.ImeProcessed:
				case Key.System:
				case Key.OemAttn:
				case Key.OemFinish:
				case Key.OemCopy:
				case Key.DbeSbcsChar:
				case Key.OemEnlw:
				case Key.OemBackTab:
				case Key.DbeNoRoman:
				case Key.DbeEnterWordRegisterMode:
				case Key.DbeEnterImeConfigureMode:
				case Key.EraseEof:
				case Key.Play:
				case Key.DbeNoCodeInput:
				case Key.NoName:
				case Key.Pa1:
				case Key.OemClear:
				case Key.DeadCharProcessed:
				default:
					return -1;
			}
		}

		#endregion
	}
}