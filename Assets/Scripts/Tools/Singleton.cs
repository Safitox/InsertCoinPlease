using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool m_ShuttingDown = false;
    private static object m_Lock = new object();
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            if (m_ShuttingDown)
            {
                return null;
            }

            lock (m_Lock)
            {
                if (m_Instance == null)
                {
                    // Chequeo si existe una instancia
                    m_Instance = (T)FindFirstObjectByType(typeof(T));

                    if (m_Instance == null)
                    {
                        // Si no existe, creo un GO para el singleton
                        var singletonObject = new GameObject();
                        m_Instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T).ToString() + " (Singleton)";

                        // Hacer persistente
                     //   DontDestroyOnLoad(singletonObject);
                    }
                }

                return m_Instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        m_ShuttingDown = true;
    }


    private void OnDestroy()
    {
        m_ShuttingDown = true;
    }
}


public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
{
    private static T m_Instance = null;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindFirstObjectByType<T>();
                if (m_Instance == null)
                    m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
            }
            return m_Instance;
        }
    }
}