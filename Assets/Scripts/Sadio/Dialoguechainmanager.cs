using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// DialogueChainManager V2 — surveille Done ET End pour débloquer
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

        [Header("Clé WaitAction (si bloqué au départ)")]
        public string lockKey = "LOCKED";

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
        }
    }

    void Update()
    {
        foreach (var link in chain)
        {
            if (link.thisPNJ == null) continue;
            if (link.hasFinished) continue;

            // Débloque le PNJ si son tour est venu
            bool isOnWaitAction = link.thisPNJ.waitingForAction;
            bool isLockNode = isOnWaitAction &&
                              link.thisPNJ.currentNode != null &&
                              link.thisPNJ.currentNode.awaitedActionKey == link.lockKey;

            if (isLockNode && link.unlocked && link.startsLocked)
            {
                link.thisPNJ.ReceiveAction(link.lockKey);
                Debug.Log($"[ChainManager] {link.pnjName} débloqué !");
            }

            // ✅ Surveille Done ET dialogueDone ET End
            bool estFini = link.thisPNJ.dialogueDone ||
                           link.thisPNJ.dialogueFinished ||
                           EstSurNodeDoneOuEnd(link.thisPNJ);

            if (estFini && !link.hasFinished)
            {
                link.hasFinished = true;
                Debug.Log($"[ChainManager] {link.pnjName} a fini → débloque suivant.");
                UnlockNext(link);
            }
        }
    }

    bool EstSurNodeDoneOuEnd(NPCDialogue pnj)
    {
        if (pnj.currentNode == null) return false;
        return pnj.currentNode.nodeType == DialogueNodeType.Done ||
               pnj.currentNode.nodeType == DialogueNodeType.End;
    }

    void UnlockNext(DialogueLink finishedLink)
    {
        if (finishedLink.pnjToUnlock == null) return;

        foreach (var link in chain)
        {
            if (link.thisPNJ == finishedLink.pnjToUnlock)
            {
                link.unlocked = true;
                Debug.Log($"[ChainManager] {link.pnjName} maintenant débloqué !");
                return;
            }
        }
    }

    public void UnlockPNJ(NPCDialogue pnj)
    {
        foreach (var link in chain)
        {
            if (link.thisPNJ == pnj)
            {
                link.unlocked = true;
                Debug.Log($"[ChainManager] {link.pnjName} débloqué manuellement.");
                return;
            }
        }
    }
}