using System;
using System.Threading.Tasks;

namespace BlackTrader
{
    internal class Program
    {
        private static readonly ItemDataPool DataPool=new ItemDataPool();
        public static void Main(string[] args)
        {
            new Task((async () =>
            {
                await DataPool.AddItem("T4_BAG");
                Console.WriteLine(DataPool.GetItem("T4_BAG",DataPool.Cities[0]).sell_price_max);
                
            })).RunSynchronously();
            Console.ReadKey();
            
        }

    }
}