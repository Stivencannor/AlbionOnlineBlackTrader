using System;
using BlackTrader.Items;

namespace BlackTrader.DataPool
{
    public partial class ItemDataPool
    {
        public static partial class DataPoolAnalyser
        {
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

                public int MaxDistancePrice()
                {
                    return Math.Abs(toThisItem.buy_price_max - fromThisItem.sell_price_max);
                }

                public int MinDistancePrice()
                {
                    return Math.Abs(toThisItem.buy_price_min - fromThisItem.sell_price_min);
                }

                public override string ToString()
                {
                    return (starred ? "*" : "") + " you can buy " + fromThisItem.item_id + " from " +
                           fromThisItem.city + " for " +
                           (starred ? fromThisItem.sell_price_max : fromThisItem.sell_price_min) + " and sell to " +
                           toThisItem.city + " for " +
                           (starred ? toThisItem.buy_price_max : toThisItem.buy_price_min) + " then profit over :" +
                           (starred ? MaxDistancePrice() : MinDistancePrice());
                }
            }
        }
    }
}