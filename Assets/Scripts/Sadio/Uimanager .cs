using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Gère les notifications de quête et les sous-titres de dialogue.
/// Attache sur un Canvas World Space ou Screen Space — Overlay.
/// </summary>
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI Quêtes")]
    public GameObject questNotifPanel;
    public TextMeshProUGUI questNotifText;

    [Header("UI Dialogues")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerName;

    [Header("UI Choix")]
    public GameObject choicePanel;
    public TextMeshProUGUI[] choiceButtons; // 2-3 boutons

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    // ── Notification quête ──
    public void ShowQuestNotification(string message, float duration = 3f)
    {
        StopCoroutine("HideNotif");
        questNotifPanel.SetActive(true);
        questNotifText.text = message;
        StartCoroutine(HideNotif(duration));
    }

    IEnumerator HideNotif(float t)
    {
        yield return new WaitForSeconds(t);
        questNotifPanel.SetActive(false);
    }

    // ── Dialogue ──
    public void ShowDialogue(string speaker, string text)
    {
        dialoguePanel.SetActive(true);
        speakerName.text  = speaker;
        dialogueText.text = text;
        choicePanel.SetActive(false);
    }

    public void ShowChoices(string[] choices)
    {
        choicePanel.SetActive(true);
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < choices.Length)
            {
                choiceButtons[i].transform.parent.gameObject.SetActive(true);
                choiceButtons[i].text = choices[i];
            }
            else
            {
                choiceButtons[i].transform.parent.gameObject.SetActive(false);
            }
        }
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }
}