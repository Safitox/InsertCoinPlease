using UnityEngine;
using Unity.UI;
using UnityEngine.UI;

public class HealtBarManager : MonoBehaviour
{
   
    Slider silderHealtBar;
    
    Camera camera;

    HealthManager healtManager;

    int healthBarValue;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        camera = Camera.main;
        silderHealtBar = GetComponentInChildren<Slider>();
        healtManager = GetComponentInParent<HealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform cam = camera.transform;
        //Le digo que no importa donde este siempre este orientado en frente a la camara
        transform.LookAt(cam);
        silderHealtBar.value = healtManager.Health/100f;
        //Hay que agregar un if


    }
}