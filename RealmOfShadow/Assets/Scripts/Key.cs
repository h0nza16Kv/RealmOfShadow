using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Teleport teleport = FindObjectOfType<Teleport>();
            if (teleport != null)
            {
                teleport.CollectKey();
            }

            KeyManager uiManager = FindObjectOfType<KeyManager>();
            if (uiManager != null)
            {
                uiManager.UpdateKeyCount();
            }

            Destroy(gameObject);
        }
    }
}
