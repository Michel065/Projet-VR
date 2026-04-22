using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class NearGrabOnly : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public float maxGrabDistance = 0.3f; // distance max pour attraper

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        float distance = Vector3.Distance(
            args.interactorObject.transform.position,
            transform.position
        );

        Debug.Log("Distance : " + distance);

        if (distance > maxGrabDistance)
        {
            grabInteractable.interactionManager.SelectExit(
                args.interactorObject, grabInteractable);
        }
    }
}