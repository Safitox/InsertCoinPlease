using System.Collections;
using UnityEngine;

public class Eyelids : MonoBehaviour
{
    [SerializeField] GameObject lids;
    void Start()
    {
     StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 8));
            lids.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            lids.SetActive(false);
        }

    }
}
