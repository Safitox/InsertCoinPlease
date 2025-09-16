using UnityEngine;

public class test_Carry : MonoBehaviour, ICarryable
{


    public string Identity { get; set; } = "BotonRojo";
    public bool EnableCarry { get; set; } = true;


    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public void OnCarry()
    {

    }

    public void OnDrop()
    {

    }

}
