using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallMissDetector : MonoBehaviour
{
    private bool scoredPoint = false;
    private bool launched = false;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (!launched)
        {
            launched = true;
            scoredPoint = false;
            StartCoroutine(CheckIfScored());
        }
    }

    IEnumerator CheckIfScored()
    {
        yield return new WaitForSeconds(2.5f);
        if (!scoredPoint)
        {
            Debug.Log("Raté ! Le joueur doit boire.");
            BallManager.instance.SpawnDrinkingCup();
        }
    }

    public void SetScored()
    {
        scoredPoint = true;
    }
}