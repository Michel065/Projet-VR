using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DrinkingCup : MonoBehaviour
{
    public float drinkHeight = 1.4f;
    public AudioClip drinkSound;
    private AudioSource audioSource;
    private bool isDrinking = false;
    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            StartCoroutine(DisableAfterSound());
        }
    }

    IEnumerator DisableAfterSound()
    {
        if (drinkSound != null)
        {
            audioSource.PlayOneShot(drinkSound);
            yield return new WaitForSeconds(drinkSound.length);
        }
        gameObject.SetActive(false);
    }
}