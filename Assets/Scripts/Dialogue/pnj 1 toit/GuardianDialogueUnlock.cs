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

    private bool guardianUnlocked;
    private int validateCount = 0;

    private void Start()
    {
        RefreshState();
    }

    private void Update()
    {
        if (!guardianUnlocked && mainDialogue != null && mainDialogue.currentNode != null)
        {
            if (mainDialogue.currentNode.id == requiredNodeId)
            {
                guardianUnlocked = true;
            }
        }

        RefreshState();
    }

    private void RefreshState()
    {
        bool unlocked = guardianUnlocked;

        if (guardianDialogue != null)
            guardianDialogue.enabled = unlocked;

        if (guardianTrigger != null)
            guardianTrigger.enabled = unlocked;

        if (guardianScreen != null)
        {
            if (unlocked)
                guardianScreen.ShowPing();
            else
                guardianScreen.HideAll();
        }
    }

    public void ValidateGuardianOnce(Collider other)
    {
        if (validateCount > 0)
        {
            return;
        }

        if (!guardianUnlocked)
        {
            return;
        }

        if (mainDialogue == null)
        {
            return;
        }

        if (mainDialogue.currentNode == null)
        {
            return;
        }

        mainDialogue.UnlockWaitAction(actionKey, true);

        if (guardianDialogue != null)
        {
            guardianDialogue.dialogueActive = false;
        }

        validateCount++;
    }
}