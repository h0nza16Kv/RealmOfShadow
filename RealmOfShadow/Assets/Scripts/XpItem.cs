using UnityEngine;

public class XpItem : MonoBehaviour
{
    [SerializeField] private int xpAmount = 5;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            XpManager xpManager = FindObjectOfType<XpManager>();

            if (xpManager != null && xpManager.level < 10)
            {
                xpManager.GainXpFromItem(xpAmount);
                Destroy(gameObject);
            }
        }
    }
}
