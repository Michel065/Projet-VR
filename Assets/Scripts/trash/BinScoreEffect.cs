using UnityEngine;

public class BinScoreEffect : MonoBehaviour
{
    public string canetteTag = "Canette";

    public AudioSource audioSource;
    public GameObject fireworkPrefab;
    public Transform spawnPoint;

    public float destroyEffectAfter = 5f;
    public bool trigger = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(canetteTag))
            return;

        PlayEffect();
    }

    private void Update()
    {
        if (trigger)
        {
            trigger = false;
            PlayEffect();
        }
    }

    private void PlayEffect()
    {
        if (audioSource != null)
            audioSource.Play();

        if (fireworkPrefab != null)
        {
            Vector3 pos = spawnPoint != null ? spawnPoint.position : transform.position;
            Quaternion rot = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

            GameObject fx = Instantiate(fireworkPrefab, pos, rot);
            Destroy(fx, destroyEffectAfter);
        }
    }
}