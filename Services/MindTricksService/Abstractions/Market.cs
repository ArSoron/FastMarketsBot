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
        public Price LastPrice => RandomPrice(DateTimeOffset.UtcNow.AddDays(-1));
        [NotMapped]
        public Price STLM => RandomPrice(DateTimeOffset.UtcNow.AddDays(-1).AddMonths(-1));
        [NotMapped]
        public Price STLY => RandomPrice(DateTimeOffset.UtcNow.AddDays(-1).AddMonths(-12));

        public string ToDisplayValue() => $"/{NormalizedSymbol}\n{Product} in {Location}, {Currency} per {UnitOfMeasure}; {Incoterm}";
        public string ToLongDisplayValue() => $"/{NormalizedSymbol}\n{Description} in {Location}, {Currency} per {UnitOfMeasure}; {Incoterm}";

        private Price RandomPrice(DateTimeOffset assessmentDate)
        {
            var random = new Random();
            var mid = random.NextDouble() * 0.9 + 0.1; //0.1-1
            var deviation = random.NextDouble() * mid; //Scale down to avoid negative values
            return new Price()
            {
                Low = ToDecimalWithTwoDigits(mid * 100 - deviation * 10),
                Mid = ToDecimalWithTwoDigits(mid * 100),
                High = ToDecimalWithTwoDigits(mid * 100 + deviation * 10),
                AssessmentDate = assessmentDate,
                MidChangeSincePrevious = Math.Round(new decimal(random.NextDouble()-0.5), 2)
            };
        }
        private decimal ToDecimalWithTwoDigits(double d) => Math.Round(new decimal(d), 2);
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
