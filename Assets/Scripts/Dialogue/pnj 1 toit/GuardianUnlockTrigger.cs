using UnityEngine;

public class GuardianUnlockTrigger : MonoBehaviour
{
    public GuardianDialogueUnlock guardianUnlock;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("[GuardianUnlockTrigger] ENTER " + other.name + " tag=" + other.tag);

        if (!other.CompareTag("Player"))
        {
            Debug.Log("[GuardianUnlockTrigger] STOP tag != Player");
            return;
        }

        if (guardianUnlock == null)
        {
            Debug.Log("[GuardianUnlockTrigger] STOP guardianUnlock null");
            return;
        }

        guardianUnlock.ValidateGuardianOnce(other);
    }
}