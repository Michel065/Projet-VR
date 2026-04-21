using UnityEngine;

public class DialogueTriggerSecondary : MonoBehaviour
{
    public DialogueScreenSecondary screen;

    private void OnTriggerEnter(Collider other)
    {
        screen?.OnPlayerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        screen?.OnPlayerExit(other);
    }
}