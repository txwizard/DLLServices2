using System;
using System.IO;
using System.Text;
using System.Windows.Forms;


namespace KeysEnumLab
{
	class Program
	{
		const int ARRAY_SUBSCRIPT_FIRST = 0;
		const int BUFFER_SIZE_SWEET_SPOT = 8192;
		const int EMPTY_STRING_LENGTH = 0;
		const bool STREAMWRITER_OVERWRITE = false;
		const char TAB_CHAR = '\t';

		static string s_strProgramName = System.IO.Path.GetFileNameWithoutExtension (
			System.Reflection.Assembly.GetExecutingAssembly ( ).Location );
		static DateTime s_dtmStartupTime = System.Diagnostics.Process.GetCurrentProcess ( ).StartTime.ToUniversalTime ( );

		static string s_strKeysMsgFormat = Properties.Resources.KEYS_MSG;
		static string s_strKeysLstFormat = Properties.Resources.KEYS_LST;

		static void Main ( string [ ] args )
		{
			Console.WriteLine (
				Properties.Resources.BOJ_MSG ,
				s_strProgramName ,
				s_dtmStartupTime ,
				Environment.NewLine );

			StreamWriter twKeysInfo = ( args.Length > EMPTY_STRING_LENGTH ? new StreamWriter ( args [ ARRAY_SUBSCRIPT_FIRST ] , STREAMWRITER_OVERWRITE , Encoding.ASCII , BUFFER_SIZE_SWEET_SPOT ) : null );

			if ( twKeysInfo != null )
			{
				twKeysInfo.WriteLine (
					Properties.Resources.KEYS_LBL ,
					new object [ ] {
						Properties.Resources.KEYS_NAME ,
						Properties.Resources.KEYS_VALUE_HEX ,
						Properties.Resources.KEYS_VALUE_DEC ,
						TAB_CHAR } );
			}	// if ( twKeysInfo != null )

			DisplayKeys ( Keys.A , twKeysInfo );
			DisplayKeys ( Keys.Add , twKeysInfo );
			DisplayKeys ( Keys.Alt , twKeysInfo );
			DisplayKeys ( Keys.Apps , twKeysInfo );
			DisplayKeys ( Keys.Attn , twKeysInfo );
			DisplayKeys ( Keys.B , twKeysInfo );
			DisplayKeys ( Keys.Back , twKeysInfo );
			DisplayKeys ( Keys.BrowserBack , twKeysInfo );
			DisplayKeys ( Keys.BrowserFavorites , twKeysInfo );
			DisplayKeys ( Keys.BrowserForward , twKeysInfo );
			DisplayKeys ( Keys.BrowserHome , twKeysInfo );
			DisplayKeys ( Keys.BrowserRefresh , twKeysInfo );
			DisplayKeys ( Keys.BrowserSearch , twKeysInfo );
			DisplayKeys ( Keys.BrowserStop , twKeysInfo );
			DisplayKeys ( Keys.C , twKeysInfo );
			DisplayKeys ( Keys.Cancel , twKeysInfo );
			DisplayKeys ( Keys.Capital , twKeysInfo );
			DisplayKeys ( Keys.CapsLock , twKeysInfo );
			DisplayKeys ( Keys.Clear , twKeysInfo );
			DisplayKeys ( Keys.Control , twKeysInfo );
			DisplayKeys ( Keys.ControlKey , twKeysInfo );
			DisplayKeys ( Keys.Crsel , twKeysInfo );
			DisplayKeys ( Keys.D , twKeysInfo );
			DisplayKeys ( Keys.D0 , twKeysInfo );
			DisplayKeys ( Keys.D1 , twKeysInfo );
			DisplayKeys ( Keys.D2 , twKeysInfo );
			DisplayKeys ( Keys.D3 , twKeysInfo );
			DisplayKeys ( Keys.D4 , twKeysInfo );
			DisplayKeys ( Keys.D5 , twKeysInfo );
			DisplayKeys ( Keys.D6 , twKeysInfo );
			DisplayKeys ( Keys.D7 , twKeysInfo );
			DisplayKeys ( Keys.D8 , twKeysInfo );
			DisplayKeys ( Keys.D9 , twKeysInfo );
			DisplayKeys ( Keys.Decimal , twKeysInfo );
			DisplayKeys ( Keys.Delete , twKeysInfo );
			DisplayKeys ( Keys.Divide , twKeysInfo );
			DisplayKeys ( Keys.Down , twKeysInfo );
			DisplayKeys ( Keys.E , twKeysInfo );
			DisplayKeys ( Keys.End , twKeysInfo );
			DisplayKeys ( Keys.Enter , twKeysInfo );
			DisplayKeys ( Keys.EraseEof , twKeysInfo );
			DisplayKeys ( Keys.Escape , twKeysInfo );
			DisplayKeys ( Keys.Execute , twKeysInfo );
			DisplayKeys ( Keys.Exsel , twKeysInfo );
			DisplayKeys ( Keys.F , twKeysInfo );
			DisplayKeys ( Keys.F1 , twKeysInfo );
			DisplayKeys ( Keys.F10 , twKeysInfo );
			DisplayKeys ( Keys.F11 , twKeysInfo );
			DisplayKeys ( Keys.F12 , twKeysInfo );
			DisplayKeys ( Keys.F13 , twKeysInfo );
			DisplayKeys ( Keys.F14 , twKeysInfo );
			DisplayKeys ( Keys.F15 , twKeysInfo );
			DisplayKeys ( Keys.F16 , twKeysInfo );
			DisplayKeys ( Keys.F17 , twKeysInfo );
			DisplayKeys ( Keys.F18 , twKeysInfo );
			DisplayKeys ( Keys.F19 , twKeysInfo );
			DisplayKeys ( Keys.F2 , twKeysInfo );
			DisplayKeys ( Keys.F20 , twKeysInfo );
			DisplayKeys ( Keys.F21 , twKeysInfo );
			DisplayKeys ( Keys.F22 , twKeysInfo );
			DisplayKeys ( Keys.F23 , twKeysInfo );
			DisplayKeys ( Keys.F24 , twKeysInfo );
			DisplayKeys ( Keys.F3 , twKeysInfo );
			DisplayKeys ( Keys.F4 , twKeysInfo );
			DisplayKeys ( Keys.F5 , twKeysInfo );
			DisplayKeys ( Keys.F6 , twKeysInfo );
			DisplayKeys ( Keys.F7 , twKeysInfo );
			DisplayKeys ( Keys.F8 , twKeysInfo );
			DisplayKeys ( Keys.F9 , twKeysInfo );
			DisplayKeys ( Keys.FinalMode , twKeysInfo );
			DisplayKeys ( Keys.G , twKeysInfo );
			DisplayKeys ( Keys.H , twKeysInfo );
			DisplayKeys ( Keys.HanguelMode , twKeysInfo );
			DisplayKeys ( Keys.HangulMode , twKeysInfo );
			DisplayKeys ( Keys.HanjaMode , twKeysInfo );
			DisplayKeys ( Keys.Help , twKeysInfo );
			DisplayKeys ( Keys.Home , twKeysInfo );
			DisplayKeys ( Keys.I , twKeysInfo );
			DisplayKeys ( Keys.IMEAccept , twKeysInfo );
			DisplayKeys ( Keys.IMEAceept , twKeysInfo );
			DisplayKeys ( Keys.IMEConvert , twKeysInfo );
			DisplayKeys ( Keys.IMEModeChange , twKeysInfo );
			DisplayKeys ( Keys.IMENonconvert , twKeysInfo );
			DisplayKeys ( Keys.Insert , twKeysInfo );
			DisplayKeys ( Keys.J , twKeysInfo );
			DisplayKeys ( Keys.JunjaMode , twKeysInfo );
			DisplayKeys ( Keys.K , twKeysInfo );
			DisplayKeys ( Keys.KanaMode , twKeysInfo );
			DisplayKeys ( Keys.KanjiMode , twKeysInfo );
			DisplayKeys ( Keys.KeyCode , twKeysInfo );
			DisplayKeys ( Keys.L , twKeysInfo );
			DisplayKeys ( Keys.LaunchApplication1 , twKeysInfo );
			DisplayKeys ( Keys.LaunchApplication2 , twKeysInfo );
			DisplayKeys ( Keys.LaunchMail , twKeysInfo );
			DisplayKeys ( Keys.LButton , twKeysInfo );
			DisplayKeys ( Keys.LControlKey , twKeysInfo );
			DisplayKeys ( Keys.Left , twKeysInfo );
			DisplayKeys ( Keys.LineFeed , twKeysInfo );
			DisplayKeys ( Keys.LMenu , twKeysInfo );
			DisplayKeys ( Keys.LShiftKey , twKeysInfo );
			DisplayKeys ( Keys.LWin , twKeysInfo );
			DisplayKeys ( Keys.M , twKeysInfo );
			DisplayKeys ( Keys.MButton , twKeysInfo );
			DisplayKeys ( Keys.MediaNextTrack , twKeysInfo );
			DisplayKeys ( Keys.MediaPlayPause , twKeysInfo );
			DisplayKeys ( Keys.MediaPreviousTrack , twKeysInfo );
			DisplayKeys ( Keys.MediaStop , twKeysInfo );
			DisplayKeys ( Keys.Menu , twKeysInfo );
			DisplayKeys ( Keys.Modifiers , twKeysInfo );
			DisplayKeys ( Keys.Multiply , twKeysInfo );
			DisplayKeys ( Keys.N , twKeysInfo );
			DisplayKeys ( Keys.Next , twKeysInfo );
			DisplayKeys ( Keys.NoName , twKeysInfo );
			DisplayKeys ( Keys.None , twKeysInfo );
			DisplayKeys ( Keys.NumLock , twKeysInfo );
			DisplayKeys ( Keys.NumPad0 , twKeysInfo );
			DisplayKeys ( Keys.NumPad1 , twKeysInfo );
			DisplayKeys ( Keys.NumPad2 , twKeysInfo );
			DisplayKeys ( Keys.NumPad3 , twKeysInfo );
			DisplayKeys ( Keys.NumPad4 , twKeysInfo );
			DisplayKeys ( Keys.NumPad5 , twKeysInfo );
			DisplayKeys ( Keys.NumPad6 , twKeysInfo );
			DisplayKeys ( Keys.NumPad7 , twKeysInfo );
			DisplayKeys ( Keys.NumPad8 , twKeysInfo );
			DisplayKeys ( Keys.NumPad9 , twKeysInfo );
			DisplayKeys ( Keys.O , twKeysInfo );
			DisplayKeys ( Keys.Oem1 , twKeysInfo );
			DisplayKeys ( Keys.Oem102 , twKeysInfo );
			DisplayKeys ( Keys.Oem2 , twKeysInfo );
			DisplayKeys ( Keys.Oem3 , twKeysInfo );
			DisplayKeys ( Keys.Oem4 , twKeysInfo );
			DisplayKeys ( Keys.Oem5 , twKeysInfo );
			DisplayKeys ( Keys.Oem6 , twKeysInfo );
			DisplayKeys ( Keys.Oem7 , twKeysInfo );
			DisplayKeys ( Keys.Oem8 , twKeysInfo );
			DisplayKeys ( Keys.OemBackslash , twKeysInfo );
			DisplayKeys ( Keys.OemClear , twKeysInfo );
			DisplayKeys ( Keys.OemCloseBrackets , twKeysInfo );
			DisplayKeys ( Keys.Oemcomma , twKeysInfo );
			DisplayKeys ( Keys.OemMinus , twKeysInfo );
			DisplayKeys ( Keys.OemOpenBrackets , twKeysInfo );
			DisplayKeys ( Keys.OemPeriod , twKeysInfo );
			DisplayKeys ( Keys.OemPipe , twKeysInfo );
			DisplayKeys ( Keys.Oemplus , twKeysInfo );
			DisplayKeys ( Keys.OemQuestion , twKeysInfo );
			DisplayKeys ( Keys.OemQuotes , twKeysInfo );
			DisplayKeys ( Keys.OemSemicolon , twKeysInfo );
			DisplayKeys ( Keys.Oemtilde , twKeysInfo );
			DisplayKeys ( Keys.P , twKeysInfo );
			DisplayKeys ( Keys.Pa1 , twKeysInfo );
			DisplayKeys ( Keys.Packet , twKeysInfo );
			DisplayKeys ( Keys.PageDown , twKeysInfo );
			DisplayKeys ( Keys.PageUp , twKeysInfo );
			DisplayKeys ( Keys.Pause , twKeysInfo );
			DisplayKeys ( Keys.Play , twKeysInfo );
			DisplayKeys ( Keys.Print , twKeysInfo );
			DisplayKeys ( Keys.PrintScreen , twKeysInfo );
			DisplayKeys ( Keys.Prior , twKeysInfo );
			DisplayKeys ( Keys.ProcessKey , twKeysInfo );
			DisplayKeys ( Keys.Q , twKeysInfo );
			DisplayKeys ( Keys.R , twKeysInfo );
			DisplayKeys ( Keys.RButton , twKeysInfo );
			DisplayKeys ( Keys.RControlKey , twKeysInfo );
			DisplayKeys ( Keys.Return , twKeysInfo );
			DisplayKeys ( Keys.Right , twKeysInfo );
			DisplayKeys ( Keys.RMenu , twKeysInfo );
			DisplayKeys ( Keys.RShiftKey , twKeysInfo );
			DisplayKeys ( Keys.RWin , twKeysInfo );
			DisplayKeys ( Keys.S , twKeysInfo );
			DisplayKeys ( Keys.Scroll , twKeysInfo );
			DisplayKeys ( Keys.Select , twKeysInfo );
			DisplayKeys ( Keys.SelectMedia , twKeysInfo );
			DisplayKeys ( Keys.Separator , twKeysInfo );
			DisplayKeys ( Keys.Shift , twKeysInfo );
			DisplayKeys ( Keys.ShiftKey , twKeysInfo );
			DisplayKeys ( Keys.Sleep , twKeysInfo );
			DisplayKeys ( Keys.Snapshot , twKeysInfo );
			DisplayKeys ( Keys.Space , twKeysInfo );
			DisplayKeys ( Keys.Subtract , twKeysInfo );
			DisplayKeys ( Keys.T , twKeysInfo );
			DisplayKeys ( Keys.Tab , twKeysInfo );
			DisplayKeys ( Keys.U , twKeysInfo );
			DisplayKeys ( Keys.Up , twKeysInfo );
			DisplayKeys ( Keys.V , twKeysInfo );
			DisplayKeys ( Keys.VolumeDown , twKeysInfo );
			DisplayKeys ( Keys.VolumeMute , twKeysInfo );
			DisplayKeys ( Keys.VolumeUp , twKeysInfo );
			DisplayKeys ( Keys.W , twKeysInfo );
			DisplayKeys ( Keys.X , twKeysInfo );
			DisplayKeys ( Keys.XButton1 , twKeysInfo );
			DisplayKeys ( Keys.XButton2 , twKeysInfo );
			DisplayKeys ( Keys.Y , twKeysInfo );
			DisplayKeys ( Keys.Z , twKeysInfo );
			DisplayKeys ( Keys.Zoom , twKeysInfo );

			if ( twKeysInfo != null )
			{
				twKeysInfo.Flush ( );
				twKeysInfo.Close ( );
				twKeysInfo.Dispose ( );

				FileInfo fiKeysInfo = new FileInfo ( args [ ARRAY_SUBSCRIPT_FIRST ] );

				Console.WriteLine (
					Properties.Resources.KEYS_FILE_MSG ,
					fiKeysInfo.Length ,
					fiKeysInfo.FullName ,
					Environment.NewLine );
			}	// if ( twKeysInfo != null )

			Console.WriteLine (
				Properties.Resources.EOJ_MSG,
				s_strProgramName ,
				new TimeSpan (
					System.DateTime.UtcNow.Ticks
					- s_dtmStartupTime.Ticks ) ,
				Environment.NewLine );
			Environment.Exit ( 0 );
		}	// static void Main


		private static void DisplayKeys (
			Keys pkeys ,
			StreamWriter ptwKeysInfo )
		{
			Console.WriteLine (
				s_strKeysMsgFormat ,
				pkeys ,
				( int ) pkeys );

			if ( ptwKeysInfo != null )
			{
				ptwKeysInfo.WriteLine (
					s_strKeysLstFormat ,
					pkeys ,
					( int ) pkeys ,
					TAB_CHAR );
			}	// if ( ptwKeysInfo != null )
		}	// private static void DisplayKeys
	}	// class Program
}	// namespace KeysEnumLab