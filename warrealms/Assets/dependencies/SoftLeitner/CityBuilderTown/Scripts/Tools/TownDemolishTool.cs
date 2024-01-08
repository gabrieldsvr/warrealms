using CityBuilderCore;
using System.Collections.Generic;
using UnityEngine;

namespace CityBuilderTown
{
    /// <summary>
    /// tool that removes tasks and places demlish tasks for structures
    /// </summary>
    public class TownDemolishTool : PointerToolBase
    {
        [Tooltip("is displayed as its tooltip")]
        public string Name;

        public override string TooltipName => Name;

        private IHighlightManager _highlighting;

        protected override void Start()
        {
            base.Start();

            _highlighting = Dependencies.Get<IHighlightManager>();
        }

        protected override void updatePointer(Vector2Int mousePoint, Vector2Int dragStart, bool isDown, bool isApply)
        {
            _highlighting.Clear();

            IEnumerable<Vector2Int> points;

            if (isDown)
            {
                points = PositionHelper.GetBoxPositions(dragStart, mousePoint);
            }
            else
            {
                if (IsTouchActivated)
                    points = new Vector2Int[] { };
                else
                    points = new Vector2Int[] { mousePoint };
            }

            _highlighting.Highlight(points, false);

            if (isApply)
            {
                if (TownManager.Instance.Demolish(points))
                onApplied();
            }
        }
    }
}