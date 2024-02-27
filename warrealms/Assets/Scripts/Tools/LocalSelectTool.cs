using CityBuilderCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalSelectTool : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                HudManager hudManager = hit.transform.GetComponent<HudManager>();

                if (hudManager != null)
                {
                    hudManager.ActiveHud();
                }
            }
        }
    }
}