using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

namespace win2d_text_game
{
    public static class UI
    {
        static win2d_Textbox textbox;
        static win2d_Button button;
        static win2d_Textblock textblock;
        static win2d_ScrollBar scrollbar;

        static List<win2d_Control> Controls = new List<win2d_Control>();
        static win2d_Control ControlInFocus;

        static CanvasAnimatedControl Canvas;

        public static void Initialize(CanvasAnimatedControl sender)
        {
            Canvas = sender;

            Vector2 textboxPosition = new Vector2(20, (float)sender.Size.Height - 50);
            textbox = new win2d_Textbox(sender.Device, textboxPosition, 600);

            button = new win2d_Button(sender.Device, new Vector2(textboxPosition.X + textbox.Width + 20, textboxPosition.Y), 100, textbox.Height, "Test button!");
            button.Click += Button_Click;

            textblock = new win2d_Textblock(new Vector2(20, 20), textbox.Width + 20 + button.Width, (int)sender.Size.Height - textbox.Height - 40, true);

            // START DEBUG
            for (int i = 100; i < 200; i++)
            {
                StringBuilder s = new StringBuilder();
                s.Append(i.ToString());
                for (int j = 0; j < 29; j++)
                {
                    s.Append(" ");
                    s.Append(i.ToString());
                }

                textblock.Append(sender.Device, s.ToString());
            }
            // END DEBUG

            scrollbar = new win2d_ScrollBar(new Vector2(textblock.Position.X + textblock.Width, textblock.Position.Y), 20, textblock.Height);
            scrollbar.ScrollUp += Scrollbar_ScrollUp;
            scrollbar.ScrollDown += Scrollbar_ScrollDown;
            scrollbar.ScrollToTop += Scrollbar_ScrollToTop;
            scrollbar.ScrollToBottom += Scrollbar_ScrollToBottom;

            Controls.Add(textbox);
            Controls.Add(button);
            Controls.Add(textblock);
            Controls.Add(scrollbar);
        }

        #region Control Event Handling
        private static void Scrollbar_ScrollToBottom() { textblock.ScrollToBottom(); }
        private static void Scrollbar_ScrollToTop() { textblock.ScrollToTop(); }
        private static void Scrollbar_ScrollDown() { textblock.ScrollDown(); }
        private static void Scrollbar_ScrollUp() { textblock.ScrollUp(); }

        private static void Button_Click(PointerPoint point)
        {
            // START DEBUG
            Statics.DebugButtonClickCount++;
            // END DEBUG

            if (textbox.Text != null)
            {
                string text = textbox.Text.Trim();

                if (text != string.Empty)
                {
                    textblock.Append(Canvas.Device, text);
                }

                textbox.Text = string.Empty;
                ControlInFocus = textbox;
                textbox.GiveFocus();
            }
        }
        #endregion

        public static void Draw(CanvasAnimatedDrawEventArgs args)
        {
            foreach (win2d_Control control in Controls)
            {
                control.Draw(args);
            }

            // START DEBUG
            DrawDebug(args);
            // END DEBUG
        }

        public static void Update(CanvasAnimatedUpdateEventArgs args)
        {
            foreach (win2d_Control control in Controls)
            {
                control.Update(args);
            }

            // START DEBUG
            Statics.DebugUpdateTimeString = "Update time: " + args.Timing.ElapsedTime.TotalMilliseconds.ToString() + "ms";
            Statics.DebugStringsCount = textblock.DebugStringsCount;
            // END DEBUG
        }

        public static void KeyDown(VirtualKey vk)
        {
            if (ControlInFocus != null) { ControlInFocus.KeyDown(vk); }
        }

        public static void KeyUp(VirtualKey vk)
        {
            if (ControlInFocus != null) { ControlInFocus.KeyUp(vk); }
        }

        public static void PointerPressed(PointerRoutedEventArgs e)
        {
            if (ControlInFocus != null)
            {
                ControlInFocus.LoseFocus();
                ControlInFocus = null;
            }

            PointerPoint p = e.GetCurrentPoint(Canvas);
            foreach (win2d_Control control in Controls)
            {
                if (control.HitTest(p.Position))
                {
                    // START DEBUG
                    Statics.DebugControlInFocusString = "Focus: " + control.ToString();
                    // END DEBUG

                    control.MouseDown(p);
                    control.GiveFocus();
                    ControlInFocus = control;

                    return;
                }
            }

            // no hits
            // START DEBUG
            Statics.DebugControlInFocusString = "Focus: N/A";
            // END DEBUG
        }

        public static void PointerMoved(PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(Canvas);

            foreach (win2d_Control control in Controls)
            {
                if (control.HitTest(p.Position))
                {
                    control.MouseEnter(p);
                }
                else
                {
                    control.MouseLeave();
                }
            }
        }

        public static void PointerReleased(PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(Canvas);

            foreach (win2d_Control control in Controls)
            {
                if (control.HitTest(p.Position))
                {
                    control.MouseUp(p);
                }
            }
        }

        public static void PointerWheelChanged(PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(Canvas);

            if (textblock.HitTest(p.Position))
            {
                if (p.Properties.MouseWheelDelta < 0)
                {
                    textblock.ScrollDown();
                }
                else if (p.Properties.MouseWheelDelta > 0)
                {
                    textblock.ScrollUp();
                }
            }
        }

        #region Debug
        private static void DrawDebug(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawText("Calculate count: " + Statics.DebugCalculateCount.ToString(), new Vector2(800, 20), Colors.White);
            args.DrawingSession.DrawText("Button click count: " + Statics.DebugButtonClickCount.ToString(), new Vector2(800, 40), Colors.White);
            args.DrawingSession.DrawText(Statics.DebugControlInFocusString, new Vector2(800, 60), Colors.White);
            args.DrawingSession.DrawText(Statics.DebugUpdateTimeString, new Vector2(800, 80), Colors.White);
            args.DrawingSession.DrawText("Strings count: " + Statics.DebugStringsCount.ToString(), new Vector2(800, 100), Colors.White);
        }
        #endregion
    }
}
