using UnityEngine;

public class GuardianDialogueUnlock : MonoBehaviour
{
    [Header("Main")]
    public NPCDialogue mainDialogue;
    public int requiredNodeId = 9;
    public string actionKey = "gardian";

    [Header("Guardian")]
    public NPCDialogue guardianDialogue;
    public Collider guardianTrigger;
    public DialogueScreen guardianScreen;

    private bool guardianUnlocked = false;
    private bool unlockVisualApplied = false;
    private int validateCount = 0;

    private void Start()
    {
        if (guardianScreen != null)
            guardianScreen.HideAll();

        if (guardianDialogue != null)
            guardianDialogue.enabled = false;

        if (guardianTrigger != null)
            guardianTrigger.enabled = false;
    }

    private void Update()
    {
        CheckGuardianUnlock();
    }

    private void CheckGuardianUnlock()
    {
        if (guardianUnlocked)
            return;

        if (mainDialogue == null || mainDialogue.currentNode == null)
            return;

        if (mainDialogue.currentNode.id != requiredNodeId)
            return;

        guardianUnlocked = true;

        if (guardianDialogue != null)
            guardianDialogue.enabled = true;

        if (guardianTrigger != null)
            guardianTrigger.enabled = true;

        if (!unlockVisualApplied && guardianScreen != null)
        {
            if (guardianDialogue != null)
                guardianDialogue.Interact();
            if (guardianScreen != null)
                guardianScreen.RefreshDisplay();
            unlockVisualApplied = true;
        }
    }

    public void ValidateGuardianOnce(Collider other)
    {
        if (validateCount > 0)
            return;

        if (!guardianUnlocked)
            return;

        if (mainDialogue == null)
            return;

        if (mainDialogue.currentNode == null)
            return;

        mainDialogue.UnlockWaitAction(actionKey, true);

        if (guardianDialogue != null)
        {
            guardianDialogue.dialogueActive = false;
            guardianDialogue.dialogueDone = false;
        }

        validateCount++;
    }
}