using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BlackTrader
{
    public class ItemDataPool
    {
        private readonly List<string> cities = new List<string>();
        private readonly HttpClient client = new HttpClient();
        private readonly Dictionary<ItemIds, ItemDetails[]> itemDataPool = new Dictionary<ItemIds, ItemDetails[]>();
        public string[] Cities => cities.ToArray();

        public ItemDetails GetItem(ItemIds itemName, string cityName)
        {
            foreach (var item in itemDataPool[itemName])
                if (item.city.Equals(cityName))
                    return item;
            return new ItemDetails();
        }

        public ItemIds[] GetItems()
        {
            return itemDataPool.Keys.ToArray();
        }

        public void RemoveItem(ItemIds itemName)
        {
            itemDataPool.Remove(itemName);
        }

        public async Task AddItem(ItemIds itemName)
        {
            itemDataPool.Add(itemName, new ItemDetails[0]);
            await Update(itemName);
            Console.WriteLine(itemName + " added to item data pool.");
        }

        public async Task Update()
        {
            foreach (var itemName in itemDataPool.Keys)
                await Update(itemName);
            Console.WriteLine("All of Data pool items updated.");
        }

        public async Task Update(ItemIds itemName)
        {
            var itemDetailsRequest = await GetItemDetailsRequest(itemName);
            itemDataPool[itemName] = itemDetailsRequest;
            foreach (var item in itemDetailsRequest)
                if (!cities.Any(city => item.city.Equals(city)))
                    cities.Add(item.city);
            Console.WriteLine(itemName + " item updated at Data pool.");
        }

        private async Task<ItemDetails[]> GetItemDetailsRequest(ItemIds itemName)
        {
            Console.WriteLine("getting" + itemName + " item data from albion online data project.");
            var resp = await client.GetAsync("https://www.albion-online-data.com/api/v2/stats/Prices/" +
                                             itemName.ToString().Replace("_AtSign_", "@"));
            resp.EnsureSuccessStatusCode();
            Console.WriteLine(itemName + " item data Received from albion online data project.");
            return JsonConvert.DeserializeObject<List<ItemDetails>>(await resp.Content.ReadAsStringAsync()).ToArray();
        }
    }
}