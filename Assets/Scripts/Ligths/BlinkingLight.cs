using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class BlinkingLight : MonoBehaviour
{
    public Light _light;
    public float _interval = 0.5f;

    private Coroutine controlCoroutine;

    public static bool blinkingOn = false;

    void Start()
    {
        if (_light == null)
            _light = GetComponent<Light>();

        controlCoroutine = StartCoroutine(Blinking());
    }

    private IEnumerator Blinking()
    {
        while (!blinkingOn)
        {
            _light.enabled = !_light.enabled;
            yield return new WaitForSeconds(_interval);
        }

        _light.enabled = true;
    }
}
