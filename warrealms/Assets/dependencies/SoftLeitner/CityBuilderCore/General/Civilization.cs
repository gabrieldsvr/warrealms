using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// object that defines parameters that influence how hard a mission is<br/>
    /// for additional parameters just derive from this object
    /// </summary>
    [CreateAssetMenu(menuName = "CityBuilder/" + nameof(Civilization))]
    public class Civilization : KeyedObject, ICivilizationFactor
    {
        [Tooltip("display name")] public string Name;

        [Tooltip("about civilization")] public string About;

        [Tooltip("exclusive construction")] public GameObject ExclusiveConstruction;

        public float GoldMiningTimeMutiplier = 1f;
        public float RockMiningTimeMutiplier = 1f;
        public float WoodExtractionTimeMutiplier = 1f;
        public float HarvestTimeMutiplier = 1f;
        public float BuildingTimeMutiplier = 1f;
        public float GoldExtractedAmountMutiplier = 1f;
        public float RockExtractedAmountMutiplier = 1f;
        public float WoodExtractedAmountMutiplier = 1f;
        public float FoodHarvestedAmountMutiplier = 1f;

        GameObject ICivilizationFactor.ExclusiveConstruction => ExclusiveConstruction;

        float ICivilizationFactor.GoldMiningTimeMutiplier => GoldMiningTimeMutiplier;
        float ICivilizationFactor.RockMiningTimeMutiplier => RockMiningTimeMutiplier;
        float ICivilizationFactor.WoodExtractionTimeMutiplier => WoodExtractionTimeMutiplier;
        float ICivilizationFactor.HarvestTimeMutiplier => HarvestTimeMutiplier;
        float ICivilizationFactor.GoldExtractedAmountMutiplier => GoldExtractedAmountMutiplier;
        float ICivilizationFactor.RockExtractedAmountMutiplier => RockExtractedAmountMutiplier;
        float ICivilizationFactor.WoodExtractedAmountMutiplier => WoodExtractedAmountMutiplier;
        float ICivilizationFactor.FoodHarvestedAmountMutiplier => FoodHarvestedAmountMutiplier;
        float ICivilizationFactor.BuildingTimeMutiplier => BuildingTimeMutiplier;
    }
}