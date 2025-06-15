using System;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private string requiredTag = "Pushable"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(requiredTag))
        {
            door.SetActive(false); 
            gameObject.SetActive(false);
        }
    }
}
