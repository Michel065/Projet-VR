using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// DialogueChainManager — Gestionnaire de chaîne de dialogues
/// ─────────────────────────────────────────────────────────────
/// SETUP UNITY :
///   1. Crée un GameObject vide "DialogueChainManager" dans la Hierarchy
///   2. Attache ce script dessus
///   3. Pour chaque PNJ, configure un DialogueLink (voir Inspector)
///   4. Tous les PNJ sauf le premier démarrent sur un nœud WaitAction
///      avec awaitedActionKey = "LOCKED" (ils sont bloqués par défaut)
///   5. Le premier PNJ n'est PAS bloqué — il démarre directement
///
/// LOGIQUE :
///   PNJ_DuoA (libre) → parle → débloque PNJ_DuoB
///   PNJ_DuoB (bloqué) → parle → débloque PNJ_Informateur
///   PNJ_Informateur (bloqué) → parle → complète Q1
/// ─────────────────────────────────────────────────────────────
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

        [Header("Clé WaitAction de ce PNJ (si bloqué au départ)")]
        [Tooltip("Doit correspondre au awaitedActionKey dans le DialogueData. Laisse vide si ce PNJ démarre libre.")]
        public string lockKey = "LOCKED";

        [Header("Ce PNJ est-il bloqué au départ ?")]
        public bool startsLocked = true;

        // État interne
        [HideInInspector] public bool unlocked = false;
        [HideInInspector] public bool hasFinished = false;
    }

    [Header("Chaîne de dialogues — dans l'ordre")]
    public List<DialogueLink> chain = new List<DialogueLink>();

    void Start()
    {
        // Débloque le premier PNJ automatiquement
        foreach (var link in chain)
        {
            if (!link.startsLocked)
            {
                link.unlocked = true;
                Debug.Log($"[ChainManager] {link.pnjName} démarre libre.");
            }
            else
            {
                Debug.Log($"[ChainManager] {link.pnjName} démarre bloqué (clé: {link.lockKey}).");
            }
        }
    }

    void Update()
    {
        foreach (var link in chain)
        {
            if (link.thisPNJ == null) continue;
            if (link.hasFinished) continue;

            // Vérifie si ce PNJ est sur un nœud WaitAction
            bool isOnWaitAction = link.thisPNJ.waitingForAction;
            bool isLockNode = isOnWaitAction &&
                              link.thisPNJ.currentNode != null &&
                              link.thisPNJ.currentNode.awaitedActionKey == link.lockKey;

            // Si PNJ bloqué et vient d'être débloqué → avance son dialogue
            if (isLockNode && link.unlocked)
            {
                link.thisPNJ.ReceiveAction(link.lockKey);
                Debug.Log($"[ChainManager] {link.pnjName} débloqué → dialogue lancé.");
            }

            // Vérifie si ce PNJ a fini son dialogue
            if (link.thisPNJ.dialogueDone && !link.hasFinished)
            {
                link.hasFinished = true;
                Debug.Log($"[ChainManager] {link.pnjName} a fini → débloque le suivant.");
                UnlockNext(link);
            }
        }
    }

    void UnlockNext(DialogueLink finishedLink)
    {
        if (finishedLink.pnjToUnlock == null) return;

        // Trouve le lien correspondant au PNJ à débloquer
        foreach (var link in chain)
        {
            if (link.thisPNJ == finishedLink.pnjToUnlock)
            {
                link.unlocked = true;
                Debug.Log($"[ChainManager] {link.pnjName} est maintenant débloqué !");
                return;
            }
        }
    }

    // Appelé depuis l'extérieur pour débloquer un PNJ manuellement
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