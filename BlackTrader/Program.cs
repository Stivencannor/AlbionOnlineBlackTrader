using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace BlackTrader
{
    internal static class Program
    {
        private static readonly ItemDataPool DataPool = new ItemDataPool();

        public static void Main(string[] args)
        {
            Commands cmd;
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
                switch (cmd)
                {
                    case Commands.Help:
                    case Commands.None:
                        Console.WriteLine("Please Enter a Command:");
                        var helpCmdList = Enum.GetNames(typeof(Commands));
                        foreach (var hCmd in helpCmdList)
                            Console.WriteLine(hCmd);
                        break;
                    case Commands.End:
                    case Commands.Exit:
                        Console.WriteLine("Ending Program. :D");
                        break;
                    case Commands.ItemsList:
                        Console.WriteLine("Items List:");
                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        foreach (var itmName in itemsNamesList)
                            Console.Write(itmName + ", ");
                        Console.WriteLine("");
                        break;
                    case Commands.SearchAndAddItem:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.SearchAndAddItem + " ItemId");
                            break;
                        }

                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        Console.WriteLine("Items that contain " + cmdData[1] + " :");
                        lower = cmdData[1].ToLower();
                        var listOfFoundItems = new List<ItemIds>();
                        foreach (var itmName in itemsNamesList)
                            if (itmName.ToLower().Contains(lower) && Enum.TryParse<ItemIds>(itmName, out var itmId))
                                listOfFoundItems.Add(itmId);
                        DataPool?.AddItem(listOfFoundItems.ToArray());
                        break;
                    case Commands.SearchItem:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.SearchItem + " ItemId");
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
                    case Commands.AddItem:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.AddItem +
                                              " ItemId \nTo get item lists use " + Commands.ItemsList +
                                              "command or  search item by " + Commands.SearchItem + " command.");
                            break;
                        }

                        if (Enum.TryParse(cmdData[1], true, out itemName))
                            DataPool?.AddItem(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              Commands.SearchItem + " command or " + Commands.ItemsList);
                        break;
                    case Commands.RemoveItem:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.RemoveItem +
                                              " ItemId \nTo get item lists use " + Commands.ItemsList +
                                              "command or  search item by " + Commands.SearchItem + " command.");
                            break;
                        }

                        if (Enum.TryParse(cmdData[1], true, out itemName))
                            DataPool?.RemoveItem(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              Commands.SearchItem + " command or " + Commands.ItemsList);
                        break;
                    case Commands.Update:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine("You are not entered any item id, we are updating all of them.");
                            DataPool?.Update();
                            break;
                        }

                        if (Enum.TryParse(cmdData[1], true, out itemName))
                            DataPool?.Update(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              Commands.SearchItem + " command or " + Commands.ItemsList);
                        break;
                    case Commands.TradeOffers:

                        // ReSharper disable once LocalFunctionCanBeMadeStatic
                        void PrintPairs(
                            IEnumerable<ItemDataPool.DataPoolAnalyser.TradeOfferPair> tradeOffersFromCity)
                        {
                            foreach (var pair in tradeOffersFromCity)
                                Console.WriteLine(pair.ToString());
                        }

                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine("Trade offers for All cities:");
                            PrintPairs(ItemDataPool.DataPoolAnalyser.TradeOffersFromCity(DataPool));
                            Console.WriteLine("Using this data is a big risk. not calculated number of items.");
                            break;
                        }

                        Console.WriteLine("Trade offers for " + cmdData[1] + " city:");
                        PrintPairs(ItemDataPool.DataPoolAnalyser.TradeOffersFromCity(DataPool, cmdData[1]));
                        Console.WriteLine("Using this data is a big risk. not calculated number of items.");
                        break;
                    case Commands.CityNames:
                        var cities = DataPool.GetCities();
                        if (cities.Length < 1)
                        {
                            Console.WriteLine("Please add item to data pool at first by " + Commands.AddItem +
                                              " command.");
                            break;
                        }

                        Console.WriteLine("Cities List:");
                        foreach (var city in cities)
                            Console.Write(city + ", ");
                        Console.WriteLine();
                        break;
                    case Commands.Save:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.Save + " filename");
                            break;
                        }

                        File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), cmdData[1] + ".json"),
                            DataPool.GetJson());
                        Console.WriteLine(cmdData[1] + " saved.");
                        break;
                    case Commands.Load:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.Load + " filename");
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
                    case Commands.PoolItems:
                        Console.WriteLine("Current items that exist at pool:");
                        var itemIds = DataPool?.GetItems();
                        foreach (var itm in itemIds)
                            Console.Write(itm + ", ");
                        Console.WriteLine("");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (cmd != Commands.End && cmd != Commands.Exit);
        }
    }
}