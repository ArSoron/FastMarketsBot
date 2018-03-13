using FastMarkets.MindTricksService.Abstractions;
using System.Collections.Generic;

namespace FastMarkets.MindTricksService
{
    public interface IMindTricksService
    {
        IEnumerable<Market> GetFavourites();
        IEnumerable<Market> Search(string searchString);
        Market GetMarket(string symbol);
        bool IsValidSymbol(string symbol);
    }
}
