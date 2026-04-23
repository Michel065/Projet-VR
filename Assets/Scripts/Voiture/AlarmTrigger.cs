using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AlarmTrigger : MonoBehaviour
{
    public string canetteTag = "Canette";
    public float alarmDuration = 10f;

    public bool trigger = false; // trigger manuel

    private AudioSource audioSource;
    private bool isPlaying = false;
    private float timer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPlaying) return;

        if (collision.gameObject.CompareTag(canetteTag))
        {
            StartAlarm();
        }
    }

    void Update()
    {
        // trigger manuel
        if (trigger)
        {
            trigger = false;
            if (!isPlaying)
                StartAlarm();
        }

        if (!isPlaying) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    void StartAlarm()
    {
        audioSource.Play();
        isPlaying = true;
        timer = alarmDuration;
    }
}