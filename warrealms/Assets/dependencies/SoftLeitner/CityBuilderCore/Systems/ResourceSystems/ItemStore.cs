using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// behaviour that does nothing except store items<br/>
    /// can be used in ItemStorages with <see cref="ItemStorageMode.Store"/> to combine storages<br/>
    /// for example this can be used to combine the storage across different components of the same building
    /// </summary>
    public class ItemStore : MonoBehaviour, IItemOwner
    {
        [Tooltip("stores items, can be referenced in other item storages with ItemStorageMode.Store to shift and combine storages")]
        public ItemStorage Storage;

        public IItemContainer ItemContainer => Storage;
    }
}
