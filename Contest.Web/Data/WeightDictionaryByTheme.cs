using System;
using System.Collections.Generic;
using Contest.Enums;

namespace Contest.Data
{
    public static class WeightDictionaryByTheme
    {
        public static Dictionary<Enum, Dictionary<Enum, byte>> ThemeWeights => new Dictionary<Enum, Dictionary<Enum, byte>>
        {
            [Theme.OpenIdea] = new Dictionary<Enum, byte>
            {
                {JudgingCriteria.Innovation, 20},
                {JudgingCriteria.UsefulNess, 20},
                {JudgingCriteria.Quality, 20},
                {JudgingCriteria.ValueToCompany, 30},
                {JudgingCriteria.Presentation, 10}
            },
            [Theme.ArtificialIntelligence] = new Dictionary<Enum, byte>
            {
                {JudgingCriteria.Innovation, 20},
                {JudgingCriteria.UsefulNess, 20},
                {JudgingCriteria.Quality, 30},
                {JudgingCriteria.ValueToCompany, 20},
                {JudgingCriteria.Presentation, 10}
            },
            [Theme.DataVisualizationAndReporting] = new Dictionary<Enum, byte>
            {
                {JudgingCriteria.Innovation, 20},
                {JudgingCriteria.UsefulNess, 35},
                {JudgingCriteria.Quality, 20},
                {JudgingCriteria.ValueToCompany, 15},
                {JudgingCriteria.Presentation, 10}
            },
            [Theme.Security] = new Dictionary<Enum, byte>
            {
                {JudgingCriteria.Innovation, 25},
                {JudgingCriteria.UsefulNess, 15},
                {JudgingCriteria.Quality, 20},
                {JudgingCriteria.ValueToCompany, 30},
                {JudgingCriteria.Presentation, 10}
            }
        };
    }
}
