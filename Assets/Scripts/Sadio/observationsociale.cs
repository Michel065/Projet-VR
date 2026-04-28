using UnityEngine;

/// <summary>
/// QUÊTE 4 — Trouver l'Immobile
/// ─────────────────────────────────────────────────────────────────
/// SETUP UNITY :
///   • Sur CHAQUE PNJ de la salle : NPCDialogue + DialogueScreen + ce script
///   • Sur le bon PNJ (L'Immobile) : coche isTarget = true et neverMoves = true
///   • DialogueData pour les mauvais PNJ : nœud Normal "Ce n'est pas moi..."
///   • DialogueData pour L'Immobile : nœud Normal révélateur + nœud WaitAction awaitedActionKey="Q4_Found"
///   • L'indice de départ est donné par un autre PNJ via Q4_ObservationSociale.TriggerHint()
///     (appelle cette méthode depuis un UnityEvent dans le DialogueTrigger de l'Informateur)
/// ─────────────────────────────────────────────────────────────────
/// </summary>
[RequireComponent(typeof(NPCDialogue))]
public class Q4_ObservationSociale : MonoBehaviour
{
    [Header("Ce PNJ est-il la cible ?")]
    public bool isTarget = false;

    [Header("Ne bouge jamais (L'Immobile)")]
    public bool neverMoves = false;

    private NPCDialogue npcDialogue;
    private Vector3 startPosition;
    private bool q4Done = false;

    void Start()
    {
        npcDialogue  = GetComponent<NPCDialogue>();
        startPosition = transform.position;
    }

    void Update()
    {
        // Force l'immobilité si neverMoves
        if (neverMoves && Vector3.Distance(transform.position, startPosition) > 0.05f)
            transform.position = startPosition;

        if (q4Done || npcDialogue == null) return;
        if (!npcDialogue.dialogueActive) return;
        if (npcDialogue.currentNode == null) return;

        // Détecte le nœud WaitAction de la cible
        if (isTarget &&
            npcDialogue.waitingForAction &&
            npcDialogue.currentNode.awaitedActionKey == "Q4_Found")
        {
            npcDialogue.ReceiveAction("Q4_Found");
            QuestManager.Instance?.CompleteQuest("Q4_Observation");
            q4Done = true;
            Debug.Log("[Q4] L'Immobile trouvé ! Quête complétée.");
        }
    }

    // ★ Appelle depuis un UnityEvent (ex : fin de dialogue de L'Informateur)
    public static void TriggerHint()
    {
        QuestManager.Instance?.StartQuest("Q4_Observation");
        Debug.Log("[Q4] Indice donné : quelqu'un n'a pas bougé de sa place.");
    }
}