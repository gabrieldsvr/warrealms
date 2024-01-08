using CityBuilderCore;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// some collection of tasks<br/>
    /// a set of all taks in the game is needed by <see cref="TownManager"/> so tasks can be found when a game gets loaded
    /// </summary>
    [CreateAssetMenu(menuName = "CityBuilder/Town/" + nameof(TownTaskSet))]
    public class TownTaskSet : KeyedSet<TownTask> { }
}
