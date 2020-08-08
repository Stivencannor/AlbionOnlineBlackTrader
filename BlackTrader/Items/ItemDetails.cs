using System;
using System.Globalization;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable InconsistentNaming

namespace BlackTrader.Items
{
    public struct ItemDetails
    {
        public ItemIds ItemName
        {
            get
            {
                Enum.TryParse<ItemIds>(item_id.Replace("@","_AtSign_"), out var enm);
                return enm;
            }
        }

        public TimeSpan MaxSellDateDiff => DateTime.Now.Subtract(sell_price_max_date);
        public TimeSpan MinSellDateDiff => DateTime.Now.Subtract(sell_price_min_date);
        public TimeSpan MaxBuyDateDiff => DateTime.Now.Subtract(buy_price_max_date);
        public TimeSpan MinBuyDateDiff => DateTime.Now.Subtract(buy_price_min_date);

        public string MaxSellDateDiffString => TimeSpanDiffToString(MaxSellDateDiff);
        public string MinSellDateDiffString => TimeSpanDiffToString(MinSellDateDiff);

        public string MaxBuyDateDiffString => TimeSpanDiffToString(MaxBuyDateDiff);
        public string MinBuyDateDiffString => TimeSpanDiffToString(MinBuyDateDiff);

        private static string TimeSpanDiffToString(TimeSpan timeSpan)
        {
            if (timeSpan.Days > 0)
                return timeSpan.Days + " Days Ago!";
            if (timeSpan.Hours > 0)
                return timeSpan.Hours + " Hours Ago!";
            if (timeSpan.Minutes > 0)
                return timeSpan.Minutes + " Minutes Ago!";
            if (timeSpan.Seconds > 0)
                return timeSpan.Seconds + " Seconds Ago!";
            return timeSpan.ToString();
        }

        public string item_id { get; set; }
        public string city { get; set; }
        public int quality { get; set; }
        public int sell_price_min { get; set; }
        public DateTime sell_price_min_date { get; set; }
        public int sell_price_max { get; set; }
        public DateTime sell_price_max_date { get; set; }
        public int buy_price_min { get; set; }
        public DateTime buy_price_min_date { get; set; }
        public int buy_price_max { get; set; }
        public DateTime buy_price_max_date { get; set; }
    }
}