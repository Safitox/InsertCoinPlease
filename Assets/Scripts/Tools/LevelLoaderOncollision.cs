using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderOncollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Victoria");
         }
    }

}
