using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField] private ArrowTrap arrowTrap; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
          //  arrowTrap.ActivateTrap(); 
            Destroy(gameObject); 
        }
    }
}
