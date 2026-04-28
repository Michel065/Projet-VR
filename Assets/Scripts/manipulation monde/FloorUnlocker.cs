using UnityEngine;

public class FloorUnlocker : MonoBehaviour
{
    public NPCDialogue[] dialogues;   // liste des dialogues à surveiller
    public int requiredDone = 3;      // minimum pour débloquer

    public int floorToUnlock = 1;     // étage à débloquer
    public GameManager gameManager;

    private bool unlocked = false;

    void Update()
    {
        if (unlocked)
            return;

        int count = 0;

        for (int i = 0; i < dialogues.Length; i++)
        {
            if (dialogues[i] != null && dialogues[i].dialogueDone)
                count++;
        }

        if (count >= requiredDone)
        {
            gameManager.SetFloorAccessible(floorToUnlock, true);
            unlocked = true;
        }
    }
}