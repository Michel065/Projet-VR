using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Système d'interaction VR par regard + bouton.
/// Attache ce script sur le XR Origin (ou la Main Camera).
/// Le joueur regarde un objet IInteractable et appuie sur le bouton
/// défini pour déclencher Interact().
/// </summary>
public class VRInteractionRaycaster : MonoBehaviour
{
    [Header("Paramètres raycasting")]
    public float interactRange = 3f;
    public LayerMask interactableLayer;

    [Header("VR Input — bouton d'interaction")]
    [Tooltip("Nom du bouton XR (ex: PrimaryButton, Trigger)")]
    public string interactButton = "PrimaryButton";

    [Header("Viseur UI (optionnel)")]
    public GameObject reticle; // petit cercle au centre de la vue

    private IInteractable currentTarget;
    private Camera vrCamera;

    void Start()
    {
        vrCamera = Camera.main;
    }

    void Update()
    {
        DetectTarget();

        // Détection du bouton (adapte selon ton SDK VR)
        if (Input.GetButtonDown(interactButton) && currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    void DetectTarget()
    {
        Ray ray = new Ray(vrCamera.transform.position, vrCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                currentTarget = interactable;
                if (reticle) reticle.SetActive(true);
                return;
            }
        }

        currentTarget = null;
        if (reticle) reticle.SetActive(false);
    }

    void OnDrawGizmosSelected()
    {
        if (vrCamera == null) return;
        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(vrCamera.transform.position,
                       vrCamera.transform.forward * interactRange);
    }
}