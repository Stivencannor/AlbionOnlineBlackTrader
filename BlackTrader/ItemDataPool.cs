using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BlackTrader
{
    public class ItemDataPool
    {
        private readonly HttpClient client=new HttpClient();
        private readonly Dictionary<string, ItemDetails[]> itemDataPool=new Dictionary<string, ItemDetails[]>();
        private readonly List<string> cities=new List<string>();
        public string[] Cities => cities.ToArray();

        public ItemDetails GetItem(string itemName, string cityName)
        {
            foreach (var item in  itemDataPool[itemName])
                if(item.city.Equals(cityName))
                    return item;
            return default;
        }
        public async Task AddItem(string itemName)
        {
            itemDataPool.Add(itemName,new ItemDetails[0]);
            await Update(itemName);
        }
        public async Task Update()
        {
            foreach (var itemName in itemDataPool.Keys)
                await Update(itemName);
        }

        private async Task Update(string itemName)
        {
            var itemDetailsRequest = await GetItemDetailsRequest(itemName);
            itemDataPool[itemName] = itemDetailsRequest;
            foreach (var item in itemDetailsRequest)
                if (!cities.Any(city => item.city.Equals(city)))
                    cities.Add(item.city);
        }

        private async Task<ItemDetails[]> GetItemDetailsRequest(string itemName)
        {
            var resp = await client.GetAsync("https://www.albion-online-data.com/api/v2/stats/Prices/"+itemName);
            resp.EnsureSuccessStatusCode();
            return JsonConvert.DeserializeObject<List<ItemDetails>>(await resp.Content.ReadAsStringAsync()).ToArray();
        }
    }
}