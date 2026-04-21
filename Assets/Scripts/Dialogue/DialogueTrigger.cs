using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueScreen screen;

    private void OnTriggerEnter(Collider other)
    {
        screen?.OnPlayerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        screen?.OnPlayerExit(other);
    }
}
