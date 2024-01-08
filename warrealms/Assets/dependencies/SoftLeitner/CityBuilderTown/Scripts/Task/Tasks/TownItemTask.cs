using CityBuilderCore;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// items lying on the map ready to be collected and stored<br/>
    /// a walker has to go to the task and spend a little bit of time picking it up<br/>
    /// after that the items are added to the walkers storage and the task is finished<br/>
    /// storing the item is the walkers business and not part of the task
    /// </summary>
    public class TownItemTask : TownTask
    {
        [Tooltip("how long it takes a walker to pick up the items")]
        public float CollectDuration;
        [Tooltip("items that are added to the walker when it finishes the task")]
        public ItemQuantity Items;

        public override IEnumerable<TownWalker> Walkers
        {
            get
            {
                if (_walker != null)
                    yield return _walker;
            }
        }

        private TownWalker _walker;

        public override bool CanStartTask(TownWalker walker)
        {
            return _walker == null && !walker.ItemStorage.HasItems();
        }
        public override WalkerAction[] StartTask(TownWalker walker)
        {
            _walker = walker;

            return new WalkerAction[]
            {
                new WalkPointAction(Point),
                new WaitAnimatedAction(CollectDuration,TownManager.WorkParameter)
            };
        }
        public override void ContinueTask(TownWalker walker)
        {
            _walker = walker;
        }
        public override void FinishTask(TownWalker walker, ProcessState process)
        {
            _walker = null;

            if (process.IsCanceled)
                return;

            Terminate();

            walker.Storage.AddItems(Items.Item, Items.Quantity);
        }

        public override string GetDescription() => $"picking up {Items.Item.Name}";
        public override string GetDebugText()
        {
            return Items.ToString();
        }
    }
}
