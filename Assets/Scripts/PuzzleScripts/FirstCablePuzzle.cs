using System.Collections.Generic;
using UnityEngine;

public class FirstCablePuzzle : MonoBehaviour
{
    [SerializeField] private ProximityPositioner[] proximityPositioners;
    [SerializeField] private DoorControl doorControl;
    private List<string> connectedCables = new List<string>();
    void Start()
    {
        foreach (var positioner in proximityPositioners)
        {
            positioner.Connected += ConnectedCable;
        }
    }

    void ConnectedCable(bool connected, string id)
    {
        if (connected)
        {
            if (!connectedCables.Contains(id))
                connectedCables.Add(id);
        }
        else
        {
            if (connectedCables.Contains(id))
                connectedCables.Remove(id);
        }

        doorControl.ChangeDoorStatus(connectedCables.Count == proximityPositioners.Length);


    }
}
