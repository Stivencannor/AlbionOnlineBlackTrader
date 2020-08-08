using System;
using System.Collections.Generic;
using BlackTrader.Commands;
using BlackTrader.Config;
using BlackTrader.DataPool;
using BlackTrader.Items;

namespace BlackTrader
{
    internal static class Program
    {
        private static readonly ItemDataPool DataPool = new ItemDataPool();

        public static void Main(string[] args)
        {
            Configs.Load();
            DataPool.Load(Configs.NameOfDataPoolAutoSaveFile);
            ConsoleCommands cmd;
            do
            {
                Console.WriteLine("Please Enter your Command:");
                //get command from console
                var cmdData = Console.ReadLine()?.Split(' ');
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
                            Console.WriteLine("- " + hCmd);
                        break;
                    case ConsoleCommands.End:
                    case ConsoleCommands.Exit:
                        Console.WriteLine("Ending Program. :D");
                        break;
                    case ConsoleCommands.AddSearch:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.AddSearch + " ItemId");
                            break;
                        }

                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        Console.WriteLine("Items that contain " + cmdData[1] + " :");
                        lower = cmdData[1].ToLower();
                        listOfFoundItems = new List<ItemIds>();
                        foreach (var itmName in itemsNamesList)
                            if (itmName.ToLower().Contains(lower) && Enum.TryParse<ItemIds>(itmName, out var itmId))
                                listOfFoundItems.Add(itmId);
                        DataPool?.Add(listOfFoundItems.ToArray());
                        break;
                    case ConsoleCommands.UpdateSearch:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.UpdateSearch + " ItemId");
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
                            Console.WriteLine(@"or : " + ConsoleCommands.Search + " Command ItemId");
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
                                              " ItemId \nTo get item lists use " + ConsoleCommands.Items +
                                              "command or  search item by " + ConsoleCommands.Search +
                                              " command.");
                            break;
                        }

                        if (Enum.TryParse(cmdData[1].Replace("@", "_AtSign_"), true, out itemName))
                            DataPool?.Add(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              ConsoleCommands.Search + " command or " +
                                              ConsoleCommands.Items);
                        break;
                    case ConsoleCommands.Remove:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Remove +
                                              " ItemId \nTo get item lists use " + ConsoleCommands.Items +
                                              "command or  search item by " + ConsoleCommands.Search +
                                              " command.");
                            break;
                        }

                        if (Enum.TryParse(cmdData[1].Replace("@", "_AtSign_"), true, out itemName))
                            DataPool?.Remove(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              ConsoleCommands.Search + " command or " +
                                              ConsoleCommands.Items);
                        break;
                    case ConsoleCommands.Update:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine("You are not entered any item id, we are updating all of them.");
                            DataPool?.Update();
                            break;
                        }

                        if (Enum.TryParse(cmdData[1].Replace("@", "_AtSign_"), true, out itemName))
                            DataPool?.Update(itemName);
                        else
                            Console.WriteLine(cmdData[1] + " Item not Exist. please search id by " +
                                              ConsoleCommands.Search + " command or " +
                                              ConsoleCommands.Items);
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
                    case ConsoleCommands.City:
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
                        DataPool.Save(cmdData.Length < 2 ? Configs.NameOfDataPoolAutoSaveFile : cmdData[1]);
                        Console.WriteLine(cmdData[1] + " saved.");
                        break;
                    case ConsoleCommands.Load:
                        bool loaded;
                        loaded = DataPool.Load(cmdData.Length < 2 ? Configs.NameOfDataPoolAutoSaveFile : cmdData[1]);
                        if (loaded)
                            Console.WriteLine(cmdData[1] + " loaded.");
                        else
                            Console.WriteLine(cmdData[1] + ".json not exist to load at data pool.");
                        if (cmdData.Length >= 1)
                            DataPool.Save(Configs.NameOfDataPoolAutoSaveFile);
                        break;
                    case ConsoleCommands.Items:
                        Console.WriteLine("Current items that exist at pool:");
                        var itemIds = DataPool?.GetItems();
                        foreach (var itm in itemIds)
                            Console.Write(itm + ", ");
                        Console.WriteLine("Found " + itemIds.Length + " items.");
                        break;
                    case ConsoleCommands.WhereSell:
                        if (cmdData.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.WhereSell + " ItemID");
                            break;
                        }

                        Console.WriteLine("best prices is:");
                        if (Enum.TryParse<ItemIds>(cmdData[1].Replace("@", "_AtSign_"), true, out var itmId2))
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
                        if (Enum.TryParse<ItemIds>(cmdData[1].Replace("@", "_AtSign_"), true, out var itmId3))
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
                            Console.WriteLine(@"Usage is: " + ConsoleCommands.Conf + " " + nameof(ConfigCommands));
                            Console.WriteLine(nameof(ConfigCommands) + " Commands List:");
                            var confCmdList = Enum.GetNames(typeof(ConfigCommands));
                            foreach (var hCmd in confCmdList)
                                Console.WriteLine("- " + ConsoleCommands.Conf + " " + hCmd);
                            break;
                        }

                        if (Enum.TryParse<ConfigCommands>(cmdData[1], true, out var confCmd))
                            switch (confCmd)
                            {
                                case ConfigCommands.ChangeDateHoursLimit:
                                    if (cmdData.Length < 3)
                                    {
                                        Console.WriteLine(@"Usage is: " + ConsoleCommands.Conf + " " +
                                                          ConfigCommands.ChangeDateHoursLimit + " HoursNumber");
                                        break;
                                    }

                                    if (int.TryParse(cmdData[2], out var hours))
                                    {
                                        Console.WriteLine(nameof(Configs.UpdateDateHoursLimit) + " changed from " +
                                                          Configs.UpdateDateHoursLimit + " to " + hours + ".");
                                        Configs.UpdateDateHoursLimit = hours;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Cant Detect Hours Number, Please enter integer number!");
                                    }

                                    break;
                                case ConfigCommands.ChangeAutoSaveDataPoolFileName:
                                    if (cmdData.Length < 3)
                                    {
                                        Console.WriteLine(@"Usage is: " + ConsoleCommands.Conf + " " +
                                                          ConfigCommands.ChangeAutoSaveDataPoolFileName + " FileName");
                                        break;
                                    }

                                    Console.WriteLine(nameof(Configs.NameOfDataPoolAutoSaveFile) + " changed from " +
                                                      Configs.NameOfDataPoolAutoSaveFile + " to " + cmdData[2] + ".");
                                    Configs.NameOfDataPoolAutoSaveFile = cmdData[2];
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                        Configs.Save();
                        break;
                    case ConsoleCommands.Clear:
                        DataPool.Remove();
                        Console.WriteLine("Data Pool Cleared.");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (cmd != ConsoleCommands.End && cmd != ConsoleCommands.Exit);
        }
    }
}