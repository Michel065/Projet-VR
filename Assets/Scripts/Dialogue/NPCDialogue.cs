using UnityEngine;
using UnityEngine.InputSystem;

public class NPCDialogue : MonoBehaviour
{
    [Header("Data")]
    public DialogueData dialogueData;

    [Header("Display")]
    public DialogueScreen dialogueScreen;

    [Header("State")]
    public DialogueNode currentNode;
    public bool dialogueActive;
    public bool waitingForChoice;
    public bool waitingForAction;
    public bool dialogueFinished;
    public bool dialogueDone = false;

    [Header("Secondary")]
    public SecondaryDialogue secondaryDialogue;
    [HideInInspector] public bool skipSecondaryOnce;

    public void Interact()
    {
        if (dialogueDone)
            return;

        if (dialogueData == null || dialogueScreen == null)
            return;

        if (!dialogueActive)
            StartDialogue();
        else
            dialogueScreen.RefreshDisplay();
    }

    public void StartDialogue()
    {
        if (dialogueData == null || dialogueScreen == null || dialogueDone)
            return;

        if (dialogueActive)
        {
            dialogueScreen.RefreshDisplay();
            return;
        }

        dialogueActive = true;
        dialogueDone = false;
        waitingForChoice = false;
        waitingForAction = false;
        dialogueFinished = false;
        skipSecondaryOnce = false;

        GoToNode(dialogueData.startNodeId);
    }

    public void GoToNode(int nodeId)
    {
        if (dialogueData == null)
            return;

        currentNode = dialogueData.GetNodeById(nodeId);

        if (currentNode == null)
        {
            EndDialogue();
            return;
        }

        waitingForChoice = false;
        waitingForAction = false;

        switch (currentNode.nodeType)
        {
            case DialogueNodeType.Normal:
                break;

            case DialogueNodeType.Choice:
                if (currentNode.choices == null || currentNode.choices.Count < 2)
                {
                    EndDialogue();
                    return;
                }
                waitingForChoice = true;
                break;

            case DialogueNodeType.WaitAction:
                waitingForAction = true;
                break;

            case DialogueNodeType.End:
                EndDialogue();
                return;

            case DialogueNodeType.Done:
                dialogueDone = true;
                EndDialogue();
                return;

            default:
                EndDialogue();
                return;
        }

        if (dialogueScreen != null)
            dialogueScreen.RefreshDisplay();
    }

    public void NextTrigger()
    {
        if (!dialogueActive || currentNode == null)
            return;

        if (currentNode.nodeType != DialogueNodeType.Normal)
            return;

        if (secondaryDialogue != null &&
            secondaryDialogue.HasDialogueForCurrentMainNode() &&
            !skipSecondaryOnce)
        {
            secondaryDialogue.StartDialogueForCurrentMainNode();
            return;
        }

        skipSecondaryOnce = false;
        if (currentNode.nextNodeId < 0)
        {
            EndDialogue();
            return;
        }
        GoToNode(currentNode.nextNodeId);
    }

    public void ButtonTrigger(bool value)
    {
        if (!dialogueActive || !waitingForChoice || currentNode == null)
            return;

        if (currentNode.nodeType != DialogueNodeType.Choice)
            return;

        if (currentNode.choices == null || currentNode.choices.Count < 2)
        {
            EndDialogue();
            return;
        }

        int nextNodeId = value ? currentNode.choices[0].nextNodeId : currentNode.choices[1].nextNodeId;

        if (nextNodeId < 0)
        {
            EndDialogue();
            return;
        }

        GoToNode(nextNodeId);
    }

    public void UnlockWaitAction(string actionKey, bool success = true)
    {
        if (!dialogueActive || !waitingForAction || currentNode == null)
            return;

        if (currentNode.nodeType != DialogueNodeType.WaitAction || dialogueDone)
            return;

        if (currentNode.awaitedActionKey != actionKey)
            return;

        int nextNodeId = success ? currentNode.successNextNodeId : currentNode.failNextNodeId;

        if (nextNodeId < 0)
        {
            EndDialogue();
            return;
        }

        GoToNode(nextNodeId);
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        waitingForChoice = false;
        waitingForAction = false;
        dialogueFinished = true;
        currentNode = null;
        skipSecondaryOnce = false;

        if (secondaryDialogue != null)
            secondaryDialogue.HideSecondary();

        if (dialogueScreen != null)
            dialogueScreen.HideAll();
    }
}