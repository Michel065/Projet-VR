using UnityEngine;
using System.Collections;
public class CupDetector : MonoBehaviour
{
    public GameObject cup; // Glisse le gobelet parent ici dans l'Inspector
    public float delay = 2.5f; // Délai avant que le gobelet disparaisse (en secondes)

    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // SetScored appelé IMMEDIATEMENT
            BallMissDetector missDetector = other.GetComponent<BallMissDetector>();
            if (missDetector != null)
            {
                missDetector.SetScored();
            }

            Debug.Log("Point marqué !");
            
            StartCoroutine(DisableCupAfterDelay(other.gameObject)); // délai seulement pour le gobelet
        }
    }
    IEnumerator DisableCupAfterDelay(GameObject ball)
    {
        yield return new WaitForSeconds(delay);
        ball.SetActive(false);
        Debug.Log("Balle à désactiver : " + ball.name);

        cup.SetActive(false);
    }
}