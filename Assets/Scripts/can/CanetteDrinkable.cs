using UnityEngine;

public class CanetteDrinkable : MonoBehaviour
{
    [Header("References")]
    public AlcoholEffectController alcoholEffect;

    [Header("Audio")]
    public AudioSource drinkAudio;
    public AudioClip drinkClip;
    public AudioClip emptyClip;

    [Header("Drink")]
    public int maxSips = 3;
    public float sipCooldown = 0.8f;
    public float sipStrength = 0.2f;
    public float sipDuration = 10f;

    private int sipCount = 0;
    private float lastSipTime = -999f;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("MainCamera"))
            return;

        if (Time.time - lastSipTime < sipCooldown)
            return;
        if (sipCount >= maxSips)
            return;

        Drink();
    }

    void Drink()
    {
        lastSipTime = Time.time;
        sipCount++;

        alcoholEffect.AddDose(sipStrength, sipDuration);

        if (drinkAudio != null)
        {
            if (sipCount == maxSips)
            {
                if (emptyClip != null)
                    drinkAudio.PlayOneShot(emptyClip);
            }
            else if (sipCount < maxSips)
            {
                if (drinkClip != null)
                    drinkAudio.PlayOneShot(drinkClip);
            }
        }

        if (sipCount >= maxSips)
        {
            enabled = false;
        }
    }
}