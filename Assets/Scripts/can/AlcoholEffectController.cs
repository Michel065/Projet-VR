using UnityEngine;

public class AlcoholEffectController : MonoBehaviour
{
    public Transform cameraOffset;

    public float currentInfluence = 0f;
    public float maxInfluence = 1.5f;
    public float recoverSpeed = 0.08f;

    public float pitchAmplitude = 1.5f;
    public float yawAmplitude = 0.8f;
    public float rollAmplitude = 4f;
    public float rotationSpeed = 1.2f;

    private Quaternion baseLocalRotation;
    private float effectTimer = 0f;

    void Start()
    {
        if (cameraOffset == null)
            cameraOffset = transform;

        baseLocalRotation = cameraOffset.localRotation;
    }

    void Update()
    {
        if (cameraOffset == null)
            return;

        if (effectTimer > 0f)
            effectTimer -= Time.deltaTime;
        else
            currentInfluence = Mathf.MoveTowards(currentInfluence, 0f, recoverSpeed * Time.deltaTime);

        float t = Time.time;

        float pitch = Mathf.Sin(t * rotationSpeed) * pitchAmplitude * currentInfluence;
        float yaw = Mathf.Cos(t * (rotationSpeed * 0.7f)) * yawAmplitude * currentInfluence;
        float roll = Mathf.Sin(t * (rotationSpeed * 1.3f)) * rollAmplitude * currentInfluence;

        cameraOffset.localRotation = baseLocalRotation * Quaternion.Euler(pitch, yaw, roll);
    }

    public void AddDose(float amount, float duration)
    {
        currentInfluence = Mathf.Clamp(currentInfluence + amount, 0f, maxInfluence);
        effectTimer += duration;
    }

    public void ResetEffect()
    {
        currentInfluence = 0f;
        effectTimer = 0f;

        if (cameraOffset != null)
            cameraOffset.localRotation = baseLocalRotation;
    }
}