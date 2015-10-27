using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.Foundation;
using Microsoft.Graphics.Canvas.Text;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using System.Runtime.InteropServices;
using Windows.System;
using System.Text;

namespace win2d_text_game
{
    public static class Statics
    {
        public static int DebugStringsCount = 0;
        public static int DebugCalculateCount = 0;
        public static int DebugButtonClickCount = 0;
        public static string DebugControlInFocusString = "Focus: N/A";
        public static string DebugUpdateTimeString = "Update time: N/A";

        public static Random Random = new Random(DateTime.Now.Millisecond);
        public static CanvasTextFormat DefaultFont;
        public static CanvasTextFormat DefaultFontNoWrap;

        public static CanvasTextLayout UpArrow;
        public static CanvasTextLayout DoubleUpArrow;
        public static CanvasTextLayout DownArrow;
        public static CanvasTextLayout DoubleDownArrow;

        ///////////////////////////////////////////////

        #region Old Project Stuff
        public static int MouseX = 0;
        public static int MouseY = 0;
        public static int FrameCount = 0;

        // layout dimensions
        public static int RightColumnWidth = 800;
        public static int RightColumnPadding = 10;
        public static int LeftColumnPadding = 10;
        public static int LeftColumnWidth;
        public static Vector2 ColumnDividerTop;
        public static Vector2 ColumnDividerBottom;
        #endregion

        static Statics()
        {
            DefaultFont = new CanvasTextFormat();
            DefaultFont.FontFamily = "Arial";
            DefaultFont.FontSize = 14;
            DefaultFont.WordWrapping = CanvasWordWrapping.Wrap; //.NoWrap;

            DefaultFontNoWrap = new CanvasTextFormat();
            DefaultFontNoWrap.FontFamily = "Arial";
            DefaultFontNoWrap.FontSize = 14;
            DefaultFontNoWrap.WordWrapping = CanvasWordWrapping.NoWrap; //.NoWrap;

            // CanvasTextLayout objects are initialized in CreateResources
        }

        public static Color RandomColor()
        {
            int red = 20 + Statics.Random.Next(235);
            int green = 20 + Statics.Random.Next(235);
            int blue = 20 + Statics.Random.Next(235);

            return Color.FromArgb(255, (byte)red, (byte)green, (byte)blue);
        }

        public static string RandomString(this string[] array)
        {
            return array[Statics.Random.Next(array.Length)];
        }

        #region VirtualKeyToString
        [DllImport("user32.dll")]
        public static extern int ToUnicode(uint virtualKeyCode, uint scanCode,
            byte[] keyboardState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeConst = 64)]
            StringBuilder receivingBuffer,
            int bufferSize, uint flags);

        public static string VirtualKeyToString(VirtualKey keys, bool shift = false, bool altGr = false)
        {
            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];
            if (shift)
                keyboardState[(int)VirtualKey.Shift] = 0xff;
            if (altGr)
            {
                keyboardState[(int)VirtualKey.Control] = 0xff;
                keyboardState[(int)VirtualKey.Menu] = 0xff;
            }
            ToUnicode((uint)keys, 0, keyboardState, buf, 256, 0);
            return buf.ToString();
        }
        #endregion

        public static bool HitTestRect(Rect rect, Point point)
        {
            if (point.X < rect.X) { return false; }
            if (point.X >= rect.X + rect.Width) { return false; }
            if (point.Y < rect.Y) { return false; }
            if (point.Y >= rect.Y + rect.Height) { return false; }

            return true;
        }
    }
}