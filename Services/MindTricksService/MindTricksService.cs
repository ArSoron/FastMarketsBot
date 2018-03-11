using FastMarkets.MindTricksService.Abstractions;
using FastMarkets.MindTricksService.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastMarkets.MindTricksService
{
    public class MindTricksService : IMindTricksService
    {
        private static IQueryable<Symbol> symbolsRepo = MockSymbols.GetSymbols();

        public IEnumerable<Symbol> GetFavourites()
        {
            return symbolsRepo.Take(3);
        }

        public Symbol GetSymbol(string symbolId)
        {
            return symbolsRepo.FirstOrDefault(symbol => symbol.Id == symbolId);
        }

        public bool IsValidSymbol(string symbolId)
        {
            return symbolsRepo.Any(symbol => symbol.Id == symbolId);
        }

        public IEnumerable<Symbol> Search(string searchString)
        {
            var matching = symbolsRepo.Where(symbol => symbol.Id.Contains(searchString));
            if (matching.Any())
            {
                return matching;
            }
            return symbolsRepo
                .OrderByDescending(symbol => symbol.Description.LongestCommonSubsequence(searchString, false).Item2)
                .Take(5);
        }
    }
}
