using UnityEngine;
using UnityEngine.UI;

public class rawScroll : MonoBehaviour
{
    [SerializeField] private RawImage _imageToScroll=>GetComponent<RawImage>();
    [SerializeField] private float _x, _y;
    // Update is called once per frame
    void Update()
    {
        _imageToScroll.uvRect = new Rect(_imageToScroll.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _imageToScroll.uvRect.size);
    }
}
