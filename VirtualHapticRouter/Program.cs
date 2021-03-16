using System;
using System.Collections.Generic;
using System.Linq;
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
        static void Main(string[] args)
        {
            ViGEmClient client = new ViGEmClient();
            IXbox360Controller controller = client.CreateXbox360Controller();
            controller.FeedbackReceived += FeedbackEventHandler;
            controller.Connect();
            Console.WriteLine("Virtual gamepad connected");
            Console.WriteLine(controller.AutoSubmitReport);
            Thread.Sleep(5000);
            
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
            Console.WriteLine("Rumble");
        }
    }
}
