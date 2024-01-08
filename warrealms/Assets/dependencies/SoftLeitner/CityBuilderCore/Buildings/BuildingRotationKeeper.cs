using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// optional component that keeps rotation between different builders
    /// </summary>
    public class BuildingRotationKeeper : MonoBehaviour
    {
        [Tooltip("initial rotation used to create a building roation that is then shared between builders")]
        public int InitialRotation;

        public BuildingRotation Rotation { get; set; }

        private void Awake()
        {
            Dependencies.Register(this);
        }

        private void Start()
        {
            Rotation = BuildingRotation.Create(InitialRotation);
        }
    }
}
