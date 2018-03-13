using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastMarkets.MindTricksService.Abstractions
{
    public class Market
    {
        [Key]
        public string Symbol { get; set; }
        [NotMapped]
        public string NormalizedSymbol => Symbol.Replace('-', '_');
        public string Product { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Currency { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Incoterm { get; set; }
        [NotMapped]
        public Price LastPrice => RandomPrice(DateTimeOffset.UtcNow);
        [NotMapped]
        public Price STLM => RandomPrice(DateTimeOffset.UtcNow.AddMonths(-1));
        [NotMapped]
        public Price STLY => RandomPrice(DateTimeOffset.UtcNow.AddMonths(-12));

        public string ToDisplayValue() => $"/{NormalizedSymbol}\n{Product}";

        private Price RandomPrice(DateTimeOffset assessmentDate) {
            var random = new Random();
            decimal mid = Math.Round(new decimal(random.NextDouble() * 100),1);
            decimal deviation = Math.Round(new decimal(random.NextDouble() * 10),2);
            return new Price()
            {
                Low = mid - deviation,
                Mid = mid,
                High = mid + deviation,
                AssessmentDate = assessmentDate,
                MidChangeSincePrevious = Math.Round(new decimal(random.NextDouble()),2)
            };
        }
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
