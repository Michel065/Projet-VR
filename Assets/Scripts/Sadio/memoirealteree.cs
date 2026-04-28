using UnityEngine;

/// <summary>
/// QUÊTE 3 — Mémoire fragmentée
/// ─────────────────────────────────────────────────────────────────
/// SETUP UNITY :
///   • Sur le PNJ "Le Mémorieux" : NPCDialogue + DialogueScreen + ce script
///   • DialogueData avec cette structure de nœuds :
///     Node 0 (Normal)  — "Je suis ici depuis 22h. J'ai vu quelqu'un monter."     nextNode=1
///     Node 1 (Choice)  — "Enfin... 22h ou minuit. L'heure m'échappe."
///                        Choice[0]="Signaler l'incohérence" → nextNode=2
///                        Choice[1]="Laisser passer"         → nextNode=2
///     Node 2 (Normal)  — "La personne portait une veste rouge."                  nextNode=3
///     Node 3 (Choice)  — "Attendez... Elle était bleue."
///                        Choice[0]="Signaler l'incohérence" → nextNode=4
///                        Choice[1]="Laisser passer"         → nextNode=4
///     Node 4 (Normal)  — "En tout cas, elle est montée. Je l'ai vu."             nextNode=5
///     Node 5 (WaitAction) awaitedActionKey="Q3_Done" successNextNodeId=6 failNextNodeId=6
///     Node 6 (End)
///   • Les nœuds Choice[0] ont leur nextNodeId marqué comme "incoherence_reported"
///     via awaitedActionKey (voir OnChoiceSelected ci-dessous)
/// ─────────────────────────────────────────────────────────────────
/// </summary>
[RequireComponent(typeof(NPCDialogue))]
public class Q3_MemoireAlteree : MonoBehaviour
{
    [Header("Node IDs des choix 'Signaler' dans le DialogueData")]
    [Tooltip("IDs des nœuds Choice où Choice[0] = signaler l'incohérence")]
    public int[] incoherenceNodeIds = { 1, 3 };

    [Tooltip("Nombre de signalements corrects requis")]
    public int requiredDetections = 2;

    private NPCDialogue npcDialogue;
    private int detections = 0;
    private bool q3Done = false;

    // Suit le dernier nœud traité pour éviter les doubles comptages
    private int lastProcessedNode = -1;

    void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
        QuestManager.Instance?.StartQuest("Q3_Memory");
    }

    void Update()
    {
        if (q3Done || npcDialogue == null) return;
        if (!npcDialogue.dialogueActive) return;
        if (npcDialogue.currentNode == null) return;

        int nodeId = npcDialogue.currentNode.id;

        // Déjà traité ?
        if (nodeId == lastProcessedNode) return;

        // Nœud WaitAction final → on résout la quête
        if (npcDialogue.currentNode.awaitedActionKey == "Q3_Done")
        {
            lastProcessedNode = nodeId;
            ResoudreQuete();
            return;
        }

        // Nœud Choice d'incohérence → écoute le choix du joueur
        if (npcDialogue.waitingForChoice && IsIncoherenceNode(nodeId))
        {
            lastProcessedNode = nodeId;
            // Le choix 0 = signaler. On attend la prochaine frame pour voir quel choix a été fait.
            StartCoroutine(WaitForChoice(nodeId));
        }
    }

    System.Collections.IEnumerator WaitForChoice(int nodeId)
    {
        // Attend que le joueur fasse son choix (waitingForChoice passe à false)
        while (npcDialogue.waitingForChoice)
            yield return null;

        // Vérifie quel nœud on est allé après le choix
        // Si le joueur a choisi Choice[0], l'Animator va vers successNextNodeId
        // On déduit en comparant le nœud actuel
        // Astuce simple : on compare si le nœud suivant correspond au nextNodeId du Choice[0]
        if (npcDialogue.currentNode != null)
        {
            // On considère que si on arrive au même nextNodeId que Choice[0], c'est le bon choix
            // Dans votre DialogueData, Choice[0] et Choice[1] pointent vers le même nextNodeId
            // donc on ne peut pas distinguer → on utilise un flag statique depuis l'UI
            // Alternative : appelle OnPlayerChoseSignaler() depuis un bouton Unity Event sur le bouton Choice[0]
        }
    }

    bool IsIncoherenceNode(int id)
    {
        foreach (int n in incoherenceNodeIds)
            if (n == id) return true;
        return false;
    }

    // ★ Appelle cette méthode depuis le bouton Choice[0] de chaque nœud d'incohérence
    // via UnityEvent dans l'Inspector du bouton UI
    public void OnPlayerChoseSignaler()
    {
        detections++;
        Debug.Log($"[Q3] Incohérence signalée ({detections}/{requiredDetections})");
    }

    void ResoudreQuete()
    {
        npcDialogue.ReceiveAction("Q3_Done");

        if (detections >= requiredDetections)
        {
            Debug.Log("[Q3] Succès — toutes les incohérences détectées !");
        }
        else
        {
            Debug.Log($"[Q3] Partiel — {detections}/{requiredDetections} incohérences détectées.");
        }

        // On complète quand même la quête (même partiellement réussie)
        QuestManager.Instance?.CompleteQuest("Q3_Memory");
        q3Done = true;
    }
}