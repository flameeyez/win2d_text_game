using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace win2d_text_game
{
    class win2d_TextblockStringCollection
    {
        // keep track of total height
        private List<win2d_TextblockString> Strings = new List<win2d_TextblockString>();
        public int Count { get { return Strings.Count; } }

        private Vector2 StringsPosition { get; set; }
        private static int StringPaddingY = 5;
        private int nFirstStringToDraw = 0;

        // private int DrawingWidth { get; set; }
        private int DrawingHeight { get; set; }

        private int _totalstringsheight = 0;
        private int TotalStringsHeight { get { return _totalstringsheight; } }

        public win2d_TextblockStringCollection(Vector2 position, int drawingheight)
        {
            StringsPosition = position;
            DrawingHeight = drawingheight;
        }

        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            if(Strings.Count == 0) { return; }
            if(CanDrawAllStrings())
            {
                DrawAllStrings(args);
            }
            else
            {
                int i = nFirstStringToDraw;
                float fCurrentY = StringsPosition.Y;
                while(i < Strings.Count && fCurrentY + Strings[i].Height < StringsPosition.Y + DrawingHeight)
                {
                    args.DrawingSession.DrawTextLayout(Strings[i].Text, new Vector2(StringsPosition.X, fCurrentY), Colors.White);
                    fCurrentY += Strings[i].Height + StringPaddingY;
                    i++;
                }
            }
        }

        private void DrawAllStrings(CanvasAnimatedDrawEventArgs args)
        {
            float fCurrentY = StringsPosition.Y;

            foreach (win2d_TextblockString str in Strings)
            {
                args.DrawingSession.DrawTextLayout(str.Text, new Vector2(StringsPosition.X, fCurrentY), Colors.White);
                fCurrentY += str.Height + StringPaddingY;
            }
        }

        public void Add(CanvasDevice device, string str)
        {
            win2d_TextblockString s = new win2d_TextblockString(device, str);
            Strings.Add(s);
            _totalstringsheight += s.Height + StringPaddingY;

            // if(ScrollToBottom)
        }

        private bool CanDrawAllStrings()
        {
            return TotalStringsHeight <= DrawingHeight;
        }

        public void ScrollUp()
        {
            if (nFirstStringToDraw > 0) { nFirstStringToDraw--; }
        }

        public void ScrollDown()
        {
            // need to figure out how many end strings can be drawn

            if(nFirstStringToDraw < Strings.Count - 1)
            {
                nFirstStringToDraw++;
            }
        }

        //private int FirstStringInView(int nMaxHeight)
        //{
        //    if (Strings.Count == 0) { return -1; }

        //    int nCurrentHeight = 0;
        //    int i = Strings.Count - 1;

        //    while(i >= 0 && nCurrentHeight <= nMaxHeight)
        //    {
        //        nCurrentHeight += Strings[i].Height;
        //        i--;
        //    }

        //    return i;
        //}
    }
}
