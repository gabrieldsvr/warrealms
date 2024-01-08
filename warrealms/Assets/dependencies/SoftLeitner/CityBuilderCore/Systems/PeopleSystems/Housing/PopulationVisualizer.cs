
using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// visualizes employment as text in unity ui<br/>
    /// </summary>
    public class PopulationVisualizer : MonoBehaviour
    {
        [Tooltip("population that gets visualized on the TMPro text component")]
        public Population Population;
        [Tooltip("text component for the population in the form of '<name>: <quantity> / <capacity>'")]
        public TMPro.TMP_Text Text;

        private IPopulationManager _populationManager;

        private void Start()
        {
            _populationManager = Dependencies.Get<IPopulationManager>();
        }

        private void Update()
        {
            Text.text = $"{Population.Name}: {_populationManager.GetQuantity(Population)} / {_populationManager.GetCapacity(Population)}";
        }
    }
}