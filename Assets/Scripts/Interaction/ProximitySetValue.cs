using System;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class ProximitySetValue : ProximityInteraction
{
   
    //Parámetros
    [SerializeField] float minValue = -1f;
    [SerializeField] float maxValue = 1f;
    [SerializeField] float defaultValue = 0f;
    [SerializeField] float changeSpeed = 0.05f;
    [SerializeField] bool resetOnExit = true;
    
    //Encaje
    public Action<float> OnChangeValue;


    //Internas
    float _interactionValue = 0f;
    
    float interactionValue
    {
        get{ return _interactionValue; }
        set
        {
            _interactionValue = Mathf.Clamp(value, minValue, maxValue);
            //Actualizar UI
            mat.SetFloat("_Fill",1f - (interactionValue-minValue) / (maxValue-minValue));
        }
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetButtonDown("Interact") && !interacting)
            interacting = true;
        if (Input.GetButtonUp("Interact"))
            Reset();

    }
    protected override void FixedUpdate()
    {
        if (!playerInRange)
            return;
        base.FixedUpdate();
        //Intercepto el botón de interacción

        if (!interacting)
            return;
        if (Input.GetButton("Interact"))
            interactionValue += Time.fixedDeltaTime * changeSpeed;
        if (Input.GetButton("InteractMinus"))
            interactionValue -= Time.fixedDeltaTime * changeSpeed;
        
    }



    protected override void Reset()
    {
        if  (resetOnExit)
            interactionValue=defaultValue;
        base.Reset();
    }



}
