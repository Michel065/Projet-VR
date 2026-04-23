using System.Collections;
using UnityEngine;

public class DrunkManager : MonoBehaviour
{
    public static DrunkManager instance;

    public int drunkLevel = 0; // 0, 1, 2, 3
    public int maxLevel = 3;
    private Coroutine resetCoroutine;

    void Awake()
    {
        instance = this;
    }

    public void Drink()
    {
        // Augmente le niveau sans dépasser le plafond
        drunkLevel = Mathf.Min(drunkLevel + 1, maxLevel);
        Debug.Log("Niveau ivresse : " + drunkLevel);

        // Relance le timer de 40 secondes à chaque verre
        if (resetCoroutine != null) StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(ResetAfterDelay());

        // Applique les effets selon le niveau
        ApplyEffects();
    }

    IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(40f);
        drunkLevel = 0;
        Debug.Log("Effets dissipés");
        ApplyEffects();
    }

    void ApplyEffects()
    {
        // On branchera les effets visuels ici
        Debug.Log("Appliquer effets niveau " + drunkLevel);
        FindFirstObjectByType<DrunkCameraEffect>().SetLevel(drunkLevel);

    }
}