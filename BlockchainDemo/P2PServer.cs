using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private P2PClient Client = null;

        public void Start()
        {
            wss = new WebSocketServer($"ws://127.0.0.1:{Program.Port}");
            //wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            wss.AddWebSocketService<P2PServer>("/bcdemo");
            wss.Start();
            Console.WriteLine($"Started server at ws://127.0.0.1:{Program.Port}");
         Client = new P2PClient();

        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (Client == null)
            {
                Client = new P2PClient();
            }

            if (e.Data.Contains("Hi Server"))
            {
                Console.WriteLine(e.Data);
                Send($"From {Program.Port}: Hi Client");
            }
            else
            {
               
            }


            string[] selection = e.Data.Split(",");
           
                switch (selection[0])
                {
                    case "3":
                    
                        Program.BCDemo.InitializeChain();
                        var obj = JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented);
                        Console.WriteLine(obj);

                        Send("<p>Blockchain JSON</p>" + obj);
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
                        //Send(JsonConvert.SerializeObject(Program.BCDemo));
                      
                        
                        Client.Broadcast(JsonConvert.SerializeObject(Program.BCDemo));
                    Blockchain newChain = JsonConvert.DeserializeObject<Blockchain>(JsonConvert.SerializeObject(Program.BCDemo));

                    if (newChain.IsValid() && newChain.Chain.Count > Program.BCDemo.Chain.Count)
                    {
                        Program.BCDemo.Chain = newChain.Chain;
                        Send("I am here");
                    }
                       // var journal = JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented);
                       //string jf = Program.JournalOutput(journal);
                        Send("<p>Blockchain New Transaction</p>" + receiverName + " balance:  " + recv);

                    if (!chainSynched)
                    {
                        //Send(JsonConvert.SerializeObject(Program.BCDemo));
                        chainSynched = true;
                    }
                    break;
                case "5":
                    Program.BCDemo.InitializeChain();
                    var jfile = Program.JournalOutput(JsonConvert.SerializeObject(Program.BCDemo, Formatting.Indented));
                    Console.WriteLine("<p>Blockchain Journal/Ledger</p>" + jfile);

                    Send(jfile);
                    break;

                
            }
        }
    }
}

        






        
           
