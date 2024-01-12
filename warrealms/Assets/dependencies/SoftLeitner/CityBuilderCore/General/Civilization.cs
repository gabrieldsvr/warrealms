using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// object that defines parameters that influence how hard a mission is<br/>
    /// for additional parameters just derive from this object
    /// </summary>
    [CreateAssetMenu(menuName = "CityBuilder/" + nameof(Civilization))]
    public class Civilization : KeyedObject, ICivilizationFactor
    {
        [Tooltip("display name")]
        public string Name;
        
        [Tooltip("about civilization")]
        public string About;

        [Tooltip("influences the speed at which risks increase")]
        public float RiskMultiplier;
        [Tooltip("influences the speed at which services deplete")]
        public float ServiceMultiplier;
        [Tooltip("influences the speed at which items deplete")]
        public float ItemsMultiplier;

        float ICivilizationFactor.RiskMultiplier => RiskMultiplier;
        float ICivilizationFactor.ServiceMultiplier => ServiceMultiplier;
        float ICivilizationFactor.ItemsMultiplier => ItemsMultiplier;
    }
}