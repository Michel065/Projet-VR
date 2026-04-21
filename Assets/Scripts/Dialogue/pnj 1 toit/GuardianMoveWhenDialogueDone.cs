using System.Collections.Generic;
using UnityEngine;

public class GuardianPathMoveOnDialogueDone : MonoBehaviour
{
    public NPCDialogue guardianDialogue;
    public int requiredNodeId = 3;

    public List<Transform> pathPoints = new List<Transform>();

    public float speed = 2f;
    public float stopDistance = 0.05f;
    public bool rotateTowardsTargetWhileMoving = true;
    public bool applyPointRotationOnArrival = true;

    public Animator animator;
    public string walkParameter = "isWalking";

    private bool moving = false;
    private int currentPointIndex = 0;
    private bool started = false;

    private void Update()
    {
        if (!started && CanStartMove())
        {
            started = true;
            moving = pathPoints != null && pathPoints.Count > 0;
            currentPointIndex = 0;
            SetWalkAnimation(moving);
        }

        if (!moving)
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

        Vector3 targetPosition = target.position;
        Vector3 currentPosition = transform.position;

        if (rotateTowardsTargetWhileMoving)
        {
            Vector3 dir = targetPosition - currentPosition;
            dir.y = 0f;
            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 8f * Time.deltaTime);
            }
        }

        transform.position = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) <= stopDistance)
        {
            transform.position = targetPosition;

            if (applyPointRotationOnArrival)
                transform.rotation = target.rotation;

            currentPointIndex++;

            if (currentPointIndex >= pathPoints.Count)
            {
                moving = false;
                SetWalkAnimation(false);
            }
        }
    }

    private bool CanStartMove()
    {
        return guardianDialogue != null
            && guardianDialogue.dialogueDone
            && guardianDialogue.currentNode != null
            && guardianDialogue.currentNode.id == requiredNodeId;
    }

    private void SetWalkAnimation(bool value)
    {
        if (animator == null || string.IsNullOrEmpty(walkParameter))
            return;

        animator.SetBool(walkParameter, value);
    }
}