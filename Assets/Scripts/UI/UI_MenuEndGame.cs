using UnityEngine;

public class UI_MenuEndGame : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void GotoMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuV2");
    }
}
