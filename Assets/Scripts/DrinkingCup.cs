using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DrinkingCup : MonoBehaviour
{
    public float drinkHeight = 1.4f; // hauteur du visage approximative
    private bool isDrinking = false;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args) { isHeld = true; }
    void OnRelease(SelectExitEventArgs args) { isHeld = false; }

    void Update()
    {
     
        if (isHeld && transform.position.y >= drinkHeight && !isDrinking)
        {
            isDrinking = true;
            Debug.Log("Le joueur boit !");
            DrunkManager.instance.Drink();
            BallManager.instance.ClearDrinkingCup();
            gameObject.SetActive(false);
        }
    }
}