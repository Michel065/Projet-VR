using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHoverText : MonoBehaviour
{
    public GameObject textObject;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;

    [Header("Camera")]
    public Transform targetCamera;
    public bool followCamera = true;

    private void Reset()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void Awake()
    {
        if (interactable == null)
            interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        if (textObject != null)
            textObject.SetActive(false);

        if (targetCamera == null && Camera.main != null)
            targetCamera = Camera.main.transform;
    }

    private void OnEnable()
    {
        if (interactable == null)
            return;

        interactable.hoverEntered.AddListener(OnHoverEntered);
        interactable.hoverExited.AddListener(OnHoverExited);
    }

    private void OnDisable()
    {
        if (interactable == null)
            return;

        interactable.hoverEntered.RemoveListener(OnHoverEntered);
        interactable.hoverExited.RemoveListener(OnHoverExited);
    }

    private void Update()
    {
        if (!followCamera || textObject == null || !textObject.activeSelf || targetCamera == null)
            return;

        Vector3 dir = targetCamera.position - textObject.transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        textObject.transform.rotation = Quaternion.LookRotation(-dir);
    }

    private void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (textObject != null)
            textObject.SetActive(true);
    }

    private void OnHoverExited(HoverExitEventArgs args)
    {
        if (textObject != null)
            textObject.SetActive(false);
    }
}