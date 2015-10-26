using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.Text;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;
using Windows.UI.Input;

namespace win2d_text_game
{
    public sealed partial class MainPage : Page
    {
        win2d_Textbox textbox;
        win2d_Button button;
        win2d_Textblock textblock;
        win2d_ScrollBar scrollbar;

        List<win2d_Control> Controls = new List<win2d_Control>();
        win2d_Control ControlInFocus;

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        #region Keyboard Handling
        private void CoreWindow_KeyDown(CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (ControlInFocus != null) { ControlInFocus.KeyDown(args.VirtualKey); }
        }
        private void CoreWindow_KeyUp(CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (ControlInFocus != null) { ControlInFocus.KeyUp(args.VirtualKey); }
        }
        #endregion

        #region Mouse Handling
        private void canvasMain_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (ControlInFocus != null)
            {
                ControlInFocus.LoseFocus();
                ControlInFocus = null;
            }

            PointerPoint p = e.GetCurrentPoint(canvasMain);
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

        private void canvasMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(canvasMain);

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

        private void canvasMain_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(canvasMain);

            foreach (win2d_Control control in Controls)
            {
                if (control.HitTest(p.Position))
                {
                    control.MouseUp(p);
                }
            }
        }

        private void canvasMain_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(canvasMain);

            if(textblock.HitTest(p.Position))
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
        #endregion

        #region Draw/Update
        private void canvasMain_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            foreach (win2d_Control control in Controls)
            {
                control.Draw(args);
            }

            // START DEBUG
            DrawDebug(args);
            // END DEBUG
        }

        private void canvasMain_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
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
        #endregion

        #region Initialization
        private void canvasMain_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            int clientWidth = (int)sender.Size.Width;
            int clientHeight = (int)sender.Size.Height;

            Statics.UpArrow = new CanvasTextLayout(sender.Device, "\u2191", Statics.DefaultFont, 0, 0);
            Statics.DoubleUpArrow = new CanvasTextLayout(sender.Device, "\u219f", Statics.DefaultFont, 0, 0);
            Statics.DownArrow = new CanvasTextLayout(sender.Device, "\u2193", Statics.DefaultFont, 0, 0);
            Statics.DoubleDownArrow = new CanvasTextLayout(sender.Device, "\u21a1", Statics.DefaultFont, 0, 0);

            Vector2 textboxPosition = new Vector2(20, clientHeight - 50);
            textbox = new win2d_Textbox(sender.Device, textboxPosition, 600);

            button = new win2d_Button(sender.Device, new Vector2(textboxPosition.X + textbox.Width + 20, textboxPosition.Y), 100, textbox.Height, "Test button!");
            button.Click += Button_Click;

            textblock = new win2d_Textblock(new Vector2(20, 20), textbox.Width + 20 + button.Width, clientHeight - textbox.Height - 40, true);
            
            // START DEBUG
            for (int i = 0; i < 100000; i++)
            {
                textblock.Append(sender.Device, i.ToString());
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
        #endregion

        #region Control Event Handling
        private void Scrollbar_ScrollToBottom() { textblock.ScrollToBottom(); }
        private void Scrollbar_ScrollToTop() { textblock.ScrollToTop(); }
        private void Scrollbar_ScrollDown() { textblock.ScrollDown(); }
        private void Scrollbar_ScrollUp() { textblock.ScrollUp(); }

        private void Button_Click(PointerPoint point)
        {
            // START DEBUG
            Statics.DebugButtonClickCount++;
            // END DEBUG

            if (textbox.Text != null)
            {
                string text = textbox.Text.Trim();

                if (text != string.Empty)
                {
                    textblock.Append(canvasMain.Device, text);
                }

                textbox.Text = string.Empty;
                ControlInFocus = textbox;
                textbox.GiveFocus();
            }
        }
        #endregion

        #region Debug
        private void DrawDebug(CanvasAnimatedDrawEventArgs args)
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
