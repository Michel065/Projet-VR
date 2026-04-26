using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

/// <summary>
/// Gestionnaire central des quêtes — V2
/// Compatible avec le GameManager de l'équipe.
/// Attache sur un GameObject vide "QuestManager".
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public enum QuestState { Inactive, Active, Completed }

    [System.Serializable]
    public class Quest
    {
        public string id;
        public string title;
        public QuestState state = QuestState.Inactive;
        public UnityEvent onCompleted; // événement Unity déclenché à la complétion
    }

    [Header("Quêtes du After Lounge")]
    public List<Quest> quests = new List<Quest>
    {
        new Quest { id = "Q1_Conversation", title = "Reconstituer la vérité" },
        new Quest { id = "Q2_Unreliable",   title = "À qui faire confiance ?" },
        new Quest { id = "Q3_Memory",       title = "Mémoire fragmentée" },
        new Quest { id = "Q4_Observation",  title = "Trouver l'Immobile" },
        new Quest { id = "Q5_MovingObject", title = "L'objet insaisissable" },
    };

    [Header("Étage à débloquer quand toutes les quêtes sont faites")]
    public int floorToUnlock = 2; // correspond à floorIndex dans GameManager

    [Header("Nombre de quêtes requises pour débloquer l'étage")]
    public int requiredCompletions = 5;

    // Niveau de conscience du joueur (influence les dialogues)
    [HideInInspector] public int playerAwareness = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartQuest(string id)
    {
        Quest q = quests.Find(x => x.id == id);
        if (q != null && q.state == QuestState.Inactive)
        {
            q.state = QuestState.Active;
            Debug.Log($"[QuestManager] ▶ Quête démarrée : {q.title}");
        }
    }

    public void CompleteQuest(string id)
    {
        Quest q = quests.Find(x => x.id == id);
        if (q == null || q.state != QuestState.Active) return;

        q.state = QuestState.Completed;
        playerAwareness++;
        q.onCompleted?.Invoke();
        Debug.Log($"[QuestManager] ✓ Quête complétée : {q.title} (awareness={playerAwareness})");

        CheckAllCompleted();
    }

    void CheckAllCompleted()
    {
        int count = quests.FindAll(x => x.state == QuestState.Completed).Count;
        if (count >= requiredCompletions)
        {
            // Débloque l'étage suivant via le GameManager de l'équipe
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetFloorAccessible(floorToUnlock, true);
                Debug.Log($"[QuestManager] 🔓 Étage {floorToUnlock} débloqué !");
            }
        }
    }

    public bool IsActive(string id)
    {
        Quest q = quests.Find(x => x.id == id);
        return q != null && q.state == QuestState.Active;
    }

    public bool IsCompleted(string id)
    {
        Quest q = quests.Find(x => x.id == id);
        return q != null && q.state == QuestState.Completed;
    }
}