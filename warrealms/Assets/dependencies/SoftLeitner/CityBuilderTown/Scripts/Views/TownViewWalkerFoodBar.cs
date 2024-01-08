using CityBuilderCore;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// view that visualizes a walkers current food in percent
    /// </summary>
    [CreateAssetMenu(menuName = "CityBuilder/Town/" + nameof(TownViewWalkerFoodBar))]
    public class TownViewWalkerFoodBar : ViewWalkerBarBase, IWalkerValue
    {
        public override IWalkerValue WalkerValue => this;

        public bool HasValue(Walker walker) => walker is TownWalker;
        public float GetMaximum(Walker walker) => 100;//((TownWalker)walker).Identity.FoodCapacity;
        public float GetValue(Walker walker) => ((TownWalker)walker).Food / ((TownWalker)walker).Identity.FoodCapacity * 100f;//((TownWalker)walker).Food;
        public Vector3 GetPosition(Walker walker) => walker.Pivot.position;
    }
}