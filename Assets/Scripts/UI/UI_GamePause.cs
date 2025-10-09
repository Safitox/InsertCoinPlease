using UnityEngine;

public class UI_GamePause : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    bool gamePaused = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
        GameManager.Instance.gamePaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            GameManager.Instance.gamePaused = gamePaused;
            if (!gamePaused)
                Continue();
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

            }
        }
    }

    public void Continue()
    {
        gamePaused = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false; 
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ExitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuV2");
    }

}
