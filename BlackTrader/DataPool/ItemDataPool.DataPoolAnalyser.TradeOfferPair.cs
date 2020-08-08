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
                public readonly ItemDetails FromThisItem;

                /// <summary>
                ///     sell to here
                /// </summary>
                public readonly ItemDetails ToThisItem;

                /// <summary>
                ///     if its starred, its so valuable to trade and it is based on max price
                /// </summary>
                private readonly bool starred;

                public TradeOfferPair(ItemDetails fromThisItem, ItemDetails toThisItem, bool starred)
                {
                    this.FromThisItem = fromThisItem;
                    this.ToThisItem = toThisItem;
                    this.starred = starred;
                }

                public int MaxDistancePrice()
                {
                    return Math.Abs(ToThisItem.buy_price_max - FromThisItem.sell_price_max);
                }

                public int MinDistancePrice()
                {
                    return Math.Abs(ToThisItem.buy_price_min - FromThisItem.sell_price_min);
                }

                public override string ToString()
                {
                    return (starred ? "*" : "") + " you can buy " + FromThisItem.item_id + " from " +
                           FromThisItem.city + " for " +
                           (starred ? FromThisItem.sell_price_max : FromThisItem.sell_price_min) + " ("+(starred ? FromThisItem.MaxSellDateDiffString : FromThisItem.MinSellDateDiffString)+") and sell to " +
                           ToThisItem.city + " for " +
                           (starred ? ToThisItem.buy_price_max : ToThisItem.buy_price_min) + "  ("+(starred ? ToThisItem.MaxBuyDateDiffString : ToThisItem.MinBuyDateDiffString)+") ,then profit over :" +
                           (starred ? MaxDistancePrice() : MinDistancePrice());
                }
            }
        }
    }
}