using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    public GameObject drinkingCupPrefab;
    public Transform drinkSpawnPoint;

    private GameObject currentDrinkingCup; // garde une référence au gobelet actuel

    void Awake()
    {
        instance = this;
    }

    public void SpawnDrinkingCup()
    {
        // Si un gobelet existe déjà on n'en spawn pas un nouveau
        if (currentDrinkingCup != null) return;

        currentDrinkingCup = Instantiate(drinkingCupPrefab, drinkSpawnPoint.position, Quaternion.identity);
    }

    public void ClearDrinkingCup()
    {
        currentDrinkingCup = null; // appelé quand le joueur a bu
    }
}