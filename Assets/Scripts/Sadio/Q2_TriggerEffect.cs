using UnityEngine;

/// <summary>
/// Q2_TriggerEffect — tout automatique, rien à glisser
/// Trouve XR Origin > Camera Offset automatiquement
/// SETUP : attache juste ce script sur pnj_rico, c'est tout !
/// </summary>
[RequireComponent(typeof(NPCDialogue))]
public class Q2_TriggerEffect : MonoBehaviour
{
    [Header("Node ID acceptation (défaut : 4)")]
    public int triggerNodeId = 4;

    [Header("Intensité effet (0 à 1.5)")]
    public float doseAmount = 0.8f;

    [Header("Durée effet en secondes")]
    public float doseDuration = 10f;

    private NPCDialogue npcDialogue;
    private AlcoholEffectController alcoholEffect;
    private bool effectTriggered = false;
    private bool q2Done = false;

    void Start()
    {
        npcDialogue = GetComponent<NPCDialogue>();
        TrouverAlcoolEffect();
    }

    void TrouverAlcoolEffect()
    {
        // Méthode 1 — cherche sur XR Origin (XR Rig)
        GameObject xrOrigin = GameObject.Find("XR Origin (XR Rig)");
        if (xrOrigin != null)
        {
            // Cherche dans Camera Offset (enfant direct)
            Transform cameraOffset = xrOrigin.transform.Find("Camera Offset");
            if (cameraOffset != null)
            {
                alcoholEffect = cameraOffset.GetComponent<AlcoholEffectController>();

                // Si pas sur Camera Offset, l'ajoute automatiquement
                if (alcoholEffect == null)
                {
                    alcoholEffect = cameraOffset.gameObject.AddComponent<AlcoholEffectController>();
                    alcoholEffect.cameraOffset = cameraOffset;
                    Debug.Log("[Q2] AlcoholEffectController ajouté sur Camera Offset ✓");
                }
                else
                {
                    Debug.Log("[Q2] AlcoholEffectController trouvé sur Camera Offset ✓");
                }
                return;
            }

            // Cherche partout dans les enfants du XR Origin
            alcoholEffect = xrOrigin.GetComponentInChildren<AlcoholEffectController>();
            if (alcoholEffect != null)
            {
                Debug.Log("[Q2] AlcoholEffectController trouvé dans XR Origin ✓");
                return;
            }
        }

        // Méthode 2 — cherche via tag Player
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            alcoholEffect = player.GetComponentInChildren<AlcoholEffectController>();
            if (alcoholEffect != null)
            {
                Debug.Log("[Q2] AlcoholEffectController trouvé via tag Player ✓");
                return;
            }
        }

        // Méthode 3 — cherche partout dans la scène
        alcoholEffect = FindObjectOfType<AlcoholEffectController>();
        if (alcoholEffect != null)
        {
            Debug.Log("[Q2] AlcoholEffectController trouvé dans la scène ✓");
            return;
        }

        Debug.LogWarning("[Q2] AlcoholEffectController introuvable ! Vérifie que le script est dans la scène.");
    }

    void Update()
    {
        if (npcDialogue == null) return;
        if (!npcDialogue.dialogueActive) return;
        if (npcDialogue.currentNode == null) return;

        int currentId = npcDialogue.currentNode.id;

        // Node 4 = joueur accepte → effet visuel
        if (currentId == triggerNodeId && !effectTriggered)
        {
            effectTriggered = true;
            if (alcoholEffect != null)
            {
                alcoholEffect.AddDose(doseAmount, doseDuration);
                Debug.Log("[Q2] Effet de désorientation déclenché !");
            }
        }

        // Dialogue fini → quête complétée
        if (npcDialogue.dialogueDone && !q2Done)
        {
            q2Done = true;
            QuestManager.Instance?.CompleteQuest("Q2_Unreliable");
            Debug.Log("[Q2] Quête complétée !");
        }
    }
}