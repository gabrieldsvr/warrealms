﻿using CityBuilderCore;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// after a walker goes to the task point and spend some time working the task is finished and the walker receives some items<br/>
    /// the point is also removed from a source structure and may be added to a destination structure<br/>
    /// for example when gathering berries the point is removed from the berries structure and added to berries empty
    /// </summary>
    public class TownGatherTask : TownTask
    {
        [Tooltip("time it takes the walker to finish the task once it has arrived at the point")]
        public float Duration;
        [Tooltip("items added to the walker when the task is finished")]
        public ItemQuantity Items;
        [Tooltip("when the task finishes the point is removed from this structure(for example removing full berry bushes)")]
        public string SourceStructure;
        [Tooltip("optional, when the task finishes the point is added to this structure(for example removing adding empty bushes)")]
        public string DestinationStructure;

        public override IEnumerable<TownWalker> Walkers
        {
            get
            {
                if (_walker != null)
                    yield return _walker;
            }
        }

        private TownWalker _walker;
        private float _work;

        public override bool CanStartTask(TownWalker walker)
        {
            return _walker == null && !walker.ItemStorage.HasItems();
        }
        public override WalkerAction[] StartTask(TownWalker walker)
        {
            _walker = walker;

            return new WalkerAction[]
            {
                new WalkPointAction(){ _point = Point},
                new TownProgressAction(string.Empty,TownManager.WorkParameter)
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

            var structureManager = Dependencies.Get<IStructureManager>();

            structureManager.GetStructure(SourceStructure).Remove(Point);
            if (!string.IsNullOrWhiteSpace(DestinationStructure))
                structureManager.GetStructure(DestinationStructure).Add(Point);
        }

        public override bool ProgressTask(TownWalker walker, string key)
        {
            walker.Work();
            _work += Time.deltaTime;

            return _work < Duration;
        }

        public override string GetDescription() => $"gathering {Items.Item.Name}";
        public override string GetDebugText()
        {
            return Items.ToString() + " " + (Duration - _work);
        }

        #region Saving
        protected override string saveExtras() => _work.ToString(System.Globalization.CultureInfo.InvariantCulture);
        protected override void loadExtras(string json) => _work = float.Parse(json, System.Globalization.CultureInfo.InvariantCulture);
        #endregion
    }
}