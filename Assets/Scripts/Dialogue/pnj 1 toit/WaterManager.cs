using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [Header("Dialogue")]
    public NPCDialogue npcDialogue;
    public string actionKey = "water";
    public int waitNodeId = 5;
    public int hintNodeId = 4;

    [Header("Target")]
    public Collider targetHandCollider;
    public string targetHandTag = "main pnj 1";
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
        
        //npcDialogue.NextTrigger();
        
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