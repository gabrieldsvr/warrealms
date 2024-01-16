using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CityBuilderCore
{
    public class PeriodicProductionComponent : ProgressComponent, IPeriodicProductionComponent
    {
        public override string Key => "PPRD";

        public enum ProductionState
        {
            Idle = 0, //waiting for raw materials in consumers
            Working = 10, //progress going up according to efficiency
            Done = 20 //waiting for producers to deposit goods
        }

        [Tooltip("one for each item that is produced")]
        public ItemProducer[] ItemsProducers;

        [Tooltip("fired whenever items change, parameter is whether raw materials are in storage")]
        public BoolEvent HasRawMaterialsChanged;

        [Tooltip("fired whenever items change, parameter is whether any products are in storage")]
        public BoolEvent HasProductsChanged;

        public int Priority => 1000;
        public ItemProducer[] Producers => ItemsProducers;

        public BuildingComponentReference<IItemReceiver> Reference { get; set; }
        public IItemContainer ItemContainer { get; private set; }

        public virtual bool HasProducts => ItemsProducers.Any(p => p.HasItem);
        public ProductionState CurrentProductionState => _productionState;

        protected bool _isProgressing;
        protected ProductionState _productionState;

        private ItemStorage OwnStorage;

        protected virtual void Start()
        {
            OwnStorage = this.gameObject.GetComponent<StorageComponent>().Storage;
            onItemsChanged();
        }

        protected virtual void Update()
        {
            updateProduction();
        }


        public IEnumerable<ItemLevel> GetProducedItems() => Producers.Select(c => c.ItemLevel);

        protected virtual void updateProduction()
        {
            switch (_productionState)
            {
                case ProductionState.Idle:
                    _productionState = ProductionState.Working;
                    break;
                case ProductionState.Working:
                    bool isProgressing = Building.Efficiency > 0f;
                    if (_isProgressing != isProgressing)
                    {
                        _isProgressing = isProgressing;
                        IsProgressing?.Invoke(_isProgressing);
                    }

                    if (addProgress(Building.Efficiency))
                    {
                        setState(ProductionState.Done);
                        _isProgressing = false;
                        IsProgressing?.Invoke(false);
                    }

                    break;
                case ProductionState.Done:
                    if (canProduce())
                    {
                        produce();
                        setState(ProductionState.Idle);
                        resetProgress();
                    }

                    break;
                default:
                    break;
            }
        }

        protected virtual void setState(ProductionState productionState)
        {
            _productionState = productionState;
        }

        protected virtual bool canWork()
        {
            return true;
        }

        protected virtual bool canProduce()
        {
            return ItemsProducers.All(p => p.FitsItems);
        }

        protected virtual void produce()
        {
            foreach (var itemsProducer in ItemsProducers)
            {
                itemsProducer.Produce();
                OwnStorage.AddItems(itemsProducer.Items.Item, itemsProducer.Items.Quantity);
            }

            onItemsChanged();
        }

        protected ItemProducer getProducer(Item item) => ItemsProducers.FirstOrDefault(c => c.Items.Item == item);

        protected virtual void onItemsChanged()
        {
            HasProductsChanged?.Invoke(HasProducts);
        }

        #region Saving

        [Serializable]
        public class ProductionData
        {
            public int State;
            public float ProductionTime;
            public ItemStorage.ItemStorageData[] Consumers;
            public ItemStorage.ItemStorageData[] Producers;
        }

        public override string SaveData()
        {
            var data = new ProductionData();

            saveData(data);

            return JsonUtility.ToJson(data);
        }

        public override void LoadData(string json)
        {
            loadData(JsonUtility.FromJson<ProductionData>(json));
        }

        protected void saveData(ProductionData data)
        {
            data.State = (int)_productionState;
            data.ProductionTime = _progressTime;
            data.Producers = ItemsProducers.Select(c => c.Storage.SaveData()).ToArray();
        }

        protected void loadData(ProductionData data)
        {
            _productionState = (ProductionState)data.State;
            _progressTime = data.ProductionTime;
            for (int i = 0; i < ItemsProducers.Length; i++)
            {
                ItemsProducers[i].Storage.LoadData(data.Producers[i]);
            }

            onItemsChanged();
        }

        #endregion
    }
}