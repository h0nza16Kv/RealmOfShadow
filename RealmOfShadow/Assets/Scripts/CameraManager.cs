using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera mapCamera; 

    private bool mapMode = false;

    void Start()
    {
        if (mapCamera != null)
            mapCamera.enabled = false;

        if (mainCamera != null)
            mainCamera.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && mapCamera != null && mainCamera != null)
        {
            mapMode = !mapMode;
            mainCamera.enabled = !mapMode;
            mapCamera.enabled = mapMode;
        }
    }
}
