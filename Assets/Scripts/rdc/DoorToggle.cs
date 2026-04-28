using UnityEngine;

public class DoorToggle : MonoBehaviour
{
    public Transform door;          // objet à rotate
    public float openAngle = 90f;
    public float speed = 2f;

    private bool isOpen = false;
    private bool isMoving = false;

    private Quaternion closedRot;
    private Quaternion openRot;

    private void Start()
    {
        if (door == null)
            door = transform;

        closedRot = door.rotation;
        openRot = Quaternion.Euler(door.eulerAngles + new Vector3(0, openAngle, 0));
    }

    public void ToggleDoor()
    {
        if (isMoving)
            return;

        isOpen = !isOpen;
        StopAllCoroutines();
        StartCoroutine(RotateDoor(isOpen ? openRot : closedRot));
    }

    private System.Collections.IEnumerator RotateDoor(Quaternion target)
    {
        isMoving = true;

        while (Quaternion.Angle(door.rotation, target) > 0.1f)
        {
            door.rotation = Quaternion.Slerp(door.rotation, target, Time.deltaTime * speed);
            yield return null;
        }

        door.rotation = target;
        isMoving = false;
    }
}