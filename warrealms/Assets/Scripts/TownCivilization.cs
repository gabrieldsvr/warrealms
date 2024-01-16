using CityBuilderCore;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// inherited version of <see cref="Civilization"/> with additional fields specific to the Town demo
    /// </summary>
    [CreateAssetMenu(menuName = "CityBuilder/Town/" + nameof(TownCivilization))]
    public class TownCivilization : Civilization
    {
        public TownWalker DefaultWalker;
        public ManualTownWalkerSpawner ManualTownDefaultWalker;
    }
}