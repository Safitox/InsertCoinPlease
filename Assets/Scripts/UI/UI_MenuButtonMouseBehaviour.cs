using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MenuButtonMouseBehaviour : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{

    [SerializeField] private bool haAlphaHitTest = false;

    public GameObject showObject;

    private void Start()
    {
        if (haAlphaHitTest)
            GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        showObject.SetActive(true);
    }
}

