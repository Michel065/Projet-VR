using UnityEngine;
using System.Collections;

/// <summary>
/// QUÊTE 5 — L'objet insaisissable
/// ─────────────────────────────────────────────────────────────────
/// SETUP UNITY :
///   • Crée un GameObject "ObjetQuete" avec un XRGrabInteractable + Collider + ce script
///   • Crée 3-4 GameObjects vides dans la scène comme positions (possiblePositions)
///   • Assigne la Main Camera (ou XR Camera) dans vrCamera
///   • Quand le joueur grab l'objet → OnObjectGrabbed() est appelé via
///     l'événement "Select Entered" du XRGrabInteractable dans l'Inspector
///   • Optionnel : NPCDialogue sur un PNJ "Donneur de quête" avec DialogueData
///     dont le dernier nœud a awaitedActionKey = "Q5_Start" → appelle StartQuest via UnityEvent
/// ─────────────────────────────────────────────────────────────────
/// </summary>
public class Q5_ObjetQuiBouge : MonoBehaviour
{
    [Header("Positions possibles de l'objet")]
    public Transform[] possiblePositions;

    [Header("Délai avant téléportation (secondes, si pas regardé)")]
    public float moveDelay = 8f;

    [Header("Caméra VR")]
    public Camera vrCamera;

    [Header("Angle de détection du regard (degrés)")]
    public float visibilityAngle = 35f;

    private int currentIndex = 0;
    private Vector3 originalPosition;
    private bool isCollected = false;
    private float timer = 0f;

    void Start()
    {
        originalPosition = transform.position;
        QuestManager.Instance?.StartQuest("Q5_MovingObject");

        if (vrCamera == null && Camera.main != null)
            vrCamera = Camera.main;
    }

    void Update()
    {
        if (isCollected) return;

        if (!IsInPlayerView())
        {
            timer += Time.deltaTime;
            if (timer >= moveDelay)
            {
                timer = 0f;
                TeleportToNextPosition();
            }
        }
        else
        {
            timer = 0f;
        }
    }

    bool IsInPlayerView()
    {
        if (vrCamera == null) return true; // sécurité : ne bouge pas si pas de caméra

        Vector3 dir = (transform.position - vrCamera.transform.position).normalized;
        float angle = Vector3.Angle(vrCamera.transform.forward, dir);

        // Vérifie aussi que l'objet n'est pas derrière un mur
        if (angle < visibilityAngle)
        {
            Ray ray = new Ray(vrCamera.transform.position, dir);
            float dist = Vector3.Distance(vrCamera.transform.position, transform.position);
            if (!Physics.Raycast(ray, dist, ~LayerMask.GetMask("Interactable")))
                return true;
        }
        return false;
    }

    void TeleportToNextPosition()
    {
        if (possiblePositions == null || possiblePositions.Length == 0) return;

        int newIndex;
        int tries = 0;
        do
        {
            newIndex = Random.Range(0, possiblePositions.Length);
            tries++;
        }
        while (newIndex == currentIndex && tries < 10);

        currentIndex = newIndex;
        transform.position = possiblePositions[currentIndex].position;
        Debug.Log($"[Q5] Objet téléporté → position {currentIndex}");
    }

    // Vrai si l'objet est encore à sa position d'origine (pour Q2)
    public bool IsAtOriginalPosition()
    {
        return Vector3.Distance(transform.position, originalPosition) < 0.3f;
    }

    // ★ Connecte ceci à l'événement "Select Entered" du XRGrabInteractable
    public void OnObjectGrabbed()
    {
        if (isCollected) return;
        isCollected = true;
        Debug.Log("[Q5] Objet ramassé ! Quête complétée.");
        QuestManager.Instance?.CompleteQuest("Q5_MovingObject");
    }
}