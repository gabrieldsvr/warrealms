using System.Collections.Generic;

namespace CityBuilderCore
{
    /// <summary>
    /// building component that produces items from other items
    /// </summary>
    public interface IPeriodicProductionComponent : IProgressComponent, IItemOwner
    {
        
        /// <summary>
        /// exposes the items the component can produce
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItemLevel> GetProducedItems();
    }
}