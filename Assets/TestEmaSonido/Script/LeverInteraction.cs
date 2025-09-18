using UnityEngine;

public class LeverInteraction : MonoBehaviour
{
    [Header("Lever State")]
    public bool isUp = false; // Estado actual

    [Header("Lever Contribution")]
    public float contributionUp = 1f;
    public float contributionDown = 0f;

    [SerializeField] private ProximitySwitch proximitySwitch;

    private void Start()
    {
        proximitySwitch.OnSwitch += Interact;
    }

    public float GetContribution()
    {
        return isUp ? contributionUp : contributionDown;
    }


     void Interact(bool value)
    {
        isUp = value; // Cambia el estado
                      //UpdateVisual();
        GetComponent<Animator>().SetBool("MoveLever", isUp);
        Debug.Log("Palanca " + gameObject.name + " ahora está " + (isUp ? "arriba" : "abajo"));
    }

}