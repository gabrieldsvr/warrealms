using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// something influencing difficulty, multiple factors are multiplied
    /// </summary>
    public interface ICivilizationFactor
    {
        // Tempos de extração/mineração
        public float GoldMiningTimeMutiplier { get; }
        public float RockMiningTimeMutiplier { get; }
        public float WoodExtractionTimeMutiplier { get; }
        public float HarvestTimeMutiplier { get; }
        public float BuildingTimeMutiplier { get; }

        public float GoldExtractedAmountMutiplier { get; }
        public float RockExtractedAmountMutiplier { get; }
        public float WoodExtractedAmountMutiplier { get; }
        public float FoodHarvestedAmountMutiplier { get; }
        GameObject ExclusiveConstruction { get; }
    }
}