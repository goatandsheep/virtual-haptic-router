using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
using Nefarius.ViGEm.Client.Exceptions;
using InputInterceptorNS;

namespace VirtualHapticRouter
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            ViGEmClient client = new ViGEmClient();
            IXbox360Controller controller = client.CreateXbox360Controller();
            // Int32 INTENSITY = 32767; // TODO: experiment with various intensities
            controller.FeedbackReceived += new Xbox360FeedbackReceivedEventHandler(FeedbackEventHandler);
            controller.Connect();
            if (InputInterceptor.CheckDriverInstalled())
            {
                Console.WriteLine("Input interceptor seems to be installed.");
                if (InputInterceptor.Initialize())
                {
                    Console.WriteLine("Input interceptor successfully initialized.");
                }
                else
                {
                    Console.WriteLine("Input interceptor initialization failed.");
                    throw new Exception("Input interceptor initialization failed.");
                }
            }
            else
            {
                Console.WriteLine("Input interceptor not installed.");
                if (InputInterceptor.CheckAdministratorRights())
                {
                    Console.WriteLine("Installing...");
                    if (InputInterceptor.InstallDriver())
                    {
                        Console.WriteLine("Done! Restart your computer.");
                    }
                    else
                    {
                        Console.WriteLine("Something... gone... wrong... :(");
                        throw new Exception("Something... gone... wrong... :(");
                    }
                }
                else
                {
                    Console.WriteLine("Restart program with administrator rights so it will be installed.");
                    throw new Exception("Restart program with administrator rights so it will be installed.");
                }
            }
            MouseHook mouseHook = new MouseHook((ref MouseStroke mouseStroke) => {
                // Console.WriteLine($"{mouseStroke.X} {mouseStroke.Y} {mouseStroke.Flags} {mouseStroke.State} {mouseStroke.Information}"); // Mouse XY coordinates are raw
                if (mouseStroke.State == MouseState.LeftButtonDown)
                {
                    Console.WriteLine($"{mouseStroke.X} {mouseStroke.Y} {mouseStroke.Flags} {mouseStroke.State} {mouseStroke.Information}");
                    controller.SetButtonState(Xbox360Button.B, true);
                }
                else if (mouseStroke.State == MouseState.LeftButtonUp)
                {
                    Console.WriteLine($"{mouseStroke.X} {mouseStroke.Y} {mouseStroke.Flags} {mouseStroke.State} {mouseStroke.Information}");
                    controller.SetButtonState(Xbox360Button.B, false);
                }
                else if (mouseStroke.State == MouseState.RightButtonDown)
                {
                    Console.WriteLine($"{mouseStroke.X} {mouseStroke.Y} {mouseStroke.Flags} {mouseStroke.State} {mouseStroke.Information}");
                    controller.SetButtonState(Xbox360Button.X, true);
                }
                else if (mouseStroke.State == MouseState.RightButtonUp)
                {
                    Console.WriteLine($"{mouseStroke.X} {mouseStroke.Y} {mouseStroke.Flags} {mouseStroke.State} {mouseStroke.Information}");
                    controller.SetButtonState(Xbox360Button.X, false);
                }
            });
            KeyboardHook keyboardHook = new KeyboardHook((ref KeyStroke keyStroke) => {
                Console.WriteLine($"{keyStroke.Code} {keyStroke.State} {keyStroke.Information}");
                if (keyStroke.Code == KeyCode.A)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbX, -32767);
                    }
                    else
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    }
                }
                else if (keyStroke.Code == KeyCode.D)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbX, 32767);
                    }
                    else
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbX, 0);
                    }
                }
                else if (keyStroke.Code == KeyCode.W)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbY, 32767);
                    }
                    else
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    }
                }
                else if (keyStroke.Code == KeyCode.S)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbY, -32767);
                    }
                    else
                    {
                        controller.SetAxisValue(Xbox360Axis.LeftThumbY, 0);
                    }
                }
                else if (keyStroke.Code == KeyCode.Space)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.A, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.A, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.Q)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetSliderValue(Xbox360Slider.RightTrigger, 255);
                    }
                    else
                    {
                        controller.SetSliderValue(Xbox360Slider.RightTrigger, 0);
                    }
                }
                else if (keyStroke.Code == KeyCode.LeftShift || keyStroke.Code == KeyCode.RightShift)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetSliderValue(Xbox360Slider.LeftTrigger, 255);
                    }
                    else
                    {
                        controller.SetSliderValue(Xbox360Slider.LeftTrigger, 0);
                    }
                }
                else if (keyStroke.Code == KeyCode.F)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.Y, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.Y, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.E)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.X, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.X, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.R)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.RightThumb, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.RightThumb, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.Tab)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.Back, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.Back, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.PageUp)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.LeftShoulder, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.LeftShoulder, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.PageDown)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.RightShoulder, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.RightShoulder, false);
                    }
                }
                else if (keyStroke.Code == KeyCode.Escape)
                {
                    if (keyStroke.State == KeyState.Down)
                    {
                        controller.SetButtonState(Xbox360Button.Start, true);
                    }
                    else
                    {
                        controller.SetButtonState(Xbox360Button.Start, false);
                    }
                }
                /*controller.SetButtonState(Xbox360Button.A, true);
                controller.SetButtonState(Xbox360Button.B, true);
                controller.SetButtonState(Xbox360Button.X, true);*/
            });

            Console.WriteLine("Virtual gamepad connected");

            Console.WriteLine("Hooks enabled. Press any key to release.");
            Console.ReadKey();
            keyboardHook.Dispose();
            mouseHook.Dispose();
            controller.Disconnect();
            /*
            while (true)
            {
                try {
                    // controller.SubmitReport();
                    // Xbox360Report controller1Report = new Xbox360Report();

                    // controller.SubmitReport(controller1Report);
                    Thread.Sleep(20);
                } catch (Exception err)
                {
                    controller.Disconnect();
                    throw err;
                }
            }
            */
        }
        static void FeedbackEventHandler(object sender, Xbox360FeedbackReceivedEventArgs e)
        {
            if (e.SmallMotor > 0)
            {
                Console.WriteLine(e.SmallMotor);
                var values = new Dictionary<string, string>
                {
                    { "Value1", e.SmallMotor.ToString() },
                    // { "Value2", e.LargeMotor.ToString() }
                };
                var content = new FormUrlEncodedContent(values);

                try
                {
                    var response = client.PostAsync("https://maker.ifttt.com/trigger/rumble/with/key/dDYeWn9kg-rcdgvxBG4fnu", content);
                } catch (Exception netErr)
                {
                    Console.WriteLine(netErr);
                    throw netErr;
                }
            }
        }
    }
}
