using System.Collections.Generic;
using UnityEngine;

public class GuardianPathMoveOnDialogueDone : MonoBehaviour
{
    [Header("Dialogue")]
    public NPCDialogue guardianDialogue;

    [Header("Move")]
    public Transform objectToMove;
    public List<Transform> pathPoints = new List<Transform>();
    public float speed = 2f;
    public float stopDistance = 0.05f;

    [Header("Rotation")]
    public bool rotateTowardsTargetWhileMoving = true;
    public bool applyPointRotationOnArrival = true;
    public float rotationSpeed = 8f;

    [Header("Animation")]
    public Animator animator;
    public string walkParameter = "isWalking";

    private bool moving = false;
    private bool started = false;
    private int currentPointIndex = 0;

    private void Start()
    {
        if (objectToMove == null)
            objectToMove = transform;
    }

    private void Update()
    {
        if (!started && guardianDialogue != null && guardianDialogue.dialogueDone)
        {
            started = true;
            currentPointIndex = 0;
            moving = pathPoints != null && pathPoints.Count > 0;
            SetWalkAnimation(moving);
        }

        if (!moving || objectToMove == null)
            return;

        if (currentPointIndex >= pathPoints.Count)
        {
            moving = false;
            SetWalkAnimation(false);
            return;
        }

        Transform target = pathPoints[currentPointIndex];
        if (target == null)
        {
            currentPointIndex++;
            return;
        }

        Vector3 currentPosition = objectToMove.position;
        Vector3 targetPosition = target.position;

        if (rotateTowardsTargetWhileMoving)
        {
            Vector3 dir = targetPosition - currentPosition;
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                objectToMove.rotation = Quaternion.Slerp(
                    objectToMove.rotation,
                    lookRot,
                    rotationSpeed * Time.deltaTime
                );
            }
        }

        objectToMove.position = Vector3.MoveTowards(
            currentPosition,
            targetPosition,
            speed * Time.deltaTime
        );


        if (Vector3.Distance(objectToMove.position, targetPosition) <= stopDistance)
        {
            objectToMove.position = targetPosition;

            if (applyPointRotationOnArrival)
                objectToMove.rotation = target.rotation;

            currentPointIndex++;

            if (currentPointIndex >= pathPoints.Count)
            {
                moving = false;
                SetWalkAnimation(false);
            }
        }
    }

    private void SetWalkAnimation(bool value)
    {
        if (animator == null || string.IsNullOrEmpty(walkParameter))
            return;

        animator.SetBool(walkParameter, value);
    }
}