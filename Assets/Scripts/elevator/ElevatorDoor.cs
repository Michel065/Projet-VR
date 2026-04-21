using UnityEngine;

public class ElevatorDoor : MonoBehaviour
{
    [Header("References")]
    public ElevatorController elevator;
    public Transform doorLeft;
    public Transform doorRight;

    [Header("Config")]
    public bool isInElevator = false;
    public int floorIndex = 0;
    public float speed = 3f;

    [Header("Positions Z")]
    public float openZ = 1.3f;
    public float closedZ = 0.5f;

    void Update()
    {
        if (elevator == null || doorLeft == null || doorRight == null)
            return;

        bool shouldOpen = ShouldOpen();
        UpdateDoors(shouldOpen);
    }

    bool ShouldOpen()
    {
        if (elevator.state != ElevatorController.ElevatorState.Idle)
            return false;

        if (isInElevator)
            return true;

        return elevator.currentFloor == floorIndex;
    }

    void UpdateDoors(bool isOpen)
    {
        float targetZ = isOpen ? openZ : closedZ;

        Vector3 rightTarget = new Vector3(
            doorRight.localPosition.x,
            doorRight.localPosition.y,
            targetZ
        );

        doorRight.localPosition = Vector3.Lerp(
            doorRight.localPosition,
            rightTarget,
            Time.deltaTime * speed
        );

        Vector3 leftTarget = new Vector3(
            doorLeft.localPosition.x,
            doorLeft.localPosition.y,
            -targetZ
        );

        doorLeft.localPosition = Vector3.Lerp(
            doorLeft.localPosition,
            leftTarget,
            Time.deltaTime * speed
        );
    }

    public bool IsClosed()
    {
        float tolerance = 0.05f;

        float rightZ = doorRight.localPosition.z;
        float leftZ = doorLeft.localPosition.z;

        bool rightClosed = Mathf.Abs(rightZ - closedZ) < tolerance;
        bool leftClosed = Mathf.Abs(leftZ + closedZ) < tolerance;

        return rightClosed && leftClosed;
    }
}