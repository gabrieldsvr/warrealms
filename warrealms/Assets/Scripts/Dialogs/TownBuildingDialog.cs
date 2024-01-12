using CityBuilderCore;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CityBuilderTown
{
    /// <summary>
    /// visualizes the state of a town building in the UI
    /// </summary>
    public class TownBuildingDialog : MonoBehaviour
    {
        [Tooltip("displays the buildings name")]
        public TMPro.TMP_Text Name;
        [Tooltip("displays the buildings description")]
        public TMPro.TMP_Text Description;
        [Tooltip("toggle that suspends the building when unchecked")]
        public Toggle Toggle;
        [Tooltip("displays the buildings items")]
        public ItemsPanel ItemsPanel;

        private BuildingReference _currentBuilding;

        private void Start()
        {
            Toggle.onValueChanged.AddListener(new UnityAction<bool>(buildingToggleChanged));

            Hide();
        }

        private void Update()
        {
            if (!_currentBuilding.HasInstance)
            {
                Hide();
                return;
            }

            var itemOwner = _currentBuilding.Instance.GetBuildingParts<IItemOwner>().FirstOrDefault();
            if (itemOwner == null)
                ItemsPanel.Clear();
            else
                ItemsPanel.SetItems(itemOwner.ItemContainer);

            var description = _currentBuilding.Instance.GetDescription();
            var home = _currentBuilding.Instance.GetBuildingComponent<TownHomeComponent>();
            if (home)
                description += $"({home.Inhabitants.Count}/{home.WalkerCapacity})";
            Description.text = description;
        }

        public void Show(BuildingReference building)
        {
            gameObject.SetActive(true);

            _currentBuilding = building;

            Name.text = building.Instance.GetName();
            
            Toggle.SetIsOnWithoutNotify(!building.Instance.IsSuspended);

            Dependencies.GetOptional<IMainCamera>()?.Jump(building.Instance.WorldCenter);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void buildingToggleChanged(bool value)
        {
            if (value)
                _currentBuilding.Instance.Resume();
            else
                _currentBuilding.Instance.Suspend();
        }
    }
}
