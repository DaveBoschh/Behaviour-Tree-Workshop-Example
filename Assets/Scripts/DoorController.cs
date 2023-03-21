using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isOpen;

    private void Update()
    {
        transform.GetChild(0).gameObject.SetActive(!isOpen);
    }
}