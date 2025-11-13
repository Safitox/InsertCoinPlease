using UnityEngine;
using UnityEngine.SceneManagement;

public class BossLife : MonoBehaviour
{
    public bool isEnabled = true;
    [SerializeField] private Animator bossAnimator;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isEnabled) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar lógica para reducir la vida del jefe
            Debug.Log("Boss hit add link to animation");
            //bossAnimator.SetTrigger("Die");

            isEnabled = false;

            Invoke(nameof(GoToVictory), 10f); 
        }
    }


    void GoToVictory()
    {
        SceneManager.LoadScene("Victoria",LoadSceneMode.Single);
    }


}
