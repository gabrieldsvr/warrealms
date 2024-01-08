using UnityEngine;
using UnityEngine.EventSystems;

namespace CityBuilderCore
{
    /// <summary>
    /// only exists because OnPointerExit behaviour has changed in Unity 2021<br/>
    /// can be removed in earlier versions
    /// </summary>
    public class ToolsGroupContent : MonoBehaviour, IPointerExitHandler
    {
        [Tooltip("OnPointerExit is forwarded to this group")]
        public ToolsGroup Group;

        public void OnPointerExit(PointerEventData eventData)
        {
            Group.OnPointerExit(eventData);
        }
    }
}
