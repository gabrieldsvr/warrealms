﻿using CityBuilderCore;
using System;
using UnityEngine;

namespace CityBuilderManual.Custom
{
    public class CustomRoamingWalker : BuildingComponentWalker<ICustomBuildingComponent>
    {
        private int _count;

        protected override void onComponentEntered(ICustomBuildingComponent buildingComponent)
        {
            base.onComponentEntered(buildingComponent);

            buildingComponent.DoSomething();
            _count++;
        }

        #region Saving
        [Serializable]
        public class CustomRoamingWalkerData : RoamingWalkerData
        {
            public int Count;
        }

        public override string SaveData()
        {
            return JsonUtility.ToJson(new CustomRoamingWalkerData()
            {
                WalkerData = savewalkerData(),
                State = (int)_state,
                Count = _count
            });
        }
        public override void LoadData(string json)
        {
            base.LoadData(json);

            var data = JsonUtility.FromJson<CustomRoamingWalkerData>(json);

            _count = data.Count;
        }
        #endregion
    }

    /// <summary>
    /// concrete implementation for serialization, not needed starting unity 2020.1
    /// </summary>
    [Serializable]
    public class ManualCustomRoamingWalkerSpawner : ManualWalkerSpawner<CustomRoamingWalker> { }
    /// <summary>
    /// concrete implementation for serialization, not needed starting unity 2020.1
    /// </summary>
    [Serializable]
    public class CyclicCustomRoamingWalkerSpawner : CyclicWalkerSpawner<CustomRoamingWalker> { }
    /// <summary>
    /// concrete implementation for serialization, not needed starting unity 2020.1
    /// </summary>
    [Serializable]
    public class PooledCustomRoamingWalkerSpawner : PooledWalkerSpawner<CustomRoamingWalker> { }
}