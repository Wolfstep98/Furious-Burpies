using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider))]
public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("CollisionDeath");
        if (collision.tag == GameObjectsTags.Player)
        {
            Debug.Log("DED");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
