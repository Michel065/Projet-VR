using UnityEngine;

public class PhoneManager : MonoBehaviour
{
    [Header("Dialogue")]
    public NPCDialogue npcDialogue;
    public string actionKey = "find_phone";
    public int waitNodeId = 3;
    public int hintNodeId = 2;

    [Header("Target")]
    public Collider targetHandCollider;
    public string targetHandTag = "PNJ_Hand";
    public GameObject haloObject;

    [Header("State")]
    public bool consumeOnSuccess = true;
    public bool unlockOnlyOnce = true;

    private bool unlocked;

    private void Start()
    {
        UpdateHalo();
    }

    private void Update()
    {
        UpdateHalo();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (unlocked && unlockOnlyOnce)
            return;

        if (npcDialogue == null)
            return;

        if (targetHandCollider != null && other != targetHandCollider)
        {
            if (!other.CompareTag(targetHandTag))
                return;
        }
        else
        {
            if (!other.CompareTag(targetHandTag))
                return;
        }

        if (npcDialogue.dialogueDone)
            return;

        if (npcDialogue.currentNode == null)
            return;

        if (npcDialogue.currentNode.id != waitNodeId)
            return;

        npcDialogue.UnlockWaitAction(actionKey, true);

        unlocked = true;
        UpdateHalo();

        if (consumeOnSuccess)
            gameObject.SetActive(false);
    }

    private void UpdateHalo()
    {
        if (haloObject == null || npcDialogue == null)
            return;

        bool show = false;

        if (!npcDialogue.dialogueDone && npcDialogue.currentNode != null)
        {
            int id = npcDialogue.currentNode.id;
            show = (id == hintNodeId || id == waitNodeId);
        }

        haloObject.SetActive(show);
    }
}
