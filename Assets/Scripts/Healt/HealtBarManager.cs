using UnityEngine;
using UnityEngine.UI;

public class HealtBarManager : MonoBehaviour
{
   
    Slider silderHealtBar;
    
    Camera _camera;

    HealthManager healtManager;

    int healthBarValue;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        silderHealtBar = GetComponentInChildren<Slider>();
        healtManager = GetComponentInParent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform cam = _camera.transform;
        //Le digo que no importa donde este siempre este orientado en frente a la camara
        transform.LookAt(cam);
        silderHealtBar.value = healtManager.Health/100f;
        //Hay que agregar un if


    }
}