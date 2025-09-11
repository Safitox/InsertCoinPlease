using System;
using UnityEngine;
using static UnityEngine.ProBuilder.AutoUnwrapSettings;

public class ProximitySetValue : ProximityInteraction
{
    [Header("Suscribir OnChangeValue(float)")]
    //Parámetros
    [SerializeField] float minValue = -1f;
    [SerializeField] float maxValue = 1f;
    [SerializeField] float defaultValue = 0f;
    [SerializeField] float changeSpeed = 0.05f;
    [SerializeField] bool retentive = true;
    [SerializeField] bool resetOnExit = true;
    
    //Encaje
    public Action<float> OnChangeValue;


    //Referencias
    [SerializeField] GameObject buttonDecrease, buttonIncrease;

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
        {
            buttonIncrease.SetActive(true);
            interacting = true;
        }
        else if (Input.GetButtonUp("Interact"))
        {
            buttonIncrease.SetActive(false);
            if (!Input.GetButton("InteractMinus"))
                Reset();
        }

        if (Input.GetButtonDown("InteractMinus") && !interacting)
        {
            buttonDecrease.SetActive(true);
            interacting = true;
        }
        else if (Input.GetButtonUp("InteractMinus"))
        {
            buttonDecrease.SetActive(false);
            if (!Input.GetButton("Interact"))
                Reset();
        }

    }
    protected override void FixedUpdate()
    {
        if (!playerInRange)
            return;
        base.FixedUpdate();
        //Intercepto el botón de interacción

        if (!interacting || !retentive)
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
        buttonIncrease.SetActive(false);
        buttonDecrease.SetActive(false);
        base.Reset();
    }



}
