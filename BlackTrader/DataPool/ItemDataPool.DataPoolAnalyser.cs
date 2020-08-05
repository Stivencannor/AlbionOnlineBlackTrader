using System.Collections.Generic;
using BlackTrader.Items;

namespace BlackTrader.DataPool
{
    public partial class ItemDataPool
    {
        public static class DataPoolAnalyser
        {
            public static IEnumerable<TradeOfferPair> TradeOffersFromCity(ItemDataPool dataPool, string cityName)
            {
                cityName = cityName.ToLower();
                var pairList = new List<TradeOfferPair>();
                var itms = dataPool.GetItems();
                foreach (var id in itms)
                foreach (var itemInCity in dataPool.itemDataPool[id])
                {
                    if (itemInCity.sell_price_max == 0 || itemInCity.sell_price_min == 0) continue;
                    if (!itemInCity.city.ToLower().Equals(cityName)) continue;
                    foreach (var itemInCity2 in dataPool.itemDataPool[id])
                    {
                        if (itemInCity2.buy_price_max == 0 || itemInCity2.buy_price_min == 0) continue;
                        //todo calculate tax
                        //todo check number of items to sell and buy
                        if (itemInCity.sell_price_max < itemInCity2.buy_price_max)
                            pairList.Add(new TradeOfferPair(itemInCity, itemInCity2, true));
                        else if (itemInCity.sell_price_min < itemInCity2.buy_price_min)
                            pairList.Add(new TradeOfferPair(itemInCity, itemInCity2, false));
                    }

                    break;
                }

                return pairList.ToArray();
            }

            public static IEnumerable<TradeOfferPair> TradeOffersFromCity(ItemDataPool dataPool)
            {
                var pairList = new List<TradeOfferPair>();
                var itms = dataPool.GetItems();
                foreach (var id in itms)
                foreach (var itemInCity in dataPool.itemDataPool[id])
                {
                    if (itemInCity.sell_price_max == 0 || itemInCity.sell_price_min == 0) continue;
                    foreach (var itemInCity2 in dataPool.itemDataPool[id])
                    {
                        if (itemInCity2.buy_price_max == 0 || itemInCity2.buy_price_min == 0) continue;
                        //todo calculate tax
                        //todo check number of items to sell and buy
                        if (itemInCity.sell_price_max < itemInCity2.buy_price_max)
                            pairList.Add(new TradeOfferPair(itemInCity, itemInCity2, true));
                        else if (itemInCity.sell_price_min < itemInCity2.buy_price_min)
                            pairList.Add(new TradeOfferPair(itemInCity, itemInCity2, false));
                    }
                }

                return pairList.ToArray();
            }

            public struct TradeOfferPair
            {
                /// <summary>
                ///     buy from here
                /// </summary>
                private readonly ItemDetails fromThisItem;

                /// <summary>
                ///     sell to here
                /// </summary>
                private readonly ItemDetails toThisItem;

                /// <summary>
                ///     if its starred, its so valuable to trade and it is based on max price
                /// </summary>
                private readonly bool starred;

                public TradeOfferPair(ItemDetails fromThisItem, ItemDetails toThisItem, bool starred)
                {
                    this.fromThisItem = fromThisItem;
                    this.toThisItem = toThisItem;
                    this.starred = starred;
                }

                public override string ToString()
                {
                    return (starred ? "*" : "") + " you can buy " + fromThisItem.item_id + " from " +
                           fromThisItem.city + " for " +
                           (starred ? fromThisItem.sell_price_max : fromThisItem.sell_price_min) + " and sell to " +
                           toThisItem.city + " for " +
                           (starred ? toThisItem.buy_price_max : toThisItem.buy_price_min);
                }
            }
        }
    }
}