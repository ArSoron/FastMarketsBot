using FastMarkets.MindTricksService.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastMarkets.MindTricksService.Data
{
    public static class MockMarkets
    {
        public static IQueryable<Market> GetMarkets()
        {
            return new List<Market> {
                new Market{Symbol="MB_AL_0001", Description = "LME duty-paid premium indicator HG cash",
                                    },
                new Market{Symbol="MB_AL_0003", Description = "HG duty-paid 3 months",
                    
                },
                new Market{Symbol="MB_AL_0004", Description = "Cif Japan 99.7% duty unpaid premium indicator quarterly",
                    
                },
                new Market{ Symbol = "MB_STE_0004", Description="Steel Northern Europe imports € per tonne cfr main EU port Rebar",
                    
                }
            }.AsQueryable();
        }
    }
}
