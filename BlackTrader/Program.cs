using System;

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
                Enum.TryParse(cmdData?[0], true, out cmd);
                //calc command
                string[] itemsNamesList;
                ItemIds itemName;
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
                        Console.WriteLine("Ending Program. :D");
                        break;
                    case Commands.ItemsList:
                        Console.WriteLine("Items List:");
                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        foreach (var itmName in itemsNamesList)
                            Console.Write(itmName + ", ");
                        Console.WriteLine("");
                        break;
                    case Commands.SearchItem:
                        if (cmdData == null || cmdData?.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.SearchItem + " ItemId");
                            break;
                        }

                        itemsNamesList = Enum.GetNames(typeof(ItemIds));
                        Console.WriteLine("Items that contain " + cmdData?[1] + " :");
                        foreach (var itmName in itemsNamesList)
                            if (itmName.Contains(cmdData[1]))
                                Console.WriteLine(itmName);
                        break;
                    case Commands.AddItem:
                        if (cmdData == null || cmdData?.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.AddItem +
                                              " ItemId \nTo get item lists use " + Commands.ItemsList +
                                              "command or  search item by " + Commands.SearchItem + " command.");
                            break;
                        }

                        if (Enum.TryParse<ItemIds>(cmdData?[1], true, out itemName))
                            DataPool?.AddItem(itemName);
                        else
                            Console.WriteLine(cmdData?[1] + " Item not Exist. please search id by " +
                                              Commands.SearchItem + " command or " + Commands.ItemsList);
                        break;
                    case Commands.RemoveItem:
                        if (cmdData == null || cmdData?.Length < 2)
                        {
                            Console.WriteLine(@"Usage is: " + Commands.RemoveItem +
                                              " ItemId \nTo get item lists use " + Commands.ItemsList +
                                              "command or  search item by " + Commands.SearchItem + " command.");
                            break;
                        }

                        if (Enum.TryParse<ItemIds>(cmdData?[1], true, out itemName))
                            DataPool?.RemoveItem(itemName);
                        else
                            Console.WriteLine(cmdData?[1] + " Item not Exist. please search id by " +
                                              Commands.SearchItem + " command or " + Commands.ItemsList);
                        break;
                    case Commands.Update:
                        if (cmdData == null || cmdData?.Length < 2)
                        {
                            DataPool?.Update();
                            break;
                        }

                        if (Enum.TryParse<ItemIds>(cmdData?[1], true, out itemName))
                            DataPool?.Update(itemName);
                        else
                            Console.WriteLine(cmdData?[1] + " Item not Exist. please search id by " +
                                              Commands.SearchItem + " command or " + Commands.ItemsList);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            } while (cmd != Commands.End);
        }
    }
}