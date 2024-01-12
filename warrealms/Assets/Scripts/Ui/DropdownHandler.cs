using UnityEngine;
using UnityEngine.UI;

public class DropdownHandler : MonoBehaviour
{
    public TMPro.TMP_Dropdown dropdown;
    public TMPro.TMP_Text selectedOptionText;

    void Start()
    {
        // Garanta que o dropdown e o texto estão associados
        if (dropdown != null && selectedOptionText != null)
        {
            // Adiciona um listener para lidar com a mudança de seleção
            dropdown.onValueChanged.AddListener(delegate {
                DropdownItemSelected(dropdown);
            });

            // Atualiza o texto inicialmente com a primeira opção
            DropdownItemSelected(dropdown);
        }
    }

    void DropdownItemSelected(TMPro.TMP_Dropdown dropdown)
    {
        int index = dropdown.value;
        // selectedOptionText.text = "Selecionado: " + dropdown.options[index].text;
    }
}