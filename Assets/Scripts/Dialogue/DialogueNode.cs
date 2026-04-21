using System;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueNodeType
{
    Normal,
    Choice,
    WaitAction,
    Done,
    End
}

[Serializable]
public class DialogueChoice
{
    public string choiceText;
    public int nextNodeId;
}

[Serializable]
public class DialogueNode
{
    public int id;
    [TextArea(2, 5)] public string message;
    public DialogueNodeType nodeType = DialogueNodeType.Normal;

    public int nextNodeId = -1;

    public List<DialogueChoice> choices = new List<DialogueChoice>();

    public string awaitedActionKey = "";
    public int successNextNodeId = -1;
    public int failNextNodeId = -1;
}