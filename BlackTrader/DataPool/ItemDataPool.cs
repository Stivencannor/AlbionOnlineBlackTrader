using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BlackTrader.Items;
using Newtonsoft.Json;

namespace BlackTrader.DataPool
{
    public partial class ItemDataPool
    {
        private const float WaitSecs = 0.5f;
        private readonly HttpClient client = new HttpClient();
        private Dictionary<ItemIds, ItemDetails[]> itemDataPool = new Dictionary<ItemIds, ItemDetails[]>();

        public ItemDetails GetItem(ItemIds itemName, string cityName)
        {
            foreach (var item in itemDataPool[itemName])
                if (item.city.Equals(cityName))
                    return item;
            return new ItemDetails();
        }

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public ItemIds[] GetItems()
        {
            return itemDataPool.Keys.ToArray();
        }

        public void RemoveItem(ItemIds itemName)
        {
            itemDataPool.Remove(itemName);
        }

        public void Update()
        {
            var itemsList = GetItems();
            Console.WriteLine("Updating " + itemsList.Length + " items. it may take long.");
            var counter = 1;
            foreach (var itemName in itemsList)
            {
                Update(itemName);
                Console.WriteLine("[" + ++counter + "/" + itemsList.Length + "] Waiting for " + WaitSecs + " Secs.");
                Thread.Sleep((int) (WaitSecs * 1000));
            }

            Console.WriteLine("All of Data pool items updated.");
        }

        public void Update(ItemIds[] itemNames)
        {
            Console.WriteLine("Updating " + itemNames.Length + " items.");
            var counter = 1;
            foreach (var itemName in itemNames)
            {
                Update(itemName);
                Console.WriteLine("[" + ++counter + "/" + itemNames.Length + "] Waiting for " + WaitSecs + " Secs.");
                Thread.Sleep((int) (WaitSecs * 1000));
            }

            Console.WriteLine("All of items added to pool.");
        }

        public void AddItem(ItemIds[] itemNames)
        {
            Console.WriteLine("adding " + itemNames.Length + " items.");
            var counter = 1;
            foreach (var itemName in itemNames)
            {
                AddItem(itemName);
                Console.WriteLine("[" + ++counter + "/" + itemNames.Length + "] Waiting for " + WaitSecs + " Secs.");
                Thread.Sleep((int) (WaitSecs * 1000));
            }

            Console.WriteLine("All of items added to pool.");
        }

        public void AddItem(ItemIds itemName)
        {
            var itemDetailsRequest = GetItemDetailsRequest(itemName);
            itemDetailsRequest.Wait();
            itemDataPool.Add(itemName, new ItemDetails[0]);
            itemDataPool[itemName] = itemDetailsRequest.Result;
            Console.WriteLine(itemName + " added to item data pool.");
        }

        public void Update(ItemIds itemName)
        {
            var itemDetailsRequest = GetItemDetailsRequest(itemName);
            itemDetailsRequest.Wait();
            itemDataPool[itemName] = itemDetailsRequest.Result;
            Console.WriteLine(itemName + " item updated at Data pool.");
        }

        public string[] GetCities()
        {
            var cityNames = new List<string>();
            var items = GetItems();
            if (itemDataPool.Count == 0 || items.Length == 0) return cityNames.ToArray();
            //choose item example to get cities that downloaded from albion online project
            var firstItem = itemDataPool[items[0]];
            cityNames.AddRange(firstItem.Select(itemAndCity => itemAndCity.city));
            return cityNames.ToArray();
        }

        private async Task<ItemDetails[]> GetItemDetailsRequest(ItemIds itemName)
        {
            Console.WriteLine("Getting " + itemName + " item data from albion online data project.");
            var resp = await client.GetAsync("https://www.albion-online-data.com/api/v2/stats/Prices/" +
                                             itemName.ToString().Replace("_AtSign_", "@"));
            resp.EnsureSuccessStatusCode();
            var itemDetailsRequest = JsonConvert
                .DeserializeObject<List<ItemDetails>>(await resp.Content.ReadAsStringAsync()).ToArray();
            Console.WriteLine(itemName + " item data Received from albion online data project.");
            return itemDetailsRequest;
        }

        public string GetJson()
        {
            return JsonConvert.SerializeObject(itemDataPool);
        }

        public void SetJson(string text)
        {
            itemDataPool = JsonConvert.DeserializeObject<Dictionary<ItemIds, ItemDetails[]>>(text);
        }
    }
}