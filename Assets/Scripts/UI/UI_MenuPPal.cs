using UnityEngine;
public class UI_MenuPPal : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MatiScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}