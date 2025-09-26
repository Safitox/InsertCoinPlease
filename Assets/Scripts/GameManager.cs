using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public Transform player;
    public int lives = 3;

    private void Awake()
    {
        ServiceLocator.Instance.RegisterService (new ResourceLoader());
    }


    public void LoseLife()
    {
        lives--;
        if (lives <= 0)
        {
            Debug.Log("Game Over");
        }
        else
        {
            Debug.Log("Lives left: " + lives);
        }
    }


}
