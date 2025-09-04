using System;
using UnityEngine;

[Serializable]
public class InterfaceRef<T> where T : class
{
    [SerializeField] private MonoBehaviour _component;   // arrastrable en el inspector

    public T Value => _component as T;                   // acceso tipado a la interfaz
    public bool IsAssigned => _component != null;
#if UNITY_EDITOR
    // Validación en editor: solo acepta componentes que implementen T
    public void ValidateOrNull()
    {
        if (_component != null && (_component as T) == null)
        {
            Debug.LogWarning($"El objeto '{_component.name}' no implementa {typeof(T).Name}. Se limpia la referencia.");
            _component = null;
        }
    }
#endif
}
