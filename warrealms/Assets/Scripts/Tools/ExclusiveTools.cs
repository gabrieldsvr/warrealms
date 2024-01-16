using System;
using CityBuilderCore;
using UnityEngine;

namespace CityBuilderTown
{

    public class ExclusiveTools : MonoBehaviour
    {

        public TownCivilization _exclusiveCivilizatio;
        private void Start()
        {
      
            if (_exclusiveCivilizatio.Key != TownManager.Instance.GetCivilization().Key)
            {
                this.gameObject.SetActive(false);
            }
            
        }
    }
}