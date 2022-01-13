using Newtonsoft.Json.Bson;
using Serilog;
using Serilog.Debugging;
using SockNet.ClientSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using WatsonTcp;

namespace ConsoleApp1
{
    class Program
    {
        private static SemaphoreSlim semaphore;
        private static int padding;
        static HttpClient httpClient;
        static Random rnd = new Random();
        private static System.Timers.Timer aTimer;
        static int startMin;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World! - Press any key and enter to start.");
            string input = Console.ReadLine();
           // startMin = DateTime.Now.Minute;

             PostAPI();
           //  PostAPINextMinute();

            // RunSemaPhore(); 
            // PostByteArray();
            // GetAPI();


            // PostToServer();
            // PostWatsonTcp();
            // PostHttpPort();

            // TestLogger();

            // PostMultipleAPIsecondsWait(); 
            // PostMultipleNextMinute();

            Console.ReadLine();
        }

        private static void TestLogger()
        {
            SelfLog.Enable(Console.Out);

            var sw = System.Diagnostics.Stopwatch.StartNew();

            Log.Logger = new LoggerConfiguration().WriteTo.File("log.txt").CreateLogger(); 

            for (var i = 0; i < 51; ++i)
            {
                Log.Information($"Hello, file logger! No {i}. \n"); 
            }

            Log.CloseAndFlush();

            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Size: {new FileInfo("log.txt").Length}");

            Console.WriteLine("Press any key to delete the temporary log file..."); 
            Console.ReadKey(true);

            File.Delete("log.txt");

            Console.WriteLine("Press any key to continue:");
            string input = Console.ReadLine();
            TestLogger();
        }

        public async static void PostHttpPort()
        {
            try
            {
                /*NetworkStream serverStream = clientSocket.GetStream();
                serverStream.Flush();
                for (int i=1; i<6; i++)
                {
                    /* byte[] outStream = System.Text.Encoding.ASCII.GetBytes("Message " + i.ToString());
                     Console.WriteLine("Data sending to Server : " + System.Text.Encoding.ASCII.GetString(outStream));
                     await serverStream.WriteAsync(outStream, 0, outStream.Length);
                     serverStream.Flush();

                     byte[] inStream = new byte[10025];
                     // serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                     await serverStream.ReadAsync(inStream, 0, inStream.Length);
                     string returndata = System.Text.Encoding.ASCII.GetString(inStream); //inStream.Skip(16).ToArray();
                     Console.WriteLine("Data gotten from Server : " + returndata.Trim('\0')); 

                }*/

                for (int i = 1; i < 6; i++)
                {
                    using (TcpClient tcpclnt = new TcpClient())
                    {
                        Thread.Sleep(1000);
                        tcpclnt.Connect("192.168.10.28", 1377);

                        using (NetworkStream stream = tcpclnt.GetStream())
                        {
                            string data = "Message " + i.ToString();
                            byte[] msg = Encoding.UTF8.GetBytes(data);
                            await stream.WriteAsync(msg, 0, msg.Length);
                            byte[] rspData = new byte[1024];
                            var mm = await stream.ReadAsync(rspData, 0, rspData.Length);
                            string resp = Encoding.UTF8.GetString(rspData);
                            Console.WriteLine(resp); //rspData.Skip(2).ToArray());
                        }

                       // tcpclnt.Client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("\n Press any keys to continue:");
            string res = Console.ReadLine();
            //if (res == "Y") 
                PostHttpPort();
        }

        private static void PostWatsonTcp()
        {
            try
            {
            WatsonTcpClient client = new WatsonTcpClient("192.168.10.194", 1371);
            client.Events.ServerConnected += ServerConnected;
            client.Events.ServerDisconnected += ServerDisconnected;
            client.Events.MessageReceived += MessageReceived;
            client.Callbacks.SyncRequestReceived = SyncRequestReceived;
            client.Connect();

            
            // check connectivity
            Console.WriteLine("Am I connected?  " + client.Connected);

            /*
            // send a message
            client.Send("Hello!");

            // send a message with metadata
            Dictionary<object, object> md = new Dictionary<object, object>(); 
            md.Add("foo", "bar");
            client.Send("Hello, client!  Here's some metadata!", md);

            // send async!
            await client.SendAsync("Hello, client!  I'm async!"); 
            */

                for (int i = 1; i < 51; i++)
                {
                    string data = "OMOGIDIB" + i.ToString();
                    // send and wait for a response
                    // SyncResponse resp = client.SendAndWait(63000, data);
                    // Console.WriteLine("My friend says: " + Encoding.UTF8.GetString(resp.Data));

                    //send only
                    // var r = client.Send(data);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Too slow... : " + ex.Message);
            }
        }

        static void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message from " + args.IpPort + ": " + Encoding.UTF8.GetString(args.Data));
        }

        static void ServerConnected(object sender, ConnectionEventArgs args)
        {
            Console.WriteLine("Server " + args.IpPort + " connected");
        }

        static void ServerDisconnected(object sender, DisconnectionEventArgs args)
        {
            Console.WriteLine("Server " + args.IpPort + " disconnected");
        }

        private static SyncResponse SyncRequestReceived(SyncRequest arg)
        {
            return new SyncResponse(arg, "Hello back at you!");
        }

        private async static void PostToServer()
        {

            byte[] recData = null;
            SocketClient client = new SocketClient("192.168.10.194", 1371);
            try
            {
                if (await client.Connect())
                {
                    for (int i = 1; i < 11; i++) 
                    {
                        string data = "SUKURATB" + i.ToString();
                        await client.Send(Encoding.UTF8.GetBytes(data), 1000); 
                        recData = await client.ReceiveBytes();
                        Console.WriteLine("Received data: " + Encoding.UTF8.GetString(recData)); 
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception raised: " + e);
            }
            client.Disconnect();
        }

        private static void PostMultipleNextMinute()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEventMultiple;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEventMultiple(Object source, ElapsedEventArgs e)
        {
            int currentMin = e.SignalTime.Minute;

            if (startMin == currentMin) Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);

            if (startMin != currentMin) { Console.Clear(); PostMultipleAPIsecondsWait(); aTimer.Stop(); aTimer.Dispose(); } 
        }

        static string TimeSent;
        private async static void PostMultipleAPIsecondsWait()
        {
            try
            {
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://89e864d66dbe.ngrok.io/Api/");
                httpClient.Timeout = TimeSpan.FromMinutes(9000);

                int i = 1;
                while (i < 2)
                {
                    string mrawData = i.ToString();

                    TimeSent = DateTime.Now.ToString("hh:mm:ss fff");
                    var response = await httpClient.PostAsync("MultiplePost", new StringContent(mrawData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //var byteResult = await response.Content.ReadAsByteArrayAsync();
                        //string result = Convert.ToBase64String(byteResult);

                        string result = await response.Content.ReadAsStringAsync();

                        Console.WriteLine($" Time Sent : {TimeSent} Time Received : {DateTime.Now.ToString("hh:mm:ss fff")} \nResponse String {result} \n");

                    }
                    else
                    {
                        Console.WriteLine("No response : " + response.StatusCode.ToString());
                    }

                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
        }

        private async static void GetAPI()
        {
            try
            {
                httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("https://986fa34d001a.ngrok.io/Api/");
                var response = await httpClient.GetAsync("Test/2").ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //var byteResult = await response.Content.ReadAsByteArrayAsync();
                    //string result = Convert.ToBase64String(byteResult);

                    string result = await response.Content.ReadAsStringAsync();

                    Console.WriteLine("Response String " + result);

                }
                else
                {
                    Console.WriteLine("No response : " + response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
            }
        }

        private static void PostAPINextMinute()
        {
            // Create a timer with a two second interval.
            aTimer = new System.Timers.Timer(1);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEventAPI;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEventAPI(Object source, ElapsedEventArgs e)
        {
            int currentMin = e.SignalTime.Minute;

            if (startMin == currentMin) Console.WriteLine("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);

            if (startMin != currentMin) { Console.Clear(); PostAPI(); aTimer.Stop(); aTimer.Dispose(); }
        }

        private async static void PostAPI()
        {
            try
            {
                httpClient = new HttpClient();
                //httpClient.BaseAddress = new Uri("http://192.168.10.194/CDLISWPostBridge/ISWProcess/"); // PWKOnKeyExchange/ 
                //string mrawData = "";
                //httpClient.BaseAddress = new Uri("https://capriconisoapiv3.cdlservices.com.ng/MPOSAPI/"); // TransactionRequestOnWeb 

                httpClient.BaseAddress = new Uri("http://192.168.10.194/ETLMPOSAPI/MPOSAPI/"); // TransactionRequestOnWeb 

                //httpClient.BaseAddress = new Uri("https://61a20c25393a.ngrok.io/api/"); // PostQueue 


                int emptyRespnse = 0; 
                int getResponse = 0;
                int i = 1;
                while (i < 2) { 
                    int num = rnd.Next(1000000,9000000); 
                    string randomAlph = RandomString(3) + num.ToString(); 

                    string mrawData = $"{{\"track1\":\"5f200f4f594152494e44452f4553544845524f07a00000000410105f24032010319f160f4243544553543132333435363738009f21031855379a032105239f02060000000001009f03060000000000009f34034203009f12104465626974204d6173746572436172649f0607a00000000410109f4e0f616263640000000000000000000000c408539983ffffff0416c10a09120200630001e0051fc70803c9a94d83c80694c00a09120200630001e00604c28201a08d3670a39ceb0c98de92f21297ab7f0136238568516d8e76bd7407d2d112eb8af4d1ffb61170b0de1e72615061530155fad80e37a984484a11a4a2d5ecc407c345e319dcd8e58ad5797f9e43fca33508ffcd4a517b73fd6b0ef71e4f77a5db5bc1c49a4b7613245f5a21bad97f3d169974c750ddb7d0e5c2bb4d1a1799a0a3e4746366858f05da55f6ff34984968d5ccbeaeb6f42226346f87fedf50c3a549bf7868a73c2eba51380eb9be0605314ddcc077f8895bb1d37f4d6b5c6b96e6e37864c1942b28f8519e3967bb40f556f77dbd70d0e4f64c67498219ef5076eca5c34f673c5e33366694513b400a9169eea2df95bbd3f44a80b60b62929b2b2ef08dc20bef23e401766916b7eb716f83515ca2e4e260a4b7451bf4fbbf2d7debdc35bb68e3fbf0bc2b7988ee6924b8b36e484d69be0caef9961e2f22d1433caf19a7467e097f1d079e461c777e22768d961111dd4df8c30a0c6c8c3795c26bb03436305a422a42f600f3d404de3b88caeea3ecb74f97151db1d43d7f1b116f1f74e4064e70ae7cf63d82f4a941a73c58b6a50d7fa225340540b23d99d4b4676119e9d01044ba459c148c919e7abf3e05f2f9b9c6\",\"mobileMAC\":\"DSPREAD\",\"studentdata\":{{\"macAddress\":\"27230100118102300052\",\"surname\":\"sabimen1426(010708)\",\"client_id\":\"sabimen1426(010708)\",\"client_trans_id\":\"BXOSS_AS_00{randomAlph}\"}},\"accType\":\"10\",\"channelType\":\"CDL\",\"terminalID\":\"2030DM85\",\"RRN\":\"BXOSS_AS{randomAlph}\",\"productID\":\"CDL\",\"ProcessorID\":\"ISW\",\"iccData\":\"57115399831605940416d201022100242295655f200f4f594152494e44452f4553544845525f3401025a0853998316059404169f260832f4c134bccc2e109f2701409f10120110600003220000000000000000000000ff9f37049ce0eb189f36020357950504400480009a032105239c01009f02060000000001005f2a020566820239009f1a0205669f03060000000000009f3303e040c89f34034203009f3501229f0607a00000000410105f2403201031\"}}";

                    TimeSent = DateTime.Now.ToString("hh:mm:ss fff");
                    var response = await httpClient.PostAsync("TransactionRequestOnWeb/", new StringContent(mrawData, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //var byteResult = await response.Content.ReadAsByteArrayAsync();
                        //string result = Convert.ToBase64String(byteResult);

                        string result = await response.Content.ReadAsStringAsync();

                        Console.WriteLine("Response No" + i.ToString());
                        Console.WriteLine($" Time Sent : {TimeSent} Time Received : {DateTime.Now.ToString("hh:mm:ss fff")} \nResponse String {result} \n");
                        Console.WriteLine();
                        i += 1;

                        if (!string.IsNullOrEmpty(result) && result.Contains("\"responseCode\":\"56\""))
                            getResponse += 1;
                        else
                            emptyRespnse += 1;
                    }
                    else
                    {
                        Console.WriteLine( "No response : " + response.StatusCode.ToString());
                        emptyRespnse += 1;
                    }
                }
                Console.WriteLine("Empty Response : " + emptyRespnse.ToString());
                Console.WriteLine("Response Gotten : " + getResponse.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error " + ex.Message);
                PostAPINextMinute();
            }

            Console.WriteLine("Hello World! - Press enter to start.");
            Console.ReadLine();
            PostAPI();

            Console.ReadLine();
        }

        private static string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.  

            // char is a single Unicode character  
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)rnd.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        private static void RunSemaPhore()
        {
            // Create the semaphore.
            semaphore = new SemaphoreSlim(0, 3);
            Console.WriteLine("{0} tasks can enter the semaphore.", semaphore.CurrentCount);
            Task[] tasks = new Task[5];

            // Create and start five numbered tasks.
            for (int i = 0; i <= 4; i++)
            {
                tasks[i] = Task.Run(() =>
                {
                    // Each task begins by requesting the semaphore.
                    Console.WriteLine("Task {0} begins and waits for the semaphore.", i);

                    int semaphoreCount;
                    semaphore.Wait();
                    try
                    {
                        Interlocked.Add(ref padding, 100);

                        Console.WriteLine("Task {0} enters the semaphore.", i);

                        // The task just sleeps for 1+ seconds.
                        Thread.Sleep(1000 + padding);
                    }
                    finally
                    {
                        semaphoreCount = semaphore.Release();
                    }
                    Console.WriteLine("Task {0} releases the semaphore; previous count: {1}.", i, semaphoreCount);
                });
            }

            // Wait for half a second, to allow all the tasks to start and block.
            Thread.Sleep(500);

            // Restore the semaphore count to its maximum value.
            Console.Write("Main thread calls Release(3) --> ");
            semaphore.Release(3);
            Console.WriteLine("{0} tasks can enter the semaphore.", semaphore.CurrentCount);
            // Main thread waits for the tasks to complete.
            Task.WaitAll(tasks);

            Console.WriteLine("Main thread exits.");
        }

        public async static void PostByteArray()
        {
            using var client = new HttpClient() { BaseAddress = new Uri("https://6560e304aa8e.ngrok.io/Api/") };
            var body = new ByteArrayContent(new byte[] { 1, 2, 3 });
            body.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            var response = await client.PostAsync("PostByte", body);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Response string : " + result.ToString());
            }
            else
            {
                Console.WriteLine("No response : " + response.StatusCode.ToString());
            }

            /*byte[] rawBytes = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
            Console.WriteLine("Sending Byte " + rawBytes.ToString());

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://6560e304aa8e.ngrok.io/Api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
            var byteArrayContent = new ByteArrayContent(rawBytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            var response = client.PostAsync("PostByte", byteArrayContent).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Response string : " + result.ToString());
            }
            else
            {
                Console.WriteLine("No response : " + response.StatusCode.ToString());
            }
            */
        }

    }
}
