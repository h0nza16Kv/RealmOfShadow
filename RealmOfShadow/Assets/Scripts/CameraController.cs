using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float aheadDistance = 2f;
    [SerializeField] private float verticalOffset = 1f;
    [SerializeField] private float cameraSpeed = 2f;

    private float lookAhead;
    private float smoothY;

    private void Update()
    {
        lookAhead = Mathf.Lerp(lookAhead, aheadDistance * player.localScale.x, Time.deltaTime * cameraSpeed);

        smoothY = Mathf.Lerp(transform.position.y, player.position.y + verticalOffset, Time.deltaTime * cameraSpeed);

        transform.position = new Vector3(player.position.x + lookAhead, smoothY, transform.position.z);
    }
}
