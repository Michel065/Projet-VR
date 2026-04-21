using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SecondaryDialogueEntry
{
    public int mainNodeId;
    [TextArea(2, 5)] public string message;
}

public class SecondaryDialogue : MonoBehaviour
{
    [Header("Link")]
    public NPCDialogue mainDialogue;
    public DialogueScreenSecondary dialogueScreen;

    [Header("Entries")]
    public List<SecondaryDialogueEntry> entries = new List<SecondaryDialogueEntry>();

    [Header("State")]
    public bool activeDialogue;
    public DialogueNode currentNode;

    private void Awake()
    {
        currentNode = new DialogueNode();
        currentNode.id = -1;
        currentNode.nodeType = DialogueNodeType.Normal;
        currentNode.nextNodeId = -1;
    }

    public bool HasDialogueForCurrentMainNode()
    {
        if (mainDialogue == null || mainDialogue.currentNode == null)
        {
            return false;
        }
        var entry = GetEntryByNodeId(mainDialogue.currentNode.id);
        if (entry == null)
        {
            return false;
        }
        return true;
    }

    public void StartDialogueForCurrentMainNode()
    {
        if (mainDialogue == null || mainDialogue.currentNode == null)
        {
            return;
        }

        SecondaryDialogueEntry entry = GetEntryByNodeId(mainDialogue.currentNode.id);

        if (entry == null)
        {
            return;
        }

        activeDialogue = true;
        currentNode.message = entry.message;
        currentNode.nodeType = DialogueNodeType.Normal;
        currentNode.nextNodeId = -1;

        if (mainDialogue.dialogueScreen != null)
        {
            mainDialogue.dialogueScreen.HideAll();
        }

        if (dialogueScreen != null)
        {
            dialogueScreen.RefreshDisplay();
        }
    }

    public void NextTrigger()
    {
        if (!activeDialogue)
        {
            return;
        }

        HideSecondary();

        if (mainDialogue != null)
        {
            mainDialogue.skipSecondaryOnce = true;
            mainDialogue.NextTrigger();

            if (mainDialogue.dialogueScreen != null)
            {
                mainDialogue.dialogueScreen.RefreshDisplay();
            }
        }
    }

    public void HideSecondary()
    {
        activeDialogue = false;

        if (dialogueScreen != null)
            dialogueScreen.HideAll();
    }

    private SecondaryDialogueEntry GetEntryByNodeId(int nodeId)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            if (entries[i].mainNodeId == nodeId)
                return entries[i];
        }

        return null;
    }
}