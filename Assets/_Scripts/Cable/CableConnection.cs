using JetBrains.Annotations;
using UnityEngine;

public class CableConnection : MonoBehaviour, ICarryable
{
    [SerializeField] Color cableColor = Color.white;
    public string CableIdentity;

    public string Identity { get; set ; } 
    public bool EnableCarry { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    bool ICarryable.CarryMoving { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        Identity = CableIdentity;

    }

    public void OnCarry()
    {
        throw new System.NotImplementedException();
    }

    public void OnDrop()
    {
        throw new System.NotImplementedException();
    }
}
