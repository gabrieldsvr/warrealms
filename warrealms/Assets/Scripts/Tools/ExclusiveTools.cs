using System;
using CityBuilderCore;
using UnityEngine;

namespace CityBuilderTown
{

    public class ExclusiveTools : MonoBehaviour
    {

        public Civilization _exclusiveCivilizatio;

        public Boolean isActive=false;
        
        

        private void Start()
        {
      
            if (_exclusiveCivilizatio.Key != TownManager.Instance._civilization.Key)
            {
                this.gameObject.SetActive(false);
            }
            
        }
    }
}