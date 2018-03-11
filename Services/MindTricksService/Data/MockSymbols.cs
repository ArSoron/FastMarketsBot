using FastMarkets.MindTricksService.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FastMarkets.MindTricksService.Data
{
    public static class MockSymbols
    {
        public static IQueryable<Symbol> GetSymbols()
        {
            return new List<Symbol> {
                new Symbol{Id="MB_AL_0001", Description = "LME duty-paid premium indicator HG cash",
                    LastPrice = new Price{
                        Low = 1.0M,
                        Mid = 2.0M,
                        High = 3.0M,
                        MidChangeSincePrevious = 0.4M,
                        AssessmentDate = new DateTimeOffset(2018,03,09,0,0,0, TimeSpan.Zero)
                    },
                    STLM = new Price{
                        Low = 1.1M,
                        Mid = 2.2M,
                        High = 3.3M,
                        MidChangeSincePrevious = 0.3M,
                        AssessmentDate = new DateTimeOffset(2018,02,09,0,0,0, TimeSpan.Zero)
                    },
                    STLY = new Price{
                        Low = 1.5M,
                        Mid = 2.6M,
                        High = 3.7M,
                        MidChangeSincePrevious = 0.2M,
                        AssessmentDate = new DateTimeOffset(2017,03,10,0,0,0, TimeSpan.Zero)
                    }
                },
                new Symbol{Id="MB_AL_0003", Description = "HG duty-paid 3 months",
                    LastPrice = new Price{
                        Low = 5.0M,
                        Mid = 6.0M,
                        High = 7.0M,
                        MidChangeSincePrevious = -0.2M,
                        AssessmentDate = new DateTimeOffset(2018,03,09,0,0,0, TimeSpan.Zero)
                    },
                    STLM = new Price{
                        Low = 5.5M,
                        Mid = 6.1M,
                        High = 6.7M,
                        MidChangeSincePrevious = 0.2M,
                        AssessmentDate = new DateTimeOffset(2018,02,09,0,0,0, TimeSpan.Zero)
                    },
                    STLY = new Price{
                        Low = 5.1M,
                        Mid = 6.2M,
                        High = 7.4M,
                        MidChangeSincePrevious = 0.1M,
                        AssessmentDate = new DateTimeOffset(2017,03,10,0,0,0, TimeSpan.Zero)
                    }
                },
                new Symbol{Id="MB_AL_0004", Description = "Cif Japan 99.7% duty unpaid premium indicator quarterly",
                    LastPrice = new Price{
                        Low = 3.0M,
                        Mid = 5.0M,
                        High = 7.0M,
                        MidChangeSincePrevious = 0.2M,
                        AssessmentDate = new DateTimeOffset(2018,03,09,0,0,0, TimeSpan.Zero)
                    },
                    STLM = new Price{
                        Low = 3.1M,
                        Mid = 5.2M,
                        High = 7.3M,
                        MidChangeSincePrevious = 0.2M,
                        AssessmentDate = new DateTimeOffset(2018,02,09,0,0,0, TimeSpan.Zero)
                    },
                    STLY = new Price{
                        Low = 3.1M,
                        Mid = 5.1M,
                        High = 7.1M,
                        MidChangeSincePrevious = 0.1M,
                        AssessmentDate = new DateTimeOffset(2017,03,10,0,0,0, TimeSpan.Zero)
                    }
                },
                new Symbol{ Id = "MB_STE_0004", Description="Steel Northern Europe imports € per tonne cfr main EU port Rebar",
                    LastPrice = new Price{
                        Low = 10.0M,
                        Mid = 20.0M,
                        High = 30.0M,
                        MidChangeSincePrevious = -0.3M,
                        AssessmentDate = new DateTimeOffset(2018,03,09,0,0,0, TimeSpan.Zero)
                    },
                    STLM = new Price{
                        Low = 15.5M,
                        Mid = 25.1M,
                        High = 30.7M,
                        MidChangeSincePrevious = -0.5M,
                        AssessmentDate = new DateTimeOffset(2018,02,09,0,0,0, TimeSpan.Zero)
                    },
                    STLY = new Price{
                        Low = 20.1M,
                        Mid = 30.2M,
                        High = 40.4M,
                        MidChangeSincePrevious = -0.7M,
                        AssessmentDate = new DateTimeOffset(2017,03,10,0,0,0, TimeSpan.Zero)
                    }
                }
            }.AsQueryable();
        }
    }
}
