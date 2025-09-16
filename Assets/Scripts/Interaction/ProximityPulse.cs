using System;
using UnityEngine;

public class ProximityPulse:ProximityInteraction
{
    [Header("Suscribir OnPulse()")]
    public Action OnPulse;

    //[SerializeField] InteractionObject  interactableObject;
    [SerializeField] float TimeToExecute = 1f;
    [SerializeField] bool oneUse = false;


    float _interactionTime = 0f;
        float interactionTime
        {
            get { return _interactionTime; }
            set
            {
                _interactionTime = Mathf.Clamp(value, 0f, TimeToExecute);
                //Actualizar UI
                mat.SetFloat("_Fill",1f - _interactionTime/ TimeToExecute);
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
        if (!interacting)
            return;
        if (Input.GetButton("Interact"))
        {
            interactionTime -= Time.fixedDeltaTime;
            if (interactionTime <= 0f)
            {
                OnPulse?.Invoke();
                if (oneUse)
                    DestroyImmediate(gameObject);
                else
                    Reset();

            }
        }
    }

    protected override void Reset()
    {
        interactionTime = TimeToExecute;
        base.Reset();
    }
}
