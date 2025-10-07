using UnityEngine;
using UnityEngine.SceneManagement;
public class UI_MenuPPal : MonoBehaviour
{
    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void StartGame()
    {
        GameManager.Instance.StartNewGame();

        SceneManager.LoadScene("MatiScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}