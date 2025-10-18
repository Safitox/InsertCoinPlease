using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MenuButtonMouseBehaviour : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{

    [SerializeField] private bool haAlphaHitTest = false;
    [SerializeField] private AudioClip sndHovered;
    [SerializeField] private AudioClip sndClicked;
    

    public GameObject showObject;

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        GetComponent<AudioSource>().PlayOneShot(sndHovered);
    }
}

