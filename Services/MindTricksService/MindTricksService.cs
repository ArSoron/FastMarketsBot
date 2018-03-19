using FastMarkets.MindTricksService.Abstractions;
using FastMarkets.MindTricksService.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastMarkets.MindTricksService
{
    public class MindTricksService : IMindTricksService
    {
        private readonly IQueryable<Market> _symbolsRepo;

        public MindTricksService (MySqlMarkets mySqlMarkets)
        {
            _symbolsRepo = mySqlMarkets.GetMarkets();
        }

        public IEnumerable<Market> GetFavourites()
        {
            return _symbolsRepo.Take(3);
        }

        public Market GetMarket(string symbol)
        {
            return _symbolsRepo.FirstOrDefault(market => market.NormalizedSymbol == symbol);
        }

        public bool IsValidSymbol(string symbol)
        {
            return _symbolsRepo.Any(market => market.NormalizedSymbol == symbol);
        }

        public IEnumerable<Market> Search(params string[] searchString)
        {
            var matching = _symbolsRepo.Where(market => market.NormalizedSymbol.Contains(searchString.First()))
                .Take(5);
            if (matching.Any())
            {
                return matching;
            }
            return 
                //_symbolsRepo.OrderByDescending(market => market.Product.LongestCommonSubsequence(false,searchString).Item2)
                //.Union(
                _symbolsRepo
                .OrderByDescending(market => market.Description.LongestCommonSubsequence(false, searchString).Item2)
                //)
                .Take(5);
        }
    }
}
