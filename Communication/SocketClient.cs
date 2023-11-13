using System;
using System.Threading;
using System.Text;
using WatsonTcp;
using Newtonsoft.Json;
using System.IO;

namespace Communication
{
    public class SocketCient
    {
        /// <summary>
        /// Max message length in bytes
        /// </summary>
        public static int MaxMessageLength = 0;
        /// <summary>
        /// Iteration index to count the overall iterations
        /// </summary>
        public static int IterationIndex = 0;
        /// <summary>
        /// Iterates through image in directory and return next path each time
        /// </summary>
        /// <returns>Specific image path</returns>
        static string GetNextImagePath()
        {
            string[] filePaths = Directory.GetFiles("res/", "*.bmp", SearchOption.AllDirectories);
            if (filePaths.Length == 0)
            {
                return "";
            }
            int index = IterationIndex % filePaths.Length;
            return filePaths[index];
        }

        /// <summary>
        /// Example of request message formation
        /// </summary>
        static string FormMessage(string imagePath, string seatPart = "default")
        {
            //load image
            string imagebase64;
            int imageWidth, imageHeight, imageChannels;
            ImageManipulation.GetImageAsBase64(imagePath, out imagebase64, out imageWidth, out imageHeight, out imageChannels);
            //get image name from path
            string imageName = Path.GetFileName(imagePath);

            //form message
            RequestMessage message = new RequestMessage()
            {
                Timestamp = DateTime.Now.Ticks,
                SystemID = 0,
                CameraName = "Simulated Camera",
                Command = "SendImageData",
                Model = "default",
                Part = seatPart,
                ImageWidth = imageWidth,
                ImageHeight = imageHeight,
                ImageChannelsCount = imageChannels,
                ImageAsBase64 = imagebase64,
                ImageName = imageName
            };

            //make message JSON
            string messageJSON = JsonConvert.SerializeObject(message);
            return messageJSON;
        }

        static void Main(string[] args)
        {
           // infinitive loop
           for (;;)
            {
                try
                {
                    SocketClientSample();
                }
                catch(Exception ex) 
                {
                    Console.WriteLine("Sample will be restarted! " + ex.Message);
                    Thread.Sleep(1000);
                }
            }
        }

        static public void SocketClientSample()
        {
            // server address
            string serverIP = "127.0.0.1";
            int port = 5422;
            string serverAddress = serverIP + ":" + port.ToString();

            // create socket instance
            WatsonTcpClient client = new WatsonTcpClient(serverIP, port);

            // get max message length
            MaxMessageLength = client.Settings.StreamBufferSize;

            // set events
            client.Events.ServerConnected += OnConnect;
            client.Events.ServerDisconnected += OnDisconnect;
            client.Events.StreamReceived += OnDataStreamReceived;
            client.Events.ExceptionEncountered += OnException;

            // try to connect
            try
            {
                client.Connect();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Nothing more to do, closing application!");
                return;
                //Environment.Exit(1);
            }
            // make multiple request
            for (;;)
            {
                //Get next image path
                string imagePath = GetNextImagePath();
                if (imagePath == "")
                {
                    Console.WriteLine("No image found in directory 'res/' near executable, exiting!");
                    Environment.Exit(1);
                }

                //check if it is backrest or cushion
                string cameraName = imagePath.Contains("backrest") ? "backrest" : "cushion";

                //Form message once and sent it multiple times
                string messageJSON = FormMessage(imagePath, cameraName);

                Console.WriteLine("Sending Data");
                if (client.Connected)
                {
                    client.Send(messageJSON);
                    Console.WriteLine("Data Sent");
                }
                else
                {
                    Console.WriteLine("Client disconnected from server, nothing else to do, quiting...");
                    break;
                }
                Thread.Sleep(15000);
                //Increment iteration counter
                IterationIndex++;
                //Reset counter if it is too big
                if (IterationIndex == int.MaxValue)
                {
                    IterationIndex = 0;
                }
            }
            // disconnect
            if (client.Connected)
            {
                client.Disconnect();
            }
        }

        static void OnConnect(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Server " + args.IpPort + " connected");
        }

        static void OnException(object sender, ExceptionEventArgs args)
        {
            Console.WriteLine("Exception " + args.Exception);
        }

        static void OnDisconnect(object sender, DisconnectionEventArgs e)
        {
            string reason = "";
            switch (e.Reason)
            {
                case DisconnectReason.Removed:
                    reason = "kicked";
                    break;
                case DisconnectReason.Normal:
                    reason = "normal";
                    break;
                case DisconnectReason.Timeout:
                    reason = "timeout";
                    break;
                case DisconnectReason.Shutdown:
                    reason = "shutdown";
                    break;
            }
            Console.WriteLine("Disconnected from server " + e.IpPort + ", reason - " + reason);
        }

        static void OnDataStreamReceived(object sender, StreamReceivedEventArgs args)
        {
            Console.WriteLine("[" + args.IpPort + "]");
            long bytesRemaining = args.ContentLength;

            byte[] buffer = new byte[MaxMessageLength];
            string message = "";
            using (MemoryStream ms = new MemoryStream())
            {
                int bytesRead = 0;
                while (bytesRemaining > 0)
                {
                    bytesRead = args.DataStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        ms.Write(buffer, 0, bytesRead);
                        bytesRemaining -= bytesRead;
                    }
                }
                message = Encoding.UTF8.GetString(ms.ToArray());
            }
            //Testing purpose deserialization
            ResultMessage serverResponse = JsonConvert.DeserializeObject<ResultMessage>(message);
            //Write out if it is normal response or warning
            Console.Write("Received " + message.Length.ToString() + " bytes size message! Type/Command: " + serverResponse.Command + "\n");
        }
    }
}