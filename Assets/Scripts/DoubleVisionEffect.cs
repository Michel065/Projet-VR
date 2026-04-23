using UnityEngine;

public class DoubleVisionEffect : MonoBehaviour
{
    public GameObject ghostCopy; // duplicate semi-transparent
    public float offsetAmount = 0f;
    public float oscillationSpeed = 1.2f;

    void Update()
    {
        if (ghostCopy != null && offsetAmount > 0)
        {
            float offset = Mathf.Sin(Time.time * oscillationSpeed) * offsetAmount;
            ghostCopy.transform.position = transform.position + new Vector3(offset, 0, 0);
        }
    }

    public void SetLevel(int level)
    {
        switch (level)
        {
            case 0: offsetAmount = 0f; if (ghostCopy != null) ghostCopy.SetActive(false); break;
            case 1: offsetAmount = 0f; if (ghostCopy != null) ghostCopy.SetActive(false); break;
            case 2: offsetAmount = 0.03f; if (ghostCopy != null) ghostCopy.SetActive(true); break;
            case 3: offsetAmount = 0.07f; if (ghostCopy != null) ghostCopy.SetActive(true); break;
        }
    }
}