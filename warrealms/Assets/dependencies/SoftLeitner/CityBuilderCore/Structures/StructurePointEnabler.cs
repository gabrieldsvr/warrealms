﻿using System.Linq;
using UnityEngine;

namespace CityBuilderCore
{
    /// <summary>
    /// enables a gameobject depending on whether it has all the defined points
    /// </summary>
    public class StructurePointEnabler : MonoBehaviour
    {
        [Tooltip("structure that is checked for the points and rechecked whenever it changes")]
        public string StructureKey;
        [Tooltip("positions of these transforms are checked in the structure")]
        public Transform[] Points;
        [Tooltip("when all points are present in the structure this gameobject is enabled, otherwise disabled")]
        public GameObject GameObject;

        private IGridPositions _gridPositions;

        private void Start()
        {
            _gridPositions = Dependencies.Get<IGridPositions>();
            Dependencies.Get<IStructureManager>().Changed += structuresChanged;
        }

        private void structuresChanged()
        {
            var structure = Dependencies.Get<IStructureManager>().GetStructure(StructureKey);
            if (structure == null)
                GameObject.SetActive(false);
            else
                GameObject.SetActive(Points.Select(p => _gridPositions.GetGridPosition(p.position)).All(p => structure.HasPoint(p)));
        }
    }
}
