using UnityEngine;

/// <summary>
/// QUÊTE 2 — À qui faire confiance ?
/// ─────────────────────────────────────────────────────────────────
/// SETUP UNITY :
///   • Sur le PNJ "L'Informateur" : NPCDialogue + DialogueScreen + ce script + Q5_ObjetQuiBouge (référence)
///   • DialogueData avec 3 nœuds Normal (étapes 1, 2, 3) + 1 nœud Choice final
///     Choice[0] = "Lui faire confiance"  → nextNodeId = nœud neutre
///     Choice[1] = "Chercher moi-même"    → nextNodeId = nœud avec awaitedActionKey = "Q2_Correct"
///   • Lie objetQuete au script Q5_ObjetQuiBouge dans la scène
/// ─────────────────────────────────────────────────────────────────
/// </summary>
[RequireComponent(typeof(NPCDialogue))]
public class Q2_PNJPeuFiable : MonoBehaviour
{
    [Header("Références")]
    public Q5_ObjetQuiBouge objetQuete;

    private NPCDialogue npcDialogue;
    private bool contradiction_shown = false;
    private bool q2Done = false;

    void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
        QuestManager.Instance?.StartQuest("Q2_Unreliable");
    }

    void Update()
    {
        if (q2Done || npcDialogue == null) return;

        // Quand le dialogue est actif, vérifie si l'objet n'est pas là où le PNJ indique
        if (npcDialogue.dialogueActive && !contradiction_shown)
        {
            bool objetAbsent = objetQuete != null && !objetQuete.IsAtOriginalPosition();
            if (objetAbsent)
            {
                contradiction_shown = true;
                Debug.Log("[Q2] Contradiction détectée : l'objet n'est pas là où le PNJ indique.");
                // Le DialogueData gère le texte — ici on log juste pour debug
                // Tu peux aussi changer le DialogueData dynamiquement via :
                // npcDialogue.GoToNode(nodeIdContradiction);
            }
        }

        // Détecte le bon choix : awaitedActionKey = "Q2_Correct"
        if (npcDialogue.waitingForAction &&
            npcDialogue.currentNode != null &&
            npcDialogue.currentNode.awaitedActionKey == "Q2_Correct")
        {
            npcDialogue.ReceiveAction("Q2_Correct");
            QuestManager.Instance?.CompleteQuest("Q2_Unreliable");
            q2Done = true;
        }
    }
}