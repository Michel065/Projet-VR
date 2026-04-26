using UnityEngine;

/// <summary>
/// QUÊTE 1 — Reconstituer la vérité
/// ─────────────────────────────────────────────────────────────────
/// SETUP UNITY :
///   • Crée 3 PNJ (L'Informateur, Duo A, Duo B)
///   • Sur chaque PNJ : NPCDialogue + DialogueScreen + ce script
///   • Crée 3 DialogueData ScriptableObjects (Assets > Create > Dialogue > Dialogue Data)
///   • Dans chaque DialogueData, ajoute des nœuds Normal + un nœud Choice sur le PNJ final
///   • Sur le PNJ final, coche isFinalPNJ = true
///   • Sur le nœud Choice du PNJ final : choix[0] = "Duo A", choix[1] = "Duo B", choix[2] = "Personne"
///   • Sur le nœud Choice, nextNodeId du bon choix doit pointer vers un nœud avec awaitedActionKey = "Q1_Correct"
/// ─────────────────────────────────────────────────────────────────
/// </summary>
[RequireComponent(typeof(NPCDialogue))]
public class Q1_ConversationReconstituer : MonoBehaviour
{
    [Header("Identité")]
    public string fragmentId = "fragment_A"; // unique par PNJ (A, B, C)

    [Header("Ce PNJ est-il le dernier à parler ?")]
    public bool isFinalPNJ = false;

    // Suivi global des fragments (statique = partagé entre tous les PNJ Q1)
    private static int fragmentsCollected = 0;
    private static int requiredFragments  = 3;
    private static bool q1Done = false;

    private NPCDialogue npcDialogue;
    private bool hasGivenFragment = false;

    void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
        // Abonne l'événement de fin de dialogue pour détecter le bon choix
        // On surveille via Update car NPCDialogue n'expose pas d'événement OnEnd
    }

    void Update()
    {
        if (q1Done || npcDialogue == null) return;

        // Collecte le fragment quand le dialogue démarre
        if (npcDialogue.dialogueActive && !hasGivenFragment)
        {
            hasGivenFragment = true;
            fragmentsCollected++;
            QuestManager.Instance?.StartQuest("Q1_Conversation");
            Debug.Log($"[Q1] Fragment {fragmentId} collecté ({fragmentsCollected}/{requiredFragments})");
        }

        // Détecte le bon choix sur le PNJ final : awaitedActionKey = "Q1_Correct"
        if (isFinalPNJ && npcDialogue.waitingForAction)
        {
            if (npcDialogue.currentNode != null &&
                npcDialogue.currentNode.awaitedActionKey == "Q1_Correct" &&
                fragmentsCollected >= requiredFragments)
            {
                // Valide l'action → le dialogue avance
                npcDialogue.ReceiveAction("Q1_Correct");
                QuestManager.Instance?.CompleteQuest("Q1_Conversation");
                q1Done = true;
            }
        }
    }

    // Appelé par un bouton UI ou un événement Unity si tu veux forcer la complétion
    public void ForceComplete()
    {
        if (!q1Done)
        {
            QuestManager.Instance?.CompleteQuest("Q1_Conversation");
            q1Done = true;
        }
    }
}