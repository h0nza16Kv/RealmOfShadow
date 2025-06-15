using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    private bool isActive = false;
    private int collectedKeys = 0;
    [SerializeField] private int requiredKeys = 3; 
    [SerializeField] private bool isBossArena = false; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive && collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void ActivateTeleport()
    {
        if (isBossArena) 
        {
            isActive = true;
        }
        else if (collectedKeys >= requiredKeys) 
        {
            isActive = true;
        }
    }

    public void CollectKey() 
    {
        if (!isBossArena) 
        {
            collectedKeys++;

            if (collectedKeys >= requiredKeys)
            {
                ActivateTeleport();
            }
        }
    }
}
