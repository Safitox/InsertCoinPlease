using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MenuButtonMouseBehaviour : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{

    public GameObject showObject;


    public void OnPointerExit(PointerEventData eventData)
    {
        showObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showObject.SetActive(true);
    }
}

