using TMPro;
using UnityEngine;

public class DialogueScreen : MonoBehaviour
{
    [Header("References")]
    public Transform targetCamera;
    public NPCDialogue npcDialogue;
    public SecondaryDialogue secondaryDialogue;
    public Collider triggerZone;

    [Header("Roots")]
    public GameObject pingRoot;
    public GameObject questionRoot;
    public GameObject responseVRoot;
    public GameObject responseFRoot;
    public GameObject nextRoot;

    [Header("Texts")]
    public TextMeshPro questionText;
    public TextMeshPro responseVText;
    public TextMeshPro responseFText;

    [Header("Settings")]
    public bool followCamera = true;

    private bool playerInside;

    private void Start()
    {
        HideAll();

        if (npcDialogue != null && npcDialogue.dialogueDone)
            return;

        ShowPing();
    }

    private void Update()
    {
        if (!followCamera)
            return;
    }

    public void HideAll()
    {
        SetVisible(pingRoot, false);
        SetVisible(questionRoot, false);
        SetVisible(responseVRoot, false);
        SetVisible(responseFRoot, false);
        SetVisible(nextRoot, false);
    }

    public void ShowPing()
    {
        if (npcDialogue != null && npcDialogue.dialogueDone)
        {
            HideAll();
            return;
        }

        HideAll();
        SetVisible(pingRoot, true);
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

    public void ShowChoice(DialogueNode node)
    {
        HideAll();

        if (node == null)
            return;

        SetVisible(questionRoot, true);
        SetVisible(responseVRoot, true);
        SetVisible(responseFRoot, true);

        if (questionText != null)
            questionText.text = node.message;

        if (node.choices != null && node.choices.Count > 0 && responseVText != null)
            responseVText.text = node.choices[0].choiceText;

        if (node.choices != null && node.choices.Count > 1 && responseFText != null)
            responseFText.text = node.choices[1].choiceText;
    }

    public void ShowNode(DialogueNode node)
    {
        if (node == null)
        {
            HideAll();
            return;
        }

        switch (node.nodeType)
        {
            case DialogueNodeType.Normal:
                ShowNormal(node);
                break;

            case DialogueNodeType.Choice:
                ShowChoice(node);
                break;

            case DialogueNodeType.WaitAction:
                ShowWaitAction(node);
                break;

            case DialogueNodeType.End:
                HideAll();
                break;

            case DialogueNodeType.Done:
                HideAll();
                break;

            default:
                HideAll();
                break;
        }
    }

    public void ShowWaitAction(DialogueNode node)
    {
        HideAll();

        if (node == null)
            return;

        SetVisible(questionRoot, true);

        if (questionText != null)
            questionText.text = node.message;
    }

    public void NextTrigger()
    {
        if (secondaryDialogue != null && secondaryDialogue.activeDialogue)
        {
            secondaryDialogue.NextTrigger();
            return;
        }

        if (npcDialogue == null)
            return;
        npcDialogue.NextTrigger();
    }

    public void ButtonTrigger(bool value)
    {
        if (secondaryDialogue != null && secondaryDialogue.activeDialogue)
            return;

        if (npcDialogue == null)
            return;

        npcDialogue.ButtonTrigger(value);
    }

    public void RefreshDisplay()
    {

        if (secondaryDialogue != null && secondaryDialogue.activeDialogue)
        {

            if (secondaryDialogue.currentNode != null)
            {
                ShowNormal(secondaryDialogue.currentNode);
            }
            else
            {
                HideAll();
            }
            return;
        }

        if (npcDialogue != null && npcDialogue.dialogueDone)
        {
            HideAll();
            return;
        }

        if (!playerInside)
        {
            ShowPing();
            return;
        }

        if (npcDialogue == null)
        {
            ShowPing();
            return;
        }

        if (npcDialogue.currentNode == null)
        {
            ShowPing();
            return;
        }

        ShowNode(npcDialogue.currentNode);
    }

    public void OnPlayerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;


        if (npcDialogue != null && npcDialogue.dialogueDone && npcDialogue.dialogueFinished)
        {
            HideAll();
            return;
        }

        playerInside = true;

        if (npcDialogue != null && !npcDialogue.dialogueActive) { 
            npcDialogue.Interact();
        }
        else { 
            RefreshDisplay();
        }
    }

    public void OnPlayerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        if (secondaryDialogue != null && secondaryDialogue.activeDialogue)
            secondaryDialogue.HideSecondary();

        if (npcDialogue != null && npcDialogue.dialogueDone)
        {
            HideAll();
            return;
        }

        if (npcDialogue != null && npcDialogue.dialogueFinished)
        {
            HideAll();
            return;
        }

        ShowPing();
    }

    private void SetVisible(GameObject root, bool visible)
    {
        if (root == null)
            return;

        root.SetActive(visible);
    }

    private void RotateTowardsCamera(Transform tr)
    {
        if (tr == null || targetCamera == null)
            return;

        Vector3 dir = targetCamera.position - tr.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.001f)
            return;

        tr.rotation = Quaternion.LookRotation(dir);
    }
}