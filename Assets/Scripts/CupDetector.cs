using UnityEngine;

public class CupDetector : MonoBehaviour
{
    public GameObject cup; // glisse le gobelet parent ici dans l'Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            Debug.Log("Point marqué !");
            cup.SetActive(false); // fait disparaître le gobelet
        }
    }
}