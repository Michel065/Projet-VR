using UnityEngine;

public class DrunkCameraEffect : MonoBehaviour
{
    public float swaySpeed = 0.5f;
    public float swayAmount = 0f; // commence à 0, on va le changer selon le niveau

    void Update()
    {
        if (swayAmount > 0)
        {
            float sway = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
            transform.localRotation = Quaternion.Euler(0, 0, sway);
        }
    }

    public void SetLevel(int level)
    {
        switch (level)
        {
            case 0: swayAmount = 0f; break;   // sobre
            case 1: swayAmount = 2f; break;   // léger
            case 2: swayAmount = 5f; break;   // moyen
            case 3: swayAmount = 10f; break;  // maximum
        }
    }
}