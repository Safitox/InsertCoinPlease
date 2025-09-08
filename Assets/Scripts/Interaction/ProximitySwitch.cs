using System;
using UnityEngine;

public class ProximitySwitch:ProximityInteraction
{
    [SerializeField] InteractionObject  interactableObject;
        float _interactionTime = 0f;
        float interactionTime
        {
            get { return _interactionTime; }
            set
            {
                _interactionTime = Mathf.Clamp(value, 0f, interactableObject.TimeToExecute);
                //Actualizar UI
                mat.SetFloat("_Fill",1f - _interactionTime/ interactableObject.TimeToExecute);
            }
        }
    public Action<bool> OnSwitch;
    private bool toggle = false;

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
                toggle = !toggle;
                OnSwitch?.Invoke(toggle);
                if (interactableObject.OneUse)
                    DestroyImmediate(gameObject);
                else
                    Reset();

            }
        }
    }

    protected override void Reset()
    {
        interactionTime = interactableObject.TimeToExecute;
        base.Reset();
    }
}
