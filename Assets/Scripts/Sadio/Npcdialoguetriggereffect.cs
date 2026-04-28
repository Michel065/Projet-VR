using UnityEngine;

/// <summary>
/// NPCDialogueTriggerEffect
/// Déclenche n'importe quel effet quand un NPCDialogue atteint un node précis.
/// Utilisable pour TOUS les PNJ, pas seulement Rico.
///
/// SETUP :
///   1. Attache sur n'importe quel GameObject (PNJ ou manager vide)
///   2. Glisse le NPCDialogue cible dans npcDialogue
///   3. Règle triggerNodeId = ID du node qui déclenche l'effet
///   4. Choisis le type d'effet dans effectType
/// </summary>
public class NPCDialogueTriggerEffect : MonoBehaviour
{
    public enum EffectType
    {
        AlcoolLeger,    // Légère désorientation
        AlcoolFort,     // Forte désorientation
        AlcoolStop,     // Arrête l'effet
    }

    [Header("NPCDialogue à surveiller")]
    public NPCDialogue npcDialogue;

    [Header("Node ID qui déclenche l'effet")]
    public int triggerNodeId = 0;

    [Header("Type d'effet")]
    public EffectType effectType = EffectType.AlcoolLeger;

    [Header("Durée de l'effet (secondes)")]
    public float duration = 10f;

    private AlcoholEffectController alcoholEffect;
    private bool triggered = false;

    void Start()
    {
        // Trouve AlcoholEffectController automatiquement
        GameObject xrOrigin = GameObject.Find("XR Origin (XR Rig)");
        if (xrOrigin != null)
        {
            alcoholEffect = xrOrigin.GetComponentInChildren<AlcoholEffectController>();
            if (alcoholEffect == null)
            {
                Transform camOffset = xrOrigin.transform.Find("Camera Offset");
                if (camOffset != null)
                {
                    alcoholEffect = camOffset.gameObject.AddComponent<AlcoholEffectController>();
                    alcoholEffect.cameraOffset = camOffset;
                }
            }
        }

        if (alcoholEffect == null)
            alcoholEffect = FindObjectOfType<AlcoholEffectController>();

        if (alcoholEffect != null)
            Debug.Log("[TriggerEffect] AlcoholEffectController trouvé ✓");
        else
            Debug.LogWarning("[TriggerEffect] AlcoholEffectController introuvable !");
    }

    void Update()
    {
        if (triggered) return;
        if (npcDialogue == null) return;
        if (!npcDialogue.dialogueActive) return;
        if (npcDialogue.currentNode == null) return;

        if (npcDialogue.currentNode.id == triggerNodeId)
        {
            triggered = true;
            Declencher();
        }
    }

    void Declencher()
    {
        if (alcoholEffect == null) return;

        switch (effectType)
        {
            case EffectType.AlcoolLeger:
                alcoholEffect.AddDose(0.4f, duration);
                Debug.Log("[TriggerEffect] Effet léger déclenché.");
                break;

            case EffectType.AlcoolFort:
                alcoholEffect.AddDose(1.2f, duration);
                Debug.Log("[TriggerEffect] Effet fort déclenché.");
                break;

            case EffectType.AlcoolStop:
                alcoholEffect.ResetEffect();
                Debug.Log("[TriggerEffect] Effet arrêté.");
                break;
        }
    }

    // Remet à zéro pour pouvoir se déclencher à nouveau
    public void Reset()
    {
        triggered = false;
    }
}