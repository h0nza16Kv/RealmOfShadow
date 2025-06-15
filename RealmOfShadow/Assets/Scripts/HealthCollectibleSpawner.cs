using UnityEngine;

public class HealthCollectibleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthCollectiblePrefab;
    private GameObject spawnedCollectible;

    private void Start()
    {
        SpawnHealthCollectible();
    }

    private void SpawnHealthCollectible()
    {
        if (spawnedCollectible == null || !spawnedCollectible.activeSelf) 
        {
            spawnedCollectible = Instantiate(healthCollectiblePrefab, transform.position, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (spawnedCollectible == null || !spawnedCollectible.activeSelf)
        {
            Invoke(nameof(SpawnHealthCollectible), 10f); 
        }
    }
}
