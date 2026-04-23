using TMPro;
using UnityEngine;

public class DialogueScreenSecondary : MonoBehaviour
{
    [Header("References")]
    public Transform targetCamera;
    public SecondaryDialogue secondaryDialogue;

    [Header("Roots")]
    public GameObject questionRoot;
    public GameObject nextRoot;

    [Header("Texts")]
    public TextMeshPro questionText;

    [Header("Settings")]
    public bool followCamera = true;

    private void Start()
    {
        HideAll();
    }

    private void Update()
    {
        if (!followCamera)
            return;
    }

    public void HideAll()
    {
        SetVisible(questionRoot, false);
        SetVisible(nextRoot, false);
    }

    public void ShowNormal(DialogueNode node)
    {
        HideAll();

        if (node == null)
            return;

        SetVisible(questionRoot, true);
        SetVisible(nextRoot, true);

        if (questionText != null)
            questionText.text = node.message;
    }

    public void RefreshDisplay()
    {
        if (secondaryDialogue == null)
        {
            HideAll();
            return;
        }

        if (!secondaryDialogue.activeDialogue)
        {
            HideAll();
            return;
        }

        if (secondaryDialogue.currentNode == null)
        {
            HideAll();
            return;
        }
        ShowNormal(secondaryDialogue.currentNode);
    }

    public void OnPlayerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        RefreshDisplay();
    }

    public void OnPlayerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        HideAll();
    }

    public void NextTrigger()
    {
        if (secondaryDialogue == null)
            return;
        secondaryDialogue.NextTrigger();
        RefreshDisplay();
    }

    private void SetVisible(GameObject root, bool visible)
    {
        if (root == null)
            return;

        root.SetActive(visible);
    }
}