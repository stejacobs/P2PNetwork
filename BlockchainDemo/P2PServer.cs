using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace BCTestDemo
{
    public class P2PServer : WebSocketBehavior
    {
        bool chainSynched = false;
        WebSocketServer wss = null;
        //private P2PClient Client = null;
        //public static Blockchain BCDemo = new Blockchain();

        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            //wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            wss.AddWebSocketService<P2PServer>("/bcdemo");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
            Program.BCDemo.InitializeChain();
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            //if (Client == null)
            //{
             //   Client = new P2PClient();
               
            //}

            if (e.Data.Contains("Hi Server"))
            {
                Console.WriteLine(e.Data);
                Send($"From {Program.Port}: Hi Client");
            }
         


            string[] selection = e.Data.Split(",");

            switch (selection[0])
            {

                case "1":
                    P2PClient.Connect(selection[1]);
                    break;

                case "3":
                    var obj = JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented);
                    Console.WriteLine(obj);
                    Send(obj);
                    break;
                case "2":
                    string[] x = e.Data.Split(",");
                    string name = x[1];
                    string receiverName = x[2];
                    string amount = x[3];
                    string ownerName = x[4];


                    Program.BCDemo.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount),
                        ownerName));
                    Program.BCDemo.ProcessPendingTransactions(name, receiverName, ownerName);

                    int prime = Program.BCDemo.GetBalance(name);
                    int recv = Program.BCDemo.GetBalance(receiverName);

                    if (prime < 0)
                    {
                        Console.WriteLine(name + " negative balance:  " + prime);
                    }
                    else
                    {
                        Console.WriteLine(name + " balance:  " + prime);

                    }

                    Console.WriteLine(receiverName + " balance:  " + recv);

                    //Create Journal File
                    var journal = JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented);
                    string jf = Program.JournalOutput(journal);
                    Send(jf);
                    string filename = @"c:\test\" + ownerName + "-" + DateTime.Now.ToString("yyyyMMdd") + ".json";
                    File.WriteAllText(filename, JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented));
                    break;
                 }

            P2PClient.Broadcast(JsonConvert.SerializeObject(Program.BCDemo));
            Send(JsonConvert.SerializeObject(Program.BCDemo));
            //Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(JsonConvert.SerializeObject(Program.BCDemo));
            
            //if (newChain.IsValid() && newChain.Chain.Count > Program.BCDemo.Chain.Count)
            //{
            //    Program.BCDemo.Chain = newChain.Chain;
            //}

        

            //if (!chainSynched)
            //{
            //    Send(JsonConvert.SerializeObject(Program.BCDemo));
            //    chainSynched = true;
            //}
        }
    }
}










