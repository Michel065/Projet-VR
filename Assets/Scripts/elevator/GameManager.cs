using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [System.Serializable]
    public class FloorAccess
    {
        public int floorIndex;
        public bool accessible = true;
    }

    public FloorAccess[] floors;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public bool IsFloorAccessible(int floorIndex)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorIndex == floorIndex)
                return floors[i].accessible;
        }

        return false;
    }

    public void SetFloorAccessible(int floorIndex, bool value)
    {
        for (int i = 0; i < floors.Length; i++)
        {
            if (floors[i].floorIndex == floorIndex)
            {
                floors[i].accessible = value;
                return;
            }
        }
    }
}