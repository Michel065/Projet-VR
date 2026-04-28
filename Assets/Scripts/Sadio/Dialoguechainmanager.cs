using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Script de debug temporaire — attache sur DialogueChainManager
/// Affiche l'état exact de chaque PNJ dans la Console chaque seconde
/// SUPPRIME ce script une fois le problème réglé
/// </summary>
public class ChainManagerDebug : MonoBehaviour
{
    public List<NPCDialogue> pnjsASurveiller = new List<NPCDialogue>();

    private float timer = 0f;
    private float interval = 1f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < interval) return;
        timer = 0f;

        foreach (var pnj in pnjsASurveiller)
        {
            if (pnj == null) continue;

            string nodeInfo = pnj.currentNode != null
                ? $"Node {pnj.currentNode.id} ({pnj.currentNode.nodeType})"
                : "aucun nœud";

            Debug.Log($"[DEBUG] {pnj.gameObject.name} | " +
                      $"dialogueActive={pnj.dialogueActive} | " +
                      $"dialogueDone={pnj.dialogueDone} | " +
                      $"dialogueFinished={pnj.dialogueFinished} | " +
                      $"waitingForAction={pnj.waitingForAction} | " +
                      $"currentNode={nodeInfo}");
        }
    }
}