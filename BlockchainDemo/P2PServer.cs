using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Text;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;
using System.Configuration;

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
            var ws = ConfigurationManager.AppSettings["ws"];
            Console.WriteLine($"{ws}{Program.Port}");
            wss = new WebSocketServer($"{ws}{Program.Port}");
            //wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            wss.AddWebSocketService<P2PServer>("/bcdemo");
            wss.Start();
            Console.WriteLine($"{ws}{Program.Port}");
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

                    Transaction trans = new Transaction();
                    trans.clip = Guid.NewGuid().ToString("N").ToUpper();
                    trans.streetAddress = x[1];
                    trans.city = x[2];
                    trans.state  = x[3];
                    trans.zip  = x[4];
                    trans.propertyValue  = int.Parse(x[5]);
                    trans.lender  = x[6];
                    trans.customer = x[7];





                    Program.BCDemo.CreateTransaction(trans);
                    Program.BCDemo.ProcessPendingTransactions(
                        trans.clip,
                        trans.streetAddress,
                        trans.city,
                        trans.state,
                        trans.zip,
                        trans.propertyValue,
                        trans.lender,
                        trans.customer);

                    //int prime = Program.BCDemo.GetBalance(name);
                    //int recv = Program.BCDemo.GetBalance(receiverName);

                    //if (prime < 0)
                    //{
                    //    Console.WriteLine(name + " negative balance:  " + prime);
                    //}
                    //else
                    //{
                    //    Console.WriteLine(name + " balance:  " + prime);

                    //}

                    //Console.WriteLine(receiverName + " balance:  " + recv);

                    //Create Journal File
                    var journal = JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented);
                    string jf = Program.JournalOutput(journal);
                    Send(jf);
                    string dir = ConfigurationManager.AppSettings["bchainpath"];
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    string filename = dir + "CLBChain-" + DateTime.Now.ToString("mmddyyyyhhmmss") + ".json";
                    File.WriteAllText(filename, JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented));
                    trans = null;
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










