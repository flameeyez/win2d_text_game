using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;

namespace win2d_text_game
{
    class TextboxCursor
    {
        public enum CURSOR_STATE
        {
            ON,
            OFF
        }

        private CURSOR_STATE State { get; set; }
        public Vector2 Position { get; set; }
        private TimeSpan LastUpdate { get; set; }

        private static string CursorCharacter = "|";

        private Color Color { get; set; }

        private static int UpdateThreshold = 300;

        public TextboxCursor(Vector2 position, Color color)
        {
            Position = position;
            Color = color;
            LastUpdate = TimeSpan.Zero;
        }

        public void Draw(CanvasAnimatedDrawEventArgs args)
        {
            if (State == CURSOR_STATE.ON)
            {
                args.DrawingSession.DrawText(CursorCharacter, Position, Color);
            }
        }

        public void Update(CanvasAnimatedUpdateEventArgs args)
        {
            LastUpdate += args.Timing.ElapsedTime;

            if (LastUpdate.TotalMilliseconds > UpdateThreshold)
            {
                LastUpdate = TimeSpan.Zero;

                switch (State)
                {
                    case CURSOR_STATE.ON: State = CURSOR_STATE.OFF; break;
                    case CURSOR_STATE.OFF: State = CURSOR_STATE.ON; break;
                }
            }
        }
    }
}
