using System;
using System.Timers;

namespace testproject2
{
    public delegate void LoadMain();
    struct SandwitchPrices
    {
        public static double Tuna = 9.99;
        public static double Detroit = 12.99;

        public static double Mystery = 21;
    }

    public enum Sandwitches
        {
            INVALID,
            TUNA,
            DETROIT,
            MYSTERY //will either give or take hunger from the player
        }


    public class Sandwitch
    {
        string name;
        double hungerVal;

        int id = -1;

        

        public string Name 
        {
            get {return name;}
            
        }

        public double HungerValue
        {
            get {return hungerVal;}
        }

        public int sandwitchID
        {
            get {return id;}
            set {id = value; }
        }
        public Sandwitch(int id)
        {
            switch(id)
            {
                case (int)Sandwitches.INVALID:
                
                name = "Empty";
                hungerVal = 0;

                id = (int)Sandwitches.INVALID;

               
                break;

                case (int)Sandwitches.TUNA:

                name = "Tuna Sandwitch";
                hungerVal = -4.20;
                
                id = (int)Sandwitches.TUNA;
                break;

                case (int)Sandwitches.DETROIT:
                name = "Detroit Sandwitch";
                hungerVal = -6.5;

                id = (int)Sandwitches.DETROIT;

                break;

                
                case (int)Sandwitches.MYSTERY:
                
                name = "Mystery Sandwitch";
                id = (int)Sandwitches.MYSTERY;

                Random rnd = new Random();

                hungerVal = rnd.Next(-100, 100);


                break;
            }
        

        }

        
    }
    class Program
    {

        //make a sandwitch booth with sandwitches
        enum Commands
        {
            INVALID,
            CMDS,
            SHOWMENU,
            BUY,
            SHOWHUNGER,
            SHOWINVENTORY,
            EAT
        }
//--------------------------------------------------------------------------------------------------------------------------
        

        //PLAYER ATTRIBUTES:

        static double cash = 200.5, hunger = 98;

        static Sandwitch[] inventory = new Sandwitch[10];

        static int FindEmptyInventorySlot()
        {
            int result = -1;

            for(int i = 0; i < inventory.Length; i++)
            {
                //System.Console.WriteLine(inventory[i].sandwitchID.ToString());

                if(inventory[i].sandwitchID == (int)Sandwitches.INVALID)
                {
                    result = i; 
                    
                    break;
                }
            }
            return result;
        }
        
        static bool PutItemInInventory(int itemID)
        {
            int slot = FindEmptyInventorySlot();



            if(slot != -1)
            {
                inventory[slot] = new Sandwitch(itemID);
                return true;
            }
            else System.Console.WriteLine("No inventory slot is available!"); return false;

        }

        public static void LoadInventory()
        {
            for(int i = 0; i < inventory.Length; i++)
            {
                inventory[i] = new Sandwitch((int)Sandwitches.INVALID);
                //System.Console.WriteLine($"Set inventory slot {i} to {inventory[i].Name} with ID {inventory[i].sandwitchID}");
                
                if(inventory[i].sandwitchID != (int)Sandwitches.INVALID) {
                    inventory[i].sandwitchID = (int)Sandwitches.INVALID;
                }

                //for some fucking reason it won't set the ID using a constructor, so that if statement above is needed
                

            }
        }

        public static void LoadStartupPrompt()
        {
            Console.WriteLine("Welcome to the sandwitch booth! Type cmds for commands");
            Console.WriteLine($"Cash in your pocket: ${cash}");
            CMD_ShowHunger();
        }

//--------------------------------------------------------------------------------------------------------------------------
       
        static void Main(string[] args)
        {

            LoadMain loadGame = new LoadMain(LoadStartupPrompt);

            loadGame += LoadInventory;        
            //try to convert this to using delegates, to declutter Main as much as possible[x]

           loadGame();
            /*
           Timer aTimer = new Timer();
           aTimer.Elapsed += new ElapsedEventHandler(sayHello);
           aTimer.Interval = 3000;
           aTimer.Enabled = true;
            */
        start:
            Console.Write(">");
            string input = Console.ReadLine();


            LaunchCommand(GetIDFromInput(input));

            goto start;

        }
        static void sayHello(object source, ElapsedEventArgs e)
        {
           System.Console.WriteLine("hi :D");
        
        }
        static int GetIDFromInput(string text)
        {
            int result;

            if (Enum.IsDefined(typeof(Commands), text.ToUpper()))
            {
                result = (int)Enum.Parse(typeof(Commands), text.ToUpper());
            }
            else result = -1;

            return result;
        }

        static void LaunchCommand(int comID)
        {
            switch (comID)
            {
                case (int)Commands.CMDS:
                    CMD_ShowCommands();
                    return;
                case (int)Commands.SHOWMENU:
                    CMD_ShowMenu();
                    return;

                case (int)Commands.BUY:
                    CMD_Buy();
                    return;

                case (int)Commands.SHOWHUNGER:
                    CMD_ShowHunger();
                    return;

                case (int)Commands.SHOWINVENTORY:
                    CMD_ShowInventory();
                    return;

                case (int)Commands.EAT:
                    CMD_Eat();
                    return;
            }



        }

        static void CMD_ShowCommands()
        {
            Console.Write("---Available Commands---\n");
            Console.WriteLine("cmds - Shows available commands");
            Console.WriteLine("showmenu - Will show the restaurant's menu.");
            System.Console.WriteLine("showinventory - will show what is in the player's inventory.");
            System.Console.WriteLine("buy - used to buy sandwitches");
            System.Console.WriteLine("eat - eat a sandwitch from your inventory");
            Console.WriteLine("");
        }

        static void CMD_ShowMenu()
        {
            Console.Write("---Menu---\t\t---Price---\n");
            Console.Write("1. Tuna Sandwitch\t $10\n");
            Console.Write("2. Detroit Sandwitch\t $12.99\n");
            Console.Write("3. Mystery Sandwitch\t $21\n");
            Console.Write(" * Use 'buy' command to buy a sandwitch.\n");
        }

        static void CMD_Buy()
        {
            Console.Write("\n");
            Console.WriteLine(" * Enter the number of the item in the menu you want to eat:");

            buyStart:
            Console.Write(" #>");

            int sandwitchID = -1;
            if (Int32.TryParse(Console.ReadLine(), out sandwitchID))
            {
                bool inventoryState = false; //if a free inventory slot is found or not

                switch(sandwitchID)
                {
                    
                    case (int)Sandwitches.TUNA:
                        
                        if(cash < SandwitchPrices.Tuna) System.Console.WriteLine($"Player cannot affort. Geat Success!");

                        else inventoryState = PutItemInInventory((int)Sandwitches.TUNA);

                        if(inventoryState) 
                        {
                            cash -= SandwitchPrices.Tuna;
                            Console.WriteLine($"You bought a Tuna Sandwitch for {SandwitchPrices.Tuna.ToString()}. Cash left: {cash}");
                            
                        }
                        else System.Console.WriteLine("Failed to put item in inventory.");

                        return;


                    case (int)Sandwitches.DETROIT:

                        if(cash < SandwitchPrices.Detroit) System.Console.WriteLine($"Player cannot affort. Geat Success!");
 
                        else inventoryState = PutItemInInventory((int)Sandwitches.DETROIT);

                        if(inventoryState) 
                        {
                            cash -= SandwitchPrices.Detroit;
                            Console.WriteLine($"You bought a Detroit Sandwitch for {SandwitchPrices.Detroit.ToString()}. Cash left: {cash}");
                            
                        }
                        else System.Console.WriteLine("Failed to put item in inventory.");


                        return;

                    case (int)Sandwitches.MYSTERY:

                    if(cash < SandwitchPrices.Mystery) System.Console.WriteLine($"Player cannot affort. Geat Success!");
 
                    else inventoryState = PutItemInInventory((int)Sandwitches.MYSTERY);

                    if(inventoryState) 
                        {
                            cash -= SandwitchPrices.Mystery;
                            Console.WriteLine($"You bought a Mystery Sandwitch for {SandwitchPrices.Mystery.ToString()}. Cash left: {cash}\nHunger Value: ");
                            
                        }
                        else System.Console.WriteLine("Failed to put item in inventory.");


                        return;



                    default: Console.WriteLine("Sandwitch number not available."); return;

                }
               
                //subtract player money
                //add sandwitch to inventory
                

            }

            else
            {
                Console.Write("Wrong number input.");
                goto buyStart;
            }
            

            

        }
        static void CMD_ShowHunger()
        {
             //{##########} 100%
             //{#####     } 50%

            char[] hungerBar = new char[12];
            hungerBar[0] = '{';
            hungerBar[11] = '}';
            

            //double hungerBars = ((hunger / 100 ) * 10) > hungerBar.Length ? hungerBar.Length - 2 : (hunger / 100 ) * 10;
            
            double hungerBars = ((hunger / 100) * 10 ) % hungerBar.Length;

            int counter = 0;
            while(counter  < Math.Round(hungerBars) ) 
            {
                counter++;
                hungerBar[counter] = '#';

            }
            

            for(int i = counter + 1; i <= hungerBar.Length - 2; ++i) hungerBar[i] = ' ';
            


            string _hungerBar = "";

            for(int i = 0; i <= hungerBar.Length - 1; i++)
            {
                _hungerBar = _hungerBar + hungerBar[i].ToString();
            }

            Console.Write("Player Hunger: " + _hungerBar + "\n");
            return;

        }


       
        static void CMD_ShowInventory()
        {
            Console.Write("----Player's Inventory----\n");

            for(int k = 0; k < inventory.Length; k++)
            {
                if(k != inventory.Length - 1) Console.WriteLine($"{k}. {inventory[k].Name}, {inventory[k].HungerValue},");

                else Console.WriteLine($"{k}. {inventory[k].Name}, {inventory[k].HungerValue}.");
                
            }

            Console.Write("-----------------------------\n");
        }

        static void CMD_Eat()
        {
            
            CMD_ShowInventory();

            System.Console.WriteLine("Enter the inventory slot containing the sandwitch you want to eat: ");

            int slot = -1;


            if(Int32.TryParse(Console.ReadLine(), out slot))
            {
                if(inventory[slot].Name == "Empty" && slot != -1)
                {
                    System.Console.WriteLine($"No sandwitch was found in inventory slot {slot}.");
                    
                }
                else 
                {
                    System.Console.WriteLine($"You have just eaten a {inventory[slot].Name}.");
                    hunger += inventory[slot].HungerValue;
                    CMD_ShowHunger();

                    inventory[slot] = new Sandwitch((int)Sandwitches.INVALID);
                }
            }
            else System.Console.WriteLine("Invalid input entered."); 
        }
    }


}
