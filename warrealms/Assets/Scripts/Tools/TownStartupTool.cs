using CityBuilderCore;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// tool used when the game first starts to place the initial buildings and walkers<br/>
    /// after it is used once it gets hidden by <see cref="TownManager"/> and the rest of the tools appear
    /// </summary>
    public class TownStartupTool : PointerToolBase
    {
        [Tooltip("is displayed as its tooltip")]
        public string Name;
        [Tooltip("startup buildings placed by this tool, place them relative to 0 and deactivate them")]
        public Building[] Buildings;
        [Tooltip("startup walkers placed by this tool, place them relative to 0 and deactivate them")]
        public TownWalker[] Walkers;

        public override string TooltipName => Name;

        private IHighlightManager _highlighting;
        private IGridPositions _gridPositions;
        private IStructureManager _structureManager;

        private List<Vector2Int> _points;
        private BuildingRotation _rotation;

        protected override void Start()
        {
            base.Start();

            _highlighting = Dependencies.Get<IHighlightManager>();
            _gridPositions = Dependencies.Get<IGridPositions>();
            _structureManager = Dependencies.Get<IStructureManager>();

            _points = new List<Vector2Int>();
            foreach (var building in Buildings)
            {
                _points.AddRange(building.GetPoints());
            }
            foreach (var walker in Walkers)
            {
                _points.Add(walker.GridPoint);
            }
        }

        public override void ActivateTool()
        {
            base.ActivateTool();

            _rotation = Dependencies.GetOptional<BuildingRotationKeeper>()?.Rotation ?? BuildingRotation.Create();
        }

        protected override void updatePointer(Vector2Int mousePoint, Vector2Int dragStart, bool isDown, bool isApply)
        {
            if (!isDown)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    _rotation.TurnClockwise();
                }
            }

            List<Vector2Int> validPoints = new List<Vector2Int>();
            List<Vector2Int> invalidPoints = new List<Vector2Int>();

            foreach (var point in _points)
            {
                var mapPoint = mousePoint + point;

                if (_structureManager.CheckAvailability(mapPoint, 0))
                    validPoints.Add(mapPoint);
                else
                    invalidPoints.Add(mapPoint);
            }

            _highlighting.Clear();
            _highlighting.Highlight(validPoints, true);
            _highlighting.Highlight(invalidPoints, false);

            if (isApply && invalidPoints.Count == 0)
            {
                var worldPosition = _gridPositions.GetWorldPosition(mousePoint);

                foreach (var building in Buildings)
                {
                    building.transform.position += worldPosition;
                    building.gameObject.SetActive(true);
                }
                foreach (var walker in Walkers)
                {
                    
                    GameObject walkerPrefab = TownManager.Instance.GetCivilization().DefaultWalker.Info.Prefab;
                    GameObject walkerInstance = Instantiate(walkerPrefab);
                    TownWalker newWalker = walkerInstance.GetComponent<TownWalker>();
                    
                    // Configurar o novo walker
                    newWalker.transform.position = walker.transform.position + worldPosition;
                    newWalker.Info = TownManager.Instance.GetCivilization().DefaultWalker.Info;
                    newWalker.transform.SetParent(walker.transform.parent.transform, false);
                    newWalker.gameObject.SetActive(true);
                    
                    
                    // Integra o novo walker ao sistema, se necessário
                    TownManager.Instance.Walkers.Integrate(newWalker);
                    
                    // Destruir o walker antigo
                    Destroy(walker.gameObject);
                    
                    // walker.transform.position += worldPosition;
                    // walker.gameObject.SetActive(true);
                    // TownManager.Instance.Walkers.Integrate(walker);
                }

                TownManager.Instance.StartupSet();

                _highlighting.Clear();
            }
        }
    }
}
