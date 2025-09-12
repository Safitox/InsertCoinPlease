using UnityEngine;

public class test_Carry : MonoBehaviour, ICarryable
{


    public string Identity { get; set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Identity = "BoxCarry";
    }


    public void OnCarry()
    {

    }

    public void OnDrop()
    {

    }

}
