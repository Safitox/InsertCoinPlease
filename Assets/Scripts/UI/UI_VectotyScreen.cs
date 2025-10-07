using UnityEngine;

public class UI_VectotyScreen : MonoBehaviour
{
    void Start()
    {
        Invoke("GotoMenu", 5f);
    }

    void GotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuV2");
    }
}
