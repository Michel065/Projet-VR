using System.Collections.Generic;
using UnityEngine;

public class PNJ3PrincipalMove : MonoBehaviour
{
    [Header("Dialogue")]
    public NPCDialogue npcDialogue;

    [Header("Dialogue Trigger Nodes")]
    public int walkNodeId = 13;
    public string walkActionKey = "pnj3ready";

    public int runNodeId = 14;
    public string runActionKey = "saut";

    [Header("Object")]
    public Transform objectToMove;

    [Header("Walk Path")]
    public List<Transform> walkPoints = new List<Transform>();
    public float walkSpeed = 2f;

    [Header("Run Path")]
    public List<Transform> runPoints = new List<Transform>();
    public float runSpeed = 5f;

    [Header("Move Settings")]
    public float stopDistance = 0.05f;
    public bool rotateTowardsTargetWhileMoving = true;
    public bool applyPointRotationOnArrival = true;
    public float rotationSpeed = 8f;

    [Header("Animation")]
    public Animator animator;

    private bool moving = false;
    private bool hasDoneWalk = false;
    private bool hasDoneRun = false;

    private List<Transform> currentPath;
    private int currentPointIndex = 0;
    private float currentSpeed = 0f;
    private string currentActionKey = "";

    private void Start()
    {
        if (objectToMove == null)
            objectToMove = transform;
    }

    private void Update()
    {
        CheckDialogueState();

        if (!moving || objectToMove == null || currentPath == null)
            return;

        MoveAlongPath();
    }

    private void CheckDialogueState()
    {
        if (npcDialogue == null || npcDialogue.currentNode == null)
            return;

        DialogueNode node = npcDialogue.currentNode;

        if (node.nodeType != DialogueNodeType.WaitAction)
            return;

        if (node.id == walkNodeId && !hasDoneWalk && !moving)
        {
            StartMove(walkPoints, walkSpeed, walkActionKey, true, false);
            return;
        }

        if (node.id == runNodeId && !hasDoneRun && !moving)
        {
            StartMove(runPoints, runSpeed, runActionKey, false, true);
            return;
        }
    }

    private void StartMove(List<Transform> path, float speed, string actionKey, bool walking, bool running)
    {
        Debug.Log("[PNJ3] StartMove appelé | actionKey=" + actionKey + " walking=" + walking + " running=" + running);

        currentActionKey = actionKey;

        if (path == null || path.Count == 0)
        {
            Debug.LogWarning("[PNJ3] Aucun chemin trouvé pour " + actionKey);
            FinishMove();
            return;
        }

        Debug.Log("[PNJ3] Chemin trouvé | points=" + path.Count + " speed=" + speed);

        currentPath = path;
        currentSpeed = speed;
        currentPointIndex = 0;
        moving = true;

        SetAnimation(walking, running);
    }

    private void FinishMove()
    {
        Debug.Log("[PNJ3] FinishMove | actionKey=" + currentActionKey);

        moving = false;
        SetAnimation(false, false);

        if (currentActionKey == walkActionKey)
        {
            hasDoneWalk = true;
            Debug.Log("[PNJ3] Marche terminée");
        }

        if (currentActionKey == runActionKey)
        {
            hasDoneRun = true;
            Debug.Log("[PNJ3] Course terminée");
        }

        ValidateAction(currentActionKey);

        if (currentActionKey == runActionKey)
        {
            Debug.Log("[PNJ3] Fin du jeu déclenchée");
            QuitGame();
        }
    }

    private void ValidateAction(string actionKey)
    {
        Debug.Log("[PNJ3] ValidateAction | actionKey=" + actionKey);

        if (npcDialogue != null && !string.IsNullOrEmpty(actionKey))
        {
            npcDialogue.UnlockWaitAction(actionKey, true);
            Debug.Log("[PNJ3] UnlockWaitAction envoyé");
        }
        else
        {
            Debug.LogWarning("[PNJ3] UnlockWaitAction impossible");
        }
    }

    private void SetAnimation(bool walking, bool running)
    {
        Debug.Log("[PNJ3] SetAnimation | walking=" + walking + " running=" + running);

        if (animator == null)
        {
            Debug.LogError("[PNJ3] Animator NULL");
            return;
        }

        animator.SetBool("isWalking", walking);
        animator.SetBool("isRunning", running);

        Debug.Log("[PNJ3] Animator bools envoyés | isWalking="
            + animator.GetBool("isWalking")
            + " isRunning="
            + animator.GetBool("isRunning"));
    }






    private void MoveAlongPath()
    {
        if (currentPointIndex >= currentPath.Count)
        {
            FinishMove();
            return;
        }

        Transform target = currentPath[currentPointIndex];

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
            currentSpeed * Time.deltaTime
        );

        if (Vector3.Distance(objectToMove.position, targetPosition) <= stopDistance)
        {
            objectToMove.position = targetPosition;

            if (applyPointRotationOnArrival)
                objectToMove.rotation = target.rotation;

            currentPointIndex++;

            if (currentPointIndex >= currentPath.Count)
                FinishMove();
        }
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}