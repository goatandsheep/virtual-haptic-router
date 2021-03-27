using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
// using Nefarius.ViGEm.Client.Targets.DualShock4;
using Nefarius.ViGEm.Client.Exceptions;
using OBSWebsocketDotNet;
using OBSWebsocketDotNet.Types;
using InputInterceptorNS;

namespace VirtualHapticRouter
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static CancellationTokenSource timeout;
        private static FormUrlEncodedContent onContent;
        private static FormUrlEncodedContent offContent;
        protected static OBSWebsocket _obs;
        static void Main(string[] args)
        {
            ViGEmClient client = new ViGEmClient();
            IXbox360Controller controller = client.CreateXbox360Controller();
            // Int32 INTENSITY = 32767; // TODO: experiment with various intensities
            controller.FeedbackReceived += new Xbox360FeedbackReceivedEventHandler(FeedbackEventHandler);
            controller.Connect();

            _obs = new OBSWebsocket();

            _obs.Connect("ws://localhost:4444", "");

            _obs.Connected += onConnect;
            _obs.Disconnected += onDisconnect;

            // _obs.StreamStatus += onStreamData;
            try
            {
                _obs.SendCaptions("test");
            } catch (Exception err)
            {
                Console.WriteLine(err);
            }
            /*
            var onValues = new Dictionary<string, string>
                    {
                        { "text", "███ ███ ███" }
                    };
            var offValues = new Dictionary<string, string>
                    {
                        { "text", "" }
                    };
            onContent = new FormUrlEncodedContent(onValues);
            offContent = new FormUrlEncodedContent(offValues);
            
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
            });
            */

            Console.WriteLine("Virtual gamepad connected");

            Console.WriteLine("Hooks enabled. Press any key to release.");
            Console.ReadKey();
            controller.Disconnect();


            // keyboardHook.Dispose();
            // mouseHook.Dispose();
            /*
            int counter = 0;
            while (true)
            {
                try {
                    // controller.SubmitReport();
                    // Xbox360Report controller1Report = new Xbox360Report();

                    // controller.SubmitReport(controller1Report);
                    Thread.Sleep(5000);
            
                    _obs.SendCaptions("test" + counter);
                    Thread.Sleep(1000);
                    _obs.SendCaptions("");
                    counter++;
                } catch (Exception err)
                {
                    // keyboardHook.Dispose();
                    // mouseHook.Dispose();
                    controller.Disconnect();
                    throw err;
                }
            }
            */
            
            

        }

        private static void onConnect(object sender, EventArgs e)
        {

            var streamStatus = _obs.GetStreamingStatus();
            Console.WriteLine("Connected to OBS");
            /* 
            if (streamStatus.IsStreaming)
                onStreamingStateChange(_obs, OutputState.Started);
            else
                onStreamingStateChange(_obs, OutputState.Stopped);

            if (streamStatus.IsRecording)
                onRecordingStateChange(_obs, OutputState.Started);
            else
                onRecordingStateChange(_obs, OutputState.Stopped);
            */
        }

        protected static void onStreamData(OBSWebsocket sender, StreamStatus data)
        {
            Console.WriteLine(data);
        }

        protected static void onDisconnect(object sender, EventArgs e)
        {
            throw new Exception("disconnected");
        }
        /*
        private static void onStreamingStateChange(OBSWebsocket sender, OutputState newState)
        {
            string state = "";
            switch (newState)
            {
                case OutputState.Starting:
                    state = "Stream starting...";
                    break;

                case OutputState.Started:
                    state = "Stop streaming";
                    break;

                case OutputState.Stopping:
                    state = "Stream stopping...";
                    break;

                case OutputState.Stopped:
                    state = "Start streaming";
                    break;

                default:
                    state = "State unknown";
                    break;
            }
        }
        */

        /*
        private static void onRecordingStateChange(OBSWebsocket sender, OutputState newState)
        {
            string state = "";
            switch (newState)
            {
                case OutputState.Starting:
                    state = "Recording starting...";
                    break;

                case OutputState.Started:
                    state = "Stop recording";
                    break;

                case OutputState.Stopping:
                    state = "Recording stopping...";
                    break;

                case OutputState.Stopped:
                    state = "Start recording";
                    break;

                default:
                    state = "State unknown";
                    break;
            }
        }
        */

        static async Task closeCaption(CancellationToken token)
        {
            await Task.Delay(100, timeout.Token);
            try
            {
                _obs.SendCaptions("");
            } catch(Exception err)
            {
                Console.WriteLine(err);
            }
            timeout = null;
        }

        static void FeedbackEventHandler(object sender, Xbox360FeedbackReceivedEventArgs e)
        {
            try
            {
                if (e.SmallMotor > 0)
                {
                    Console.WriteLine(e.SmallMotor);
                    if (timeout == null)
                    {
                        _obs.SendCaptions("███ ███ ███");
                    }
                    else
                    {
                        timeout.Cancel();
                    }
                    timeout = new CancellationTokenSource();
                    closeCaption(timeout.Token);

                    /*
                    var values = new Dictionary<string, string>
                    {
                        { "Value1", e.SmallMotor.ToString() },
                        // { "Value2", e.LargeMotor.ToString() }
                    };

                    try
                    {
                        var response = client.PostAsync("https://maker.ifttt.com/trigger/rumble/with/key/dDYeWn9kg-rcdgvxBG4fnu", content);
                    }
                    catch (Exception netErr)
                    {
                        Console.WriteLine(netErr);
                        throw netErr;
                    }
                    */


                }
            }
            catch (Exception netErr)
            {
                Console.WriteLine(netErr);
                throw netErr;
            }
        }
    }
}
