using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLife : MonoBehaviour
{
    public bool isEnabled = true;
    [SerializeField] private BossController bossController;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isEnabled) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            bossController.KillBoss();

            isEnabled = false;

            Invoke(nameof(GoToVictory), 10f); 
        }
    }


    void GoToVictory()
    {
        SceneManager.LoadScene("Victoria",LoadSceneMode.Single);
    }


}
