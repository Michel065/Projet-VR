using UnityEngine;
using System.Collections;

public class CupDetector : MonoBehaviour
{
    public GameObject cup;
    public float delay = 2.5f;
    public AudioClip successSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

public class CupDetector : MonoBehaviour
{
    public GameObject cup; // glisse le gobelet parent ici dans l'Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            BallMissDetector missDetector = other.GetComponent<BallMissDetector>();
            if (missDetector != null)
                missDetector.SetScored();

            Debug.Log("Point marqué !");

            DrunkManager.instance.AddPoint(); // compteur de points

            if (successSound != null)
                audioSource.PlayOneShot(successSound);

            StartCoroutine(DisableCupAfterDelay(other.gameObject));
        }
    }

    IEnumerator DisableCupAfterDelay(GameObject ball)
    {
        yield return new WaitForSeconds(delay);
        ball.SetActive(false);
        cup.SetActive(false);
    }
            Debug.Log("Point marqué !");
            cup.SetActive(false); // fait disparaître le gobelet
        }
    }
}