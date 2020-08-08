using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BlackTrader.Config;
using BlackTrader.Items;

namespace BlackTrader.DataPool
{
    public partial class ItemDataPool
    {
        public static partial class DataPoolAnalyser
        {
            /// <summary>
            /// </summary>
            /// <param name="dataPool"></param>
            /// <param name="item"></param>
            /// <returns>city names</returns>
            public static IEnumerable<ItemDetails> WhereSell(ItemDataPool dataPool, ItemIds item)
            {
                if (!dataPool.itemDataPool.ContainsKey(item))
                {
                    Console.WriteLine("This Item Not exist at data pool.");
                    return new ItemDetails[0];
                }

                var itemAndCities = new List<ItemDetails>(dataPool.itemDataPool[item]);
                itemAndCities.RemoveAll(details => details.buy_price_max == 0);
                var removeAll =
                    itemAndCities.RemoveAll(details => details.MaxBuyDateDiff.TotalHours > Configs.UpdateDateHoursLimit);
                Console.WriteLine("Removed " + removeAll + " Old Date Items. (" + Configs.UpdateDateHoursLimit + " Hours)");
                itemAndCities.Sort((details, itemDetails) => details.buy_price_max - itemDetails.buy_price_max);
                return itemAndCities.ToArray();
            }

            public static IEnumerable<ItemDetails> WhereBuy(ItemDataPool dataPool, ItemIds item)
            {
                if (!dataPool.itemDataPool.ContainsKey(item))
                {
                    Console.WriteLine("This Item Not exist at data pool.");
                    return new ItemDetails[0];
                }

                var itemAndCities = new List<ItemDetails>(dataPool.itemDataPool[item]);
                itemAndCities.RemoveAll(details => details.sell_price_min == 0);
                var removeAll =
                    itemAndCities.RemoveAll(details => details.MinSellDateDiff.TotalHours > Configs.UpdateDateHoursLimit);
                Console.WriteLine("Removed " + removeAll + " Old Date Items. (" + Configs.UpdateDateHoursLimit + " Hours)");
                itemAndCities.Sort((details, itemDetails) => itemDetails.sell_price_min - details.sell_price_min);
                return itemAndCities.ToArray();
            }

            public static IEnumerable<TradeOfferPair> TradeOffersFromCity(ItemDataPool dataPool,
                [Optional] string fromCityName,
                [Optional] string toCityName)
            {
                if (!string.IsNullOrWhiteSpace(fromCityName))
                    fromCityName = fromCityName.ToLower();
                if (!string.IsNullOrWhiteSpace(toCityName))
                    toCityName = toCityName.ToLower();
                var pairList = new List<TradeOfferPair>();
                var pairListStarred = new List<TradeOfferPair>();
                var itms = dataPool.GetItems();
                foreach (var id in itms)
                foreach (var itemInCity in dataPool.itemDataPool[id])
                {
                    if (!string.IsNullOrWhiteSpace(fromCityName) &&
                        !itemInCity.city.Replace(" ", "").ToLower().Equals(fromCityName)) continue;
                    if (itemInCity.sell_price_max == 0 || itemInCity.sell_price_min == 0) continue;
                    foreach (var itemInCity2 in dataPool.itemDataPool[id])
                    {
                        if (!string.IsNullOrWhiteSpace(toCityName) &&
                            !itemInCity2.city.Replace(" ", "").ToLower().Equals(toCityName)) continue;
                        if (itemInCity2.buy_price_max == 0 || itemInCity2.buy_price_min == 0) continue;
                        //todo calculate tax
                        //todo check number of items to sell and buy
                        if (itemInCity.sell_price_max < itemInCity2.buy_price_max)
                            pairListStarred.Add(new TradeOfferPair(itemInCity, itemInCity2, true));
                        else if (itemInCity.sell_price_min < itemInCity2.buy_price_min)
                            pairList.Add(new TradeOfferPair(itemInCity, itemInCity2, false));
                    }

                    break;
                }

                var removeAll = pairList.RemoveAll(details =>
                    details.FromThisItem.MinSellDateDiff.TotalHours > Configs.UpdateDateHoursLimit ||
                    details.ToThisItem.MinBuyDateDiff.TotalHours > Configs.UpdateDateHoursLimit);
                removeAll += pairListStarred.RemoveAll(details =>
                    details.FromThisItem.MaxSellDateDiff.TotalHours > Configs.UpdateDateHoursLimit ||
                    details.ToThisItem.MaxBuyDateDiff.TotalHours > Configs.UpdateDateHoursLimit);
                Console.WriteLine("Removed " + removeAll + " Old Date Items. (" + Configs.UpdateDateHoursLimit + " Hours)");
                pairList.Sort((pair, offerPair) => pair.MinDistancePrice() - offerPair.MinDistancePrice());
                pairListStarred.Sort((pair, offerPair) => pair.MaxDistancePrice() - offerPair.MaxDistancePrice());
                //todo sort config by Hours,Benefit and more
                pairList.AddRange(pairListStarred);
                return pairList.ToArray();
            }
        }
    }
}