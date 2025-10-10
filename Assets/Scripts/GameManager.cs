using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform player;
    public int lives = 3;
<<<<<<< HEAD
=======
    public Vector3 lastCheckpoint;
    public bool gamePaused = false;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


    public void StartNewGame()
    {
        ServiceLocator.Instance.Clear();
        ServiceLocator.Instance.RegisterService (new ResourceLoader());
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

>>>>>>> main
}
