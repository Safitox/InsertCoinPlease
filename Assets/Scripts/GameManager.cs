using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform player;
    public int lives = 3;
    public Vector3 lastCheckpoint;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService (new ResourceLoader());
        DontDestroyOnLoad(this.gameObject);
    }


    public void StartNewGame()
    {
        lastCheckpoint = Vector3.zero;
    }

    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            Invoke("LoadDefeatScreen", 3f);
            Debug.Log("Game Over");

        }
        else
        {
            Invoke("LoadFromCheckpoint", 3f);
            Debug.Log("Lives left: " + lives);
        }
    }

    void LoadFromCheckpoint()
    {
        player.position = lastCheckpoint;
        player.GetComponent<ThirdPersonController>().animator.Play("Idle");
        player.GetComponent<HealthManager>().Health = 100;
    }


    void LoadDefeatScreen()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Derrota");
    }

    public void LoadVictoryScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Victoria");
    }

}
