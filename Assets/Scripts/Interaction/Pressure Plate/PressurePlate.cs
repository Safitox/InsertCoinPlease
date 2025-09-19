using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class PressurePlate : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Transform movingPart;          // Parte que baja
    [SerializeField] private Vector3 pressedYOffset = new Vector3(0f, -0.05f, 0f);
    [SerializeField] private float moveSpeed = 6f;         

    [Header("Activación")]
    [SerializeField] private float requiredMass = 10f;      // Masa mínima para activar (kg)
    [SerializeField] private LayerMask detectableLayers = ~0;// Capas que cuentan
    [SerializeField] private bool countCharacterControllersAsMass = true;
    [SerializeField] private float characterMass = 70f;     // “masa” simulada para jugador
    [SerializeField] private bool oneShot = false;          // Si true, queda presionada para siempre tras activarse

    [Header("Estabilidad")]
    [SerializeField] private float releaseDelay = 0.1f;     // Anti-rebote al liberar

    [Header("Eventos")]
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private Vector3 _initialLocalPos;
    private HashSet<Collider> _contacts = new HashSet<Collider>();
    private float _lastAboveThresholdTime;
    private bool _isPressed;
    private bool _hasFiredOneShot;

    private void Awake()
    {
        if (movingPart == null) movingPart = transform;
        _initialLocalPos = movingPart.localPosition;

        var col = GetComponent<Collider>();
        col.isTrigger = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & detectableLayers) == 0) return;
        _contacts.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _contacts.Remove(other);
    }

    private float ComputeMass()
    {
        float total = 0f;
        foreach (var c in _contacts)
        {
            if (c == null) continue;

            if (c.attachedRigidbody != null)
            {
                total += Mathf.Max(0f, c.attachedRigidbody.mass);
            }
            else if (countCharacterControllersAsMass && c.TryGetComponent<ThirdPersonController>(out _))
            {
                total += characterMass;
            }
        }
        return total;
    }

    private void FixedUpdate()
    {
        float m = ComputeMass();
        bool over = m >= requiredMass || (_hasFiredOneShot && oneShot);

        if (over)
        {
            _lastAboveThresholdTime = Time.time;

            if (!_isPressed)
            {
                _isPressed = true;
                if (oneShot && !_hasFiredOneShot) _hasFiredOneShot = true;
                onPressed?.Invoke();
            }
        }
        else if (!oneShot)
        {
            // Libera solo si pasó suficiente tiempo por debajo del umbral (anti-rebote)
            if (_isPressed && Time.time - _lastAboveThresholdTime >= releaseDelay)
            {
                _isPressed = false;
                onReleased?.Invoke();
            }
        }

        // Mover tapa con lerp suave
        Vector3 target = _isPressed ? _initialLocalPos + pressedYOffset : _initialLocalPos;
        movingPart.localPosition = Vector3.Lerp(movingPart.localPosition, target, Time.deltaTime * moveSpeed);
    }


}
