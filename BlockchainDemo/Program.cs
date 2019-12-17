using Newtonsoft.Json;
using System;
using System.IO;
using System.Configuration;

namespace BCTestDemo
{
    class Program
    {
        public static int Port = 0;
        public static P2PServer Server = null;
        public static P2PClient Client = new P2PClient();
        public static Blockchain BCDemo = new Blockchain();
        public static string name = "unknown";

    

        static void Main(string[] args)
        {

            BCDemo.InitializeChain();

            string ws = ConfigurationManager.AppSettings["ws"];
            string uripath = ConfigurationManager.AppSettings["uripath"];
            string port = ConfigurationManager.AppSettings["port"];
            string user = ConfigurationManager.AppSettings["user"];
            string bchainpath = ConfigurationManager.AppSettings["bchainpath"];

            if (args.Length >= 1)
                Port = int.Parse(args[0]);
            if (args.Length >= 2)
                name = args[1];

            Program.Port = int.Parse(port);
                
            if (Port > 0)
            {
                Server = new P2PServer();
                Server.Start();
            }

            name = user;

            if (name != "unkown")
            {
                Console.WriteLine($"Current user is {name}");
            }

           

            int selection = 0;
            while (selection != 4)
            {
                switch (selection)
                {
                    case 1:
                        Console.WriteLine("Please enter the server URL (enter 0 to cancel the operation)");
                        string serverURL = Console.ReadLine();
                        if (serverURL == "0")
                            break;
                        P2PClient.Connect($"{serverURL}{uripath}");
                        break;
                    case 2:
                        Transaction trans = new Transaction();
                        Console.WriteLine("Please enter street address (enter 0 to cancel the operation)");
                        trans.streetAddress  = Console.ReadLine();
                        if (trans.streetAddress == "0")   
                        break;
                        
                        Console.WriteLine("Please enter the city (enter 0 to cancel the operation)");
                        trans.city = Console.ReadLine();
                        if (trans.city == "0")
                            break;

                        Console.WriteLine("Please enter the state (enter 0 to cancel the operation)");
                        trans.state = Console.ReadLine();
                        if (trans.state == "0")
                            break;


                        Console.WriteLine("Please enter the zip (enter 0 to cancel the operation)");
                        trans.zip = Console.ReadLine();
                        if (trans.zip == "0")
                            break;

                        Console.WriteLine("Please enter the property value (enter 0 to cancel the operation)");
                        trans.propertyValue = int.Parse(Console.ReadLine());
                        if (trans.propertyValue == 0)
                            break;

                        Console.WriteLine("Please enter the lender (enter 0 to cancel the operation)");
                        trans.lender  = Console.ReadLine();
                        if (trans.lender == "0")
                            break;

                        Console.WriteLine("Please enter the customer (enter 0 to cancel the operation)");
                        trans.customer = Console.ReadLine();
                        if (trans.customer == "0")
                            break;

                        
                        trans.clip = Guid.NewGuid().ToString("N").ToUpper();
                        
                        

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

                        //BCDemo.CreateTransaction(new Transaction(name, receiverName, int.Parse(amount), ownerName));
                        //BCDemo.ProcessPendingTransactions(name, receiverName, ownerName);

                        //int prime = BCDemo.GetBalance(name);
                        //int recv = BCDemo.GetBalance(receiverName);

                        //if (prime < 0)
                        //{
                        //    Console.WriteLine(name + " negative balance:  " + prime);
                        //}
                        //else
                        //{
                        //    Console.WriteLine(name + " balance:  " + prime);

                        //}

                        //Console.WriteLine(receiverName + " balance:  " + recv);

                        P2PClient.Broadcast(JsonConvert.SerializeObject(BCDemo));
                        break;
                    case 3:
                        Console.WriteLine("Blockchain");
                        var obj = JsonConvert.SerializeObject(BCDemo, Formatting.Indented);
                        Console.WriteLine(obj);
                       ;
                        if (!Directory.Exists(bchainpath))
                        {
                        Directory.CreateDirectory(bchainpath);
                        }
                        string filename = bchainpath + name + "-" + DateTime.Now.ToString("mmddyyyyhhmmss") + ".json";
                        File.WriteAllText(filename, JsonConvert.SerializeObject(BCDemo, Formatting.Indented));
                        
                        JournalOutput(obj);
                       
                        break;

                }

                Console.WriteLine(Environment.NewLine + "<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
                Console.WriteLine("1. Join the network");
                Console.WriteLine("2. Add a transaction");
                Console.WriteLine("3. Display Current Blockchain");
                Console.WriteLine("4. Exit");
                Console.WriteLine(">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>" + Environment.NewLine);
                Console.WriteLine("Please select an option....");
                string action = Console.ReadLine();
                selection = int.Parse(action);
            }

            if (Client != null)
            {
                try
                {
                    Client.Close();
                }
                finally
                {
                    Client = null;
                }
            }
        }

      
        public static string JournalOutput(string obj)
        {
            var result = "";
            Console.WriteLine(Environment.NewLine + "Journal Output");

            RootObject ro = JsonConvert.DeserializeObject<RootObject>(obj);
            foreach (var cn in ro.Chain)
            {
                foreach (var trn in cn.Transactions)
                {
                    result = trn.clip  + "," + trn.streetAddress + "," + trn.state + "," + trn.city + "," + trn.zip  + "," +
                              trn.propertyValue + "," + trn.lender + "," + trn.customer + "~";
                    Console.WriteLine("\n{0}", result);
                }
            }

            return result.ToString();
        }
    }
}
