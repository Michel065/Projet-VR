using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueData", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    public int startNodeId = 0;
    public List<DialogueNode> nodes = new List<DialogueNode>();

    public DialogueNode GetNodeById(int id)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].id == id)
                return nodes[i];
        }

        return null;
    }
}