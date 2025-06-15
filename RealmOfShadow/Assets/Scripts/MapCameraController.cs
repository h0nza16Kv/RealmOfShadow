using UnityEngine;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Camera cam;
    public static bool isMapCameraActive = false;

    void Start()
    {
        cam = GetComponent<Camera>();
        isMapCameraActive = cam.enabled;
    }

    void Update()
    {
        isMapCameraActive = cam.enabled;

        if (!cam.enabled) return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += move * moveSpeed * Time.deltaTime;

        // Omezení pohybu kamery v definovaných hranicích
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
        );
    }
}
