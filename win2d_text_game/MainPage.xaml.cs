using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.System;
using Windows.UI.Input;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace win2d_text_game
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        win2d_Textbox textbox;
        win2d_Button button;
        win2d_Textblock textblock;

        List<win2d_Control> Controls = new List<win2d_Control>();
        win2d_Control ControlInFocus;

        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (ControlInFocus != null)
            {
                ControlInFocus.KeyDown(args.VirtualKey);
            }
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (ControlInFocus != null)
            {
                ControlInFocus.KeyUp(args.VirtualKey);
            }
        }

        private void gridMain_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ControlInFocus = null;
            PointerPoint p = e.GetCurrentPoint(this);

            foreach (win2d_Control control in Controls)
            {
                if (control.HitTest(p.Position))
                {
                    Statics.ControlInFocusString = "Control in focus: " + control.ToString();

                    control.MouseDown(p);
                    control.GiveFocus();
                    ControlInFocus = control;

                    return;
                }
                else
                {
                    control.LoseFocus();
                }
            }

            Statics.ControlInFocusString = "Control in focus: N/A";
        }

        private void gridMain_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(this);

            foreach (win2d_Control control in Controls)
            {
                if(control.HitTest(p.Position))
                {
                    control.MouseUp(p);
                }
            }
        }

        private void gridMain_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint p = e.GetCurrentPoint(this);

            foreach (win2d_Control control in Controls)
            {
                if(control.HitTest(p.Position))
                {
                    control.MouseEnter(p);
                }
                else
                {
                    control.MouseLeave();
                }
            }
        }

        private void canvasMain_Draw(ICanvasAnimatedControl sender, CanvasAnimatedDrawEventArgs args)
        {
            foreach (win2d_Control control in Controls)
            {
                control.Draw(args);
            }

            DrawDebug(args);
        }

        private void canvasMain_Update(ICanvasAnimatedControl sender, CanvasAnimatedUpdateEventArgs args)
        {
            foreach (win2d_Control control in Controls)
            {
                control.Update(args);
            }

            Statics.UpdateString = "Update time: " + args.Timing.ElapsedTime.TotalMilliseconds.ToString() + "ms";
        }

        private void canvasMain_CreateResources(CanvasAnimatedControl sender, CanvasCreateResourcesEventArgs args)
        {
            int clientWidth = (int)sender.Size.Width;
            int clientHeight = (int)sender.Size.Height;

            Vector2 textboxPosition = new Vector2(200, clientHeight - 50);
            textbox = new win2d_Textbox(sender.Device, textboxPosition, 600);

            button = new win2d_Button(sender.Device, new Vector2(textboxPosition.X + textbox.Width + 20, textboxPosition.Y), 100, textbox.Height, "Test button!");
            button.Click += Button_Click;

            textblock = new win2d_Textblock(new Vector2(200, 20), textbox.Width + 20 + button.Width, clientHeight - textbox.Height - 40);

            Controls.Add(textbox);
            Controls.Add(button);
            Controls.Add(textblock);
        }

        private void Button_Click(PointerPoint point)
        {
            Statics.ButtonClickCount++;

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

        private void DrawDebug(CanvasAnimatedDrawEventArgs args)
        {
            args.DrawingSession.DrawText("Calculate count: " + Statics.CalculateCount.ToString(), new Vector2(10, 600), Colors.White);
            args.DrawingSession.DrawText("Button click count: " + Statics.ButtonClickCount.ToString(), new Vector2(10, 620), Colors.White);
            args.DrawingSession.DrawText(Statics.ControlInFocusString, new Vector2(10, 640), Colors.White);
            args.DrawingSession.DrawText(Statics.TextString, new Vector2(10, 660), Colors.White);
            args.DrawingSession.DrawText(Statics.UpdateString, new Vector2(10, 680), Colors.White);
        }
    }
}
