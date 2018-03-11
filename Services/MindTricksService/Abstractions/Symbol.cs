using System;

namespace FastMarkets.MindTricksService.Abstractions
{
    public class Symbol
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public Price LastPrice { get; set; }
        public Price STLM { get; set; }
        public Price STLY { get; set; }
    }
    public class Price
    {
        public decimal High { get; set; }
        public decimal Mid { get; set; }
        public decimal Low { get; set; }
        public DateTimeOffset AssessmentDate { get; set; }
        public decimal MidChangeSincePrevious { get; set; }
    }
}
