using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlackTrader.Config;
using BlackTrader.Items;
using Newtonsoft.Json;

namespace BlackTrader.DataPool
{
    public partial class ItemDataPool
    {
        public ItemDataPool()
        {
            Load(Configs.NameOfDataPoolAutoSaveFile);
        }

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

        public void Remove()
        {
            itemDataPool.Clear();
            Save(Configs.NameOfDataPoolAutoSaveFile);
        }
        public void Remove(ItemIds itemName)
        {
            itemDataPool.Remove(itemName);
            Save(Configs.NameOfDataPoolAutoSaveFile);
        }

        public void Update()
        {
            Update(GetItems());
        }

        public void Update(params ItemIds[] itemNames)
        {
            Console.WriteLine("Updating " + itemNames.Length + " items.");
            var itemDetailsRequest = GetItemDetailsRequest(itemNames);
            itemDetailsRequest.Wait();
            foreach (var key in itemDetailsRequest.Result.Keys)
                itemDataPool[key] = itemDetailsRequest.Result[key];
            Console.WriteLine("All of " + itemNames.Length + " items updated.");
            Save(Configs.NameOfDataPoolAutoSaveFile);
        }

        public void Add(params ItemIds[] itemNames)
        {
            Console.WriteLine("Adding " + itemNames.Length + " items.");
            var itemDetailsRequest = GetItemDetailsRequest(itemNames);
            itemDetailsRequest.Wait();
            foreach (var key in itemDetailsRequest.Result.Keys)
                itemDataPool.Add(key, itemDetailsRequest.Result[key]);
            Console.WriteLine("All of " + itemNames.Length + " items added to pool.");
            Save(Configs.NameOfDataPoolAutoSaveFile);
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

        private const int LimitItemsForWebRequest = 100;

        private async Task<Dictionary<ItemIds, ItemDetails[]>> GetItemDetailsRequest(params ItemIds[] itemNames)
        {
            return await GetItemsDetailsRequest(itemNames);
        }

        private async Task<Dictionary<ItemIds, ItemDetails[]>> GetItemsDetailsRequest(
            IReadOnlyCollection<ItemIds> itemNames)
        {
            var namesList = itemNames.ToList();
            Console.WriteLine("Getting " + itemNames.Count + " items data from albion online data project.");
            var globalList = new Dictionary<ItemIds, ItemDetails[]>();
            //dedicate itemNames to limit number
            for (var index = 0; index < itemNames.Count; index += LimitItemsForWebRequest)
            {
                var toCount = Math.Min(LimitItemsForWebRequest, itemNames.Count - index);
                var stringOfItemsList = namesList
                    .GetRange(index, toCount).Aggregate("",
                        (current, itm) => current + (itm.ToString().Replace("_AtSign_", "@") + ",")).Trim(',');
                var resp = await client.GetAsync("https://www.albion-online-data.com/api/v2/stats/Prices/" +
                                                 stringOfItemsList);
                resp.EnsureSuccessStatusCode();
                var result = JsonConvert.DeserializeObject<List<ItemDetails>>(await resp.Content.ReadAsStringAsync());
                foreach (var itm in result.Where(itm => !globalList.ContainsKey(itm.ItemName)))
                    globalList.Add(itm.ItemName,
                        result.FindAll(details => details.item_id.Equals(itm.item_id)).ToArray());
                Console.WriteLine("Received " + (index + toCount) + "/" + itemNames.Count + " Items.");
            }

            Console.WriteLine(itemNames.Count + " items data Received from albion online data project.");
            return globalList;
        }

        public void Save(string fileName)
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName + ".json"),
                JsonConvert.SerializeObject(itemDataPool));
        }

        public bool Load(string fileName)
        {
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), fileName + ".json"))) return false;
            itemDataPool = JsonConvert.DeserializeObject<Dictionary<ItemIds, ItemDetails[]>>(
                File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), fileName + ".json")));
            return true;
        }
    }
}