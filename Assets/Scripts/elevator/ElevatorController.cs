using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public enum ElevatorState
    {
        Idle,
        MovingUp,
        MovingDown
    }

    [Header("References")]
    public Transform elevatorPlatform;
    public Transform elevatorRoot;

    [Header("Floors")]
    public Transform[] floorPoints;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float stopDistance = 0.02f;

    [Header("State")]
    public int currentFloor = 0;
    public int targetFloor = 0;
    public ElevatorState state = ElevatorState.Idle;

    [Header("Door Intern")]
    public ElevatorDoor door;

    public bool IsMoving => state != ElevatorState.Idle;
    public bool IsGoingUp => state == ElevatorState.MovingUp;
    public bool IsGoingDown => state == ElevatorState.MovingDown;

    void Start()
    {
        if (elevatorRoot == null)
            elevatorRoot = transform;

        if (elevatorPlatform == null)
            elevatorPlatform = elevatorRoot;

        if (floorPoints == null || floorPoints.Length == 0)
        {
            Debug.LogError("ElevatorController : aucun étage défini.");
            enabled = false;
            return;
        }

        currentFloor = Mathf.Clamp(currentFloor, 0, floorPoints.Length - 1);
        targetFloor = currentFloor;

        float deltaY = floorPoints[currentFloor].position.y - elevatorPlatform.position.y;
        elevatorRoot.position = new Vector3(
            elevatorRoot.position.x,
            elevatorRoot.position.y + deltaY,
            elevatorRoot.position.z
        );
    }

    void Update()
    {
        if (!IsMoving)
            return;

        if (door != null && !door.IsClosed())/// on attend que la porte ferme
            return;

        MoveElevator();
    }

    public void GoToFloor(int floorIndex)
    {
        if (floorIndex < 0 || floorIndex >= floorPoints.Length)
        {
            Debug.LogWarning("ElevatorController : étage invalide " + floorIndex);
            return;
        }

        if (floorIndex == currentFloor)
        {
            targetFloor = currentFloor;
            state = ElevatorState.Idle;
            return;
        }

        targetFloor = floorIndex;

        if (floorPoints[targetFloor].position.y > elevatorPlatform.position.y)
            state = ElevatorState.MovingUp;
        else
            state = ElevatorState.MovingDown;
    }

    void MoveElevator()
    {
        float targetY = floorPoints[targetFloor].position.y;
        float currentY = elevatorPlatform.position.y;

        float newPlatformY = Mathf.MoveTowards(currentY, targetY, moveSpeed * Time.deltaTime);
        float deltaY = newPlatformY - currentY;

        elevatorRoot.position = new Vector3(
            elevatorRoot.position.x,
            elevatorRoot.position.y + deltaY,
            elevatorRoot.position.z
        );

        if (Mathf.Abs(newPlatformY - targetY) <= stopDistance)
        {
            float finalDeltaY = targetY - elevatorPlatform.position.y;

            elevatorRoot.position = new Vector3(
                elevatorRoot.position.x,
                elevatorRoot.position.y + finalDeltaY,
                elevatorRoot.position.z
            );

            currentFloor = targetFloor;
            state = ElevatorState.Idle;
        }
    }

    public float GetCurrentTargetHeight()
    {
        return floorPoints[targetFloor].position.y;
    }

    public int GetFloorCount()
    {
        return floorPoints.Length;
    }
}