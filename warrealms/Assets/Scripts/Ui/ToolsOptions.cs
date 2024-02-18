using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolsOptions : MonoBehaviour
{
    public GameObject mainOptionsBar; // A barra com as opções principais
    public GameObject productionOptionsBar; // A barra com as opções de produção
    public GameObject resourcesOptionsBar; // A barra com as opções de recursos
    public GameObject cityOptionsBar; // A barra com as opções de cidade
    public GameObject storageOptionsBar; // A barra com as opções de estoque


    // Método para ativar as opções de produção e desativar as outras
    public void OnProductionClicked()
    {
        mainOptionsBar.SetActive(false);
        productionOptionsBar.SetActive(true);
        resourcesOptionsBar.SetActive(false);
        cityOptionsBar.SetActive(false);
        storageOptionsBar.SetActive(false);
    }

    public void OnResourcesClicked()
    {
        mainOptionsBar.SetActive(false);
        productionOptionsBar.SetActive(false);
        resourcesOptionsBar.SetActive(true);
        cityOptionsBar.SetActive(false);
        storageOptionsBar.SetActive(false);
    }

    public void OnCityClicked()
    {
        mainOptionsBar.SetActive(false);
        productionOptionsBar.SetActive(false);
        resourcesOptionsBar.SetActive(false);
        cityOptionsBar.SetActive(true);
        storageOptionsBar.SetActive(false);
    }

    public void OnStorageClicked()
    {
        mainOptionsBar.SetActive(false);
        productionOptionsBar.SetActive(false);
        resourcesOptionsBar.SetActive(false);
        cityOptionsBar.SetActive(false);
        storageOptionsBar.SetActive(true);
    }

    public void OnBackgroundClicked()
    {
        mainOptionsBar.SetActive(true);
        productionOptionsBar.SetActive(false);
        resourcesOptionsBar.SetActive(false);
        cityOptionsBar.SetActive(false);
        storageOptionsBar.SetActive(false);
    }
}