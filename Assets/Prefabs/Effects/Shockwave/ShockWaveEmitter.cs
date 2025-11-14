using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class ShockWaveEmitter : MonoBehaviour
{

    [SerializeField] private GameObject pf_Emitter;
    [SerializeField] private int subdivissions = 10;
    [SerializeField] private float impulse = 10f;
    [SerializeField] private float duration = 10f;
    private List<GameObject> emitters = new List<GameObject>();
    void Start()
    {
        PrepareEmitter();
        //InvokeRepeating("EmitShockWave", duration+3,duration+3);
    }

    


    void PrepareEmitter()
    {
        for (int i = 0; i < subdivissions; i++)
        {
            GameObject go = Instantiate(pf_Emitter, transform);
            emitters.Add(go);
            go.transform.rotation=Quaternion.Euler(0, (360f / subdivissions) * i, 0);
            go.SetActive(false);
            go.GetComponent<WaveLife>().lifeTime = duration;
        }
        
    }


    public void EmitShockWave()
    {
        foreach (var emitter in emitters)
        {
            emitter.SetActive(true);
            emitter.transform.position = transform.position;
            var rb = emitter.GetComponent<Rigidbody>();
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = emitter.transform.forward * impulse;
            
        }
    }
}
