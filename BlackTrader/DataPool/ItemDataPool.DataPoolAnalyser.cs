using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using BlackTrader.Items;

namespace BlackTrader.DataPool
{
    public partial class ItemDataPool
    {
        public static partial class DataPoolAnalyser
        {
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
                        BenefitCalcDifferenceOfCityPrices(itemInCity, itemInCity2, pairList, pairListStarred);
                    }

                    break;
                }

                pairList.Sort((pair, offerPair) => (pair.MinDistancePrice() - offerPair.MinDistancePrice()));
                pairListStarred.Sort((pair, offerPair) => (pair.MaxDistancePrice() - offerPair.MaxDistancePrice()));
                pairList.AddRange(pairListStarred);
                return pairList.ToArray();
            }

            private static void BenefitCalcDifferenceOfCityPrices(ItemDetails itemInCity, ItemDetails itemInCity2,
                ICollection<TradeOfferPair> pairList, ICollection<TradeOfferPair> pairListStarred)
            {
                //todo calculate tax
                //todo check number of items to sell and buy
                if (itemInCity.sell_price_max < itemInCity2.buy_price_max)
                    pairListStarred.Add(new TradeOfferPair(itemInCity, itemInCity2, true));
                else if (itemInCity.sell_price_min < itemInCity2.buy_price_min)
                    pairList.Add(new TradeOfferPair(itemInCity, itemInCity2, false));
            }
        }
    }
}