using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallLifetime : MonoBehaviour
{
    public float lifetime = 20f;
    public AudioClip bounceSound;
    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (bounceSound != null)
        {
            audioSource.PlayOneShot(bounceSound);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}