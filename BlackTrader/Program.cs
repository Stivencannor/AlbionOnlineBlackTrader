using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BlackTrader.Commands;
using BlackTrader.Config;
using BlackTrader.DataPool;
using BlackTrader.Items;
using Console = System.Console;

namespace BlackTrader
{
    internal static class Program
    {
        private static readonly ItemDataPool DataPool = new ItemDataPool();
        

        public static void Main(string[] args)
        {
            Configs.Load();
            ConsoleCommands cmd;
            do
            {
                Console.WriteLine("Please Enter your Command:");
                //get command from console
                var cmdData = Console.ReadLine()?.Split(' ');
                Debug.Assert(cmdData != null, nameof(cmdData) + " != null");
                Debug.Assert(DataPool != null, nameof(DataPool) + " != null");
                Enum.TryParse(cmdData[0], true, out cmd);
                //calc command
                string[] itemsNamesList;
                ItemIds itemName;
                string lower;
                List<ItemIds> listOfFoundItems;
                switch (cmd)
                {
                    case ConsoleCommands.Help:
                    case ConsoleCommands.None:
                        Console.WriteLine("Please Enter a Command:");
                        var helpCmdList = Enum.GetNames(typeof(ConsoleCommands));
                        foreach (var hCmd in helpCmdList)
                            Console.WriteLine("- "+hCmd);
                        break;
                    case ConsoleCommands.End:
                    case ConsoleCommands.Exit:
                        Console.WriteLine("Ending Program. :D");
                        break;
                    case ConsoleCommands.ItemsList:
                        Console.WriteLine("Items List:");
                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        foreach (var itmName in itemsNamesList)
                            Console.Write(itmName + ", ");
                        Console.WriteLine("");
                        break;
                    case ConsoleCommands.SearchAndAdd:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.SearchAndAdd + " ItemId");
                            break;
                        }

                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        Console.WriteLine("Items that contain " + cmdData[1] + " :");
                        lower = cmdData[1].ToLower();
                        listOfFoundItems = new List<ItemIds>();
                        foreach (var itmName in itemsNamesList)
                            if (itmName.ToLower().Contains(lower) && Enum.TryParse<ItemIds>(itmName, out var itmId))
                                listOfFoundItems.Add(itmId);
                        DataPool?.AddItem(listOfFoundItems.ToArray());
                        break;
                    case ConsoleCommands.SearchAndUpdate:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.SearchAndAdd + " ItemId");
                            break;
                        }

                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        Console.WriteLine("Items that contain " + cmdData[1] + " :");
                        lower = cmdData[1].ToLower();
                        listOfFoundItems = new List<ItemIds>();
                        foreach (var itmName in itemsNamesList)
                            if (itmName.ToLower().Contains(lower) && Enum.TryParse<ItemIds>(itmName, out var itmId))
                                listOfFoundItems.Add(itmId);
                        DataPool?.Update(listOfFoundItems.ToArray());
                        break;
                    case ConsoleCommands.Search:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Search + " ItemId");
                            break;
                        }

                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        Console.WriteLine("Items that contain " + cmdData[1] + " :");
                        lower = cmdData[1].ToLower();
                        var itemCounts = 0;
                        foreach (var itmName in itemsNamesList)
                            if (itmName.ToLower().Contains(lower))
                            {
                                Console.WriteLine(itmName);
                                itemCounts++;
                            }

                        Console.WriteLine("Found " + itemCounts + " items.");
                        break;
                    case ConsoleCommands.Add:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Add +
                                              " ItemId \nTo get item lists use " + ConsoleCommands.ItemsList +
                                              "command or  search item by " + ConsoleCommands.Search +
                                              " command.");
                            break;
                        }

                        if (Enum.TryParse(cmdData[1].Replace("@","_AtSign_"), true, out itemName))
                            DataPool?.AddItem(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              ConsoleCommands.Search + " command or " +
                                              ConsoleCommands.ItemsList);
                        break;
                    case ConsoleCommands.Remove:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Remove +
                                              " ItemId \nTo get item lists use " + ConsoleCommands.ItemsList +
                                              "command or  search item by " + ConsoleCommands.Search +
                                              " command.");
                            break;
                        }

                        if (Enum.TryParse(cmdData[1].Replace("@","_AtSign_"), true, out itemName))
                            DataPool?.RemoveItem(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              ConsoleCommands.Search + " command or " +
                                              ConsoleCommands.ItemsList);
                        break;
                    case ConsoleCommands.Update:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine("You are not entered any item id, we are updating all of them.");
                            DataPool?.Update();
                            break;
                        }

                        if (Enum.TryParse(cmdData[1].Replace("@","_AtSign_"), true, out itemName))
                            DataPool?.Update(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              ConsoleCommands.Search + " command or " +
                                              ConsoleCommands.ItemsList);
                        break;
                    case ConsoleCommands.TradeOffers:

                        // ReSharper disable once LocalFunctionCanBeMadeStatic
                        void PrintPairs(
                            IEnumerable<ItemDataPool.DataPoolAnalyser.TradeOfferPair> tradeOffersFromCity)
                        {
                            foreach (var pair in tradeOffersFromCity)
                                Console.WriteLine(pair.ToString());
                        }

                        if (cmdData.Length == 3)
                        {
                            Console.WriteLine("Trade offers for " + cmdData[1] + " city to " + cmdData[2] + " city:");
                            PrintPairs(ItemDataPool.DataPoolAnalyser.TradeOffersFromCity(DataPool, cmdData[1],
                                cmdData[2]));
                            Console.WriteLine("Using this data is a big risk. not calculated number of items.");
                            break;
                        }
                        else if (cmdData.Length == 2)
                        {
                            Console.WriteLine("Trade offers for " + cmdData[1] + " city:");
                            PrintPairs(ItemDataPool.DataPoolAnalyser.TradeOffersFromCity(DataPool, cmdData[1]));
                            Console.WriteLine("Using this data is a big risk. not calculated number of items.");
                            break;
                        }

                        Console.WriteLine("Trade offers for All cities:");
                        PrintPairs(ItemDataPool.DataPoolAnalyser.TradeOffersFromCity(DataPool));
                        Console.WriteLine("Using this data is a big risk. not calculated number of items.");
                        break;
                    case ConsoleCommands.CityNames:
                        var cities = DataPool.GetCities();
                        if (cities.Length < 1)
                        {
                            Console.WriteLine("Please add item to data pool at first by " + ConsoleCommands.Add +
                                              " command.");
                            break;
                        }

                        Console.WriteLine("Cities List:");
                        foreach (var city in cities)
                            Console.Write(city + ", ");
                        Console.WriteLine();
                        break;
                    case ConsoleCommands.Save:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Save + " filename");
                            break;
                        }

                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), cmdData[1] + ".json"),
                            DataPool.GetJson());
                        Console.WriteLine(cmdData[1] + " saved.");
                        break;
                    case ConsoleCommands.Load:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Load + " filename");
                            break;
                        }

                        if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(),
                            cmdData[1] + ".json")))
                        {
                            Console.WriteLine(cmdData[1] + ".json File Not Exist.");
                            break;
                        }

                        DataPool.SetJson(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(),
                            cmdData[1] + ".json")));
                        Console.WriteLine(cmdData[1] + " loaded.");
                        break;
                    case ConsoleCommands.GetItems:
                        Console.WriteLine("Current items that exist at pool:");
                        var itemIds = DataPool?.GetItems();
                        foreach (var itm in itemIds)
                            Console.Write(itm + ", ");
                        Console.WriteLine("Found "+itemIds.Length+" items.");
                        break;
                    case ConsoleCommands.WhereSell:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.WhereSell + " ItemID");
                            break;
                        }

                        Console.WriteLine("best prices is:");
                        if (Enum.TryParse<ItemIds>(cmdData[1].Replace("@","_AtSign_"), true, out var itmId2))
                        {
                            var items = ItemDataPool.DataPoolAnalyser.WhereSell(DataPool, itmId2);
                            foreach (var itm in items)
                                Console.WriteLine("You can sell " + itm.item_id + " to " + itm.city + " over " +
                                                  itm.buy_price_min);
                        }

                        break;
                    case ConsoleCommands.WhereBuy:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.WhereBuy + " ItemID");
                            break;
                        }

                        Console.WriteLine("best prices is:");
                        if (Enum.TryParse<ItemIds>(cmdData[1].Replace("@","_AtSign_"), true, out var itmId3))
                        {
                            var items = ItemDataPool.DataPoolAnalyser.WhereBuy(DataPool, itmId3);
                            foreach (var itm in items)
                                Console.WriteLine("You can buy " + itm.item_id + " from " + itm.city + " over " +
                                                  itm.sell_price_min);
                        }

                        break;
                    case ConsoleCommands.Conf:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Conf + " "+nameof(ConfigCommands));
                            Console.WriteLine(nameof(ConfigCommands)+ " Commands List:");
                            var confCmdList = Enum.GetNames(typeof(ConfigCommands));
                            foreach (var hCmd in confCmdList)
                                Console.WriteLine("- "+ConsoleCommands.Conf+" "+hCmd);
                            break;
                        }

                        if(Enum.TryParse<ConfigCommands>(cmdData[1],true,out var confCmd))
                            switch (confCmd)
                            {
                                case ConfigCommands.ChangeDateHoursLimit:
                                    if (cmdData.Length < 3)
                                    {
                                        Console.WriteLine(@"Usage is: " + ConsoleCommands.Conf + " "+ConfigCommands.ChangeDateHoursLimit+" HoursNumber");
                                        break;
                                    }

                                    if (int.TryParse(cmdData[2], out var hours))
                                    {
                                        Console.WriteLine(nameof(Configs.UpdateDateHoursLimit)+" changed from "+Configs.UpdateDateHoursLimit+" to "+hours+".");
                                        Configs.UpdateDateHoursLimit = hours;
                                    }
                                    else
                                        Console.WriteLine("Cant Detect Hours Number, Please enter integer number!");
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        Configs.Save();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (cmd != ConsoleCommands.End && cmd != ConsoleCommands.Exit);
        }
    }
}