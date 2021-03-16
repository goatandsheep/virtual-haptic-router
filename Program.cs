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

namespace VirtualHapticRouter
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            ViGEmClient client = new ViGEmClient();
            IXbox360Controller controller = client.CreateXbox360Controller();
            controller.FeedbackReceived += new Xbox360FeedbackReceivedEventHandler(FeedbackEventHandler);
            controller.Connect();
            Console.WriteLine("Virtual gamepad connected");
            
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
        }
        static void FeedbackEventHandler(object sender, Xbox360FeedbackReceivedEventArgs e)
        {
            if (e.SmallMotor > 0)
            {
                Console.WriteLine(e.SmallMotor);
                var values = new Dictionary<string, string>
                {
                    { "SmallMotor", e.SmallMotor.ToString() },
                    { "LargeMotor", e.LargeMotor.ToString() }
                };
                var content = new FormUrlEncodedContent(values);

                try
                {
                    var response = client.PostAsync("http://localhost:5000/", content);
                } catch (Exception netErr)
                {
                    Console.WriteLine(netErr);
                    throw netErr;
                }
            }
        }
    }
}
