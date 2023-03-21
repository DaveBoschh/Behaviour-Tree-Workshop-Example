using Panda;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public GameObject idlePadRoom1, idlePadRoom2, doorPadRoom1, doorPadRoom2, keyPad, door;

    private Vector3 destination = Vector3.zero;
    private int room = 1;
    private float playerYPosition;

    void Awake()
    {
        playerYPosition = transform.position.y;
    }

    [Task] bool HasKey;

    [Task]
    bool IsDoorOpen()
    {
        return door.GetComponent<DoorController>().isOpen;
    }

    [Task]
    void SetDestinationToDoor()
    {
        destination = room == 1
            ? new Vector3(doorPadRoom1.transform.position.x, playerYPosition, doorPadRoom1.transform.position.z)
            : new Vector3(doorPadRoom2.transform.position.x, playerYPosition, doorPadRoom2.transform.position.z);
        ThisTask.Succeed();
    }

    [Task]
    void SetDestinationToOtherRoom()
    {
        switch (room)
        {
            case 1:
                destination = new Vector3(idlePadRoom2.transform.position.x, playerYPosition,
                    idlePadRoom2.transform.position.z);
                room = 2;
                break;
            case 2:
                destination = new Vector3(idlePadRoom1.transform.position.x, playerYPosition,
                    idlePadRoom1.transform.position.z);
                room = 1;
                break;
        }

        ThisTask.Succeed();
    }

    [Task]
    void SetDestinationToKeyPad()
    {
        destination = new Vector3(keyPad.transform.position.x, playerYPosition, keyPad.transform.position.z);
        ThisTask.Succeed();
    }

    [Task]
    void OpenDoor()
    {
        door.GetComponent<DoorController>().isOpen = true;
        ThisTask.Succeed();
    }

    [Task]
    void CloseDoor()
    {
        door.GetComponent<DoorController>().isOpen = false;
        ThisTask.Succeed();
    }

    [Task]
    void MoveToDestination()
    {
        if (transform.position == destination)
            ThisTask.Succeed();

        var delta = destination - transform.position;
        var velocity = delta.normalized * speed;

        transform.position += velocity * Time.deltaTime;

        var newDelta = destination - transform.position;
        if (Vector3.Dot(newDelta, delta) < 0.0f)
        {
            transform.position = destination;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("KeyPad"))
        {
            HasKey = true;
        }
    }
}