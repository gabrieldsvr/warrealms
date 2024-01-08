using CityBuilderCore;
using System.Linq;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// visualizes the state of a town walker in the UI
    /// </summary>
    public class TownWalkerDialog : MonoBehaviour
    {
        private class WalkerEnergy : IWalkerValue
        {
            public bool HasValue(Walker walker) => walker is TownWalker;
            public float GetMaximum(Walker walker) => 100;
            public float GetValue(Walker walker) => ((TownWalker)walker).Energy / ((TownWalker)walker).Identity.EnergyCapacity * 100f;
            public Vector3 GetPosition(Walker walker) => walker.Pivot.position;
        }
        private class WalkerFood : IWalkerValue
        {
            public bool HasValue(Walker walker) => walker is TownWalker;
            public float GetMaximum(Walker walker) => 100;
            public float GetValue(Walker walker) => ((TownWalker)walker).Food / ((TownWalker)walker).Identity.FoodCapacity * 100f;
            public Vector3 GetPosition(Walker walker) => walker.Pivot.position;
        }
        private class WalkerWarmth : IWalkerValue
        {
            public bool HasValue(Walker walker) => walker is TownWalker;
            public float GetMaximum(Walker walker) => 100;
            public float GetValue(Walker walker)
            {
                var townWalker = (TownWalker)walker;
                if (townWalker.Identity.WarmthCapacity == 0f)
                    return 0;

                return townWalker.Warmth / townWalker.Identity.WarmthCapacity * 100f;
            }
            public Vector3 GetPosition(Walker walker) => walker.Pivot.position;
        }

        private CoroutineToken _followToken;
        private TownWalker _currentWalker;

        [Tooltip("displays the walkers name")]
        public TMPro.TMP_Text Name;
        [Tooltip("displays the walkers age")]
        public TMPro.TMP_Text Age;
        [Tooltip("displays the walkers job")]
        public TMPro.TMP_Text Job;
        [Tooltip("displays the walkers current activity")]
        public TMPro.TMP_Text Activity;
        [Tooltip("displays the walkers energy")]
        public WalkerRectBar EnergyBar;
        [Tooltip("displays the walkers food")]
        public WalkerRectBar FoodBar;
        [Tooltip("displays the walkers warmth")]
        public WalkerRectBar WarmthBar;
        [Tooltip("displays the walkers items")]
        public ItemPanel ItemPanel;

        private void Start()
        {
            Hide();
        }

        private void Update()
        {
            if (_followToken?.IsActive == false)
            {
                Hide();
                _followToken = null;
                return;
            }

            if (_currentWalker == null)
                return;

            Age.text = _currentWalker.VisualAge.ToString();
            Job.text = _currentWalker.Job == null ? "-" : _currentWalker.Job.Name;
            Activity.text = _currentWalker.GetActivityText();

            ItemPanel.SetItem(_currentWalker.Storage.GetItemQuantities().FirstOrDefault());
        }

        public void Show(Walker walker) => Show((TownWalker)walker);
        public void Show(TownWalker walker)
        {
            gameObject.SetActive(true);

            _currentWalker = walker;

            Name.text = _currentWalker.Identity.FullName;

            EnergyBar.Initialize(walker, new WalkerEnergy());
            FoodBar.Initialize(walker, new WalkerFood());
            WarmthBar.Initialize(walker, new WalkerWarmth());

            _followToken = Dependencies.GetOptional<IMainCamera>()?.Follow(walker.Pivot);
        }

        public void Hide()
        {
            _followToken?.Stop();
            _followToken = null;

            gameObject.SetActive(false);
        }
    }
}
