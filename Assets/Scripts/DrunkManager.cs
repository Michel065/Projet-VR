using System.Collections;
using UnityEngine;

public class DrunkManager : MonoBehaviour
{
    public static DrunkManager instance;
    public int drunkLevel = 0;
    public int maxLevel = 3;
    private Coroutine resetCoroutine;

    [Header("Score")]
    public int score = 0;
    public int scoreToWin = 3;
    public NPCDialogue victoryDialogue;
    public int victoryNodeId = 10;

    void Awake()
    {
        instance = this;
    }

    public void AddPoint()
    {
        score++;
        Debug.Log("Score : " + score);

        if (score >= scoreToWin)
        {
            Debug.Log("Gagné !");
            if (victoryDialogue != null)
            {
             
                    Debug.Log("DialogueScreen : " + victoryDialogue.dialogueScreen);
                    Debug.Log("CurrentNode : " + victoryDialogue.currentNode);
                    victoryDialogue.dialogueDone = false;
                    victoryDialogue.dialogueFinished = false;
                    victoryDialogue.dialogueActive = true;
                    victoryDialogue.GoToNode(victoryNodeId);
                    victoryDialogue.dialogueScreen.RefreshDisplay();
          
            }
        }
    }

    public void Drink()
    {
        drunkLevel = Mathf.Min(drunkLevel + 1, maxLevel);
        Debug.Log("Niveau ivresse : " + drunkLevel);
        if (resetCoroutine != null) StopCoroutine(resetCoroutine);
        resetCoroutine = StartCoroutine(ResetAfterDelay());
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
        Debug.Log("Appliquer effets niveau " + drunkLevel);
        FindFirstObjectByType<DrunkCameraEffect>().SetLevel(drunkLevel);
    }
}