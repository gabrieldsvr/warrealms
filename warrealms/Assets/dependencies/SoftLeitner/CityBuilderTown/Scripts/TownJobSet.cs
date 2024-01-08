using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// some collection of jobs<br/>
    /// a set of all jobs in the game is needed by <see cref="TownManager"/>
    /// </summary>
    [CreateAssetMenu(menuName = "CityBuilder/Town/" + nameof(TownJobSet))]
    public class TownJobSet : KeyedSet<TownJob> { }
}
