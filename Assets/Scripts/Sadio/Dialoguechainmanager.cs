using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// DialogueChainManager V3
/// Quand un PNJ est débloqué, on change son StartNodeId
/// pour qu'il démarre directement au Node 1 (après le WaitAction)
/// </summary>
public class DialogueChainManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLink
    {
        [Header("Ce PNJ")]
        public NPCDialogue thisPNJ;
        public string pnjName = "PNJ";

        [Header("PNJ à débloquer quand celui-ci finit")]
        public NPCDialogue pnjToUnlock;

        [Header("Node ID de départ quand bloqué (WaitAction)")]
        public int lockedStartNode = 0;

        [Header("Node ID de départ quand débloqué (vrai dialogue)")]
        public int unlockedStartNode = 1;

        [Header("Ce PNJ est-il bloqué au départ ?")]
        public bool startsLocked = true;

        [HideInInspector] public bool unlocked = false;
        [HideInInspector] public bool hasFinished = false;
    }

    [Header("Chaîne de dialogues — dans l'ordre")]
    public List<DialogueLink> chain = new List<DialogueLink>();

    void Start()
    {
        foreach (var link in chain)
        {
            if (!link.startsLocked)
            {
                link.unlocked = true;
                Debug.Log($"[ChainManager] {link.pnjName} démarre libre.");
            }
            else
            {
                // PNJ bloqué — on cache le ping pour qu'il soit invisible
                HidePing(link.thisPNJ);
                Debug.Log($"[ChainManager] {link.pnjName} bloqué au départ.");
            }
        }
    }

    void Update()
    {
        foreach (var link in chain)
        {
            if (link.thisPNJ == null) continue;
            if (link.hasFinished) continue;

            // Vérifie si ce PNJ a fini
            bool estFini = EstTermine(link.thisPNJ);

            if (estFini && !link.hasFinished)
            {
                link.hasFinished = true;
                Debug.Log($"[ChainManager] {link.pnjName} a fini → débloque suivant.");
                UnlockNext(link);
            }
        }
    }

    bool EstTermine(NPCDialogue pnj)
    {
        if (pnj.dialogueDone) return true;
        if (pnj.dialogueFinished) return true;
        if (pnj.currentNode == null) return false;

        return pnj.currentNode.nodeType == DialogueNodeType.Done ||
               pnj.currentNode.nodeType == DialogueNodeType.End;
    }

    void UnlockNext(DialogueLink finishedLink)
    {
        if (finishedLink.pnjToUnlock == null) return;

        foreach (var link in chain)
        {
            if (link.thisPNJ != finishedLink.pnjToUnlock) continue;

            link.unlocked = true;

            // ✅ Change le StartNodeId pour sauter le WaitAction
            if (link.thisPNJ.dialogueData != null)
            {
                link.thisPNJ.dialogueData.startNodeId = link.unlockedStartNode;
                Debug.Log($"[ChainManager] {link.pnjName} débloqué → démarre au Node {link.unlockedStartNode}");
            }

            // Montre le ping pour signaler que ce PNJ est maintenant disponible
            ShowPing(link.thisPNJ);
            return;
        }
    }

    void HidePing(NPCDialogue pnj)
    {
        if (pnj == null) return;
        DialogueScreen screen = pnj.GetComponent<DialogueScreen>();
        if (screen == null) screen = pnj.GetComponentInChildren<DialogueScreen>();
        if (screen != null) screen.HideAll();
    }

    void ShowPing(NPCDialogue pnj)
    {
        if (pnj == null) return;
        DialogueScreen screen = pnj.GetComponent<DialogueScreen>();
        if (screen == null) screen = pnj.GetComponentInChildren<DialogueScreen>();
        if (screen != null) screen.ShowPing();
    }

    // Appel manuel depuis l'extérieur
    public void UnlockPNJ(NPCDialogue pnj)
    {
        foreach (var link in chain)
        {
            if (link.thisPNJ != pnj) continue;
            link.unlocked = true;
            if (link.thisPNJ.dialogueData != null)
                link.thisPNJ.dialogueData.startNodeId = link.unlockedStartNode;
            ShowPing(link.thisPNJ);
            Debug.Log($"[ChainManager] {link.pnjName} débloqué manuellement.");
            return;
        }
    }
}