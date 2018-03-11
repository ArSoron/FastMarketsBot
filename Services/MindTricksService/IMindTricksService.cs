using FastMarkets.MindTricksService.Abstractions;
using System.Collections.Generic;

namespace FastMarkets.MindTricksService
{
    public interface IMindTricksService
    {
        IEnumerable<Symbol> GetFavourites();
        IEnumerable<Symbol> Search(string searchString);
        Symbol GetSymbol(string symbolId);
        bool IsValidSymbol(string symbol);
    }
}
