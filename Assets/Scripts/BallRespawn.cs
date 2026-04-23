using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallRespawn : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public float respawnDelay = 2.5f;
    public float spawnRadius = 0.2f;

    private XRGrabInteractable grabInteractable;
    private bool isHeld = false;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args) { isHeld = true; }

    void OnRelease(SelectExitEventArgs args)
    {
        isHeld = false;
        StartCoroutine(SpawnNewBall());
    }

    IEnumerator SpawnNewBall()
    {
        yield return new WaitForSeconds(respawnDelay);

        Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, spawnRadius);
        bool ballAlreadyThere = false;
        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Ball"))
            {
                ballAlreadyThere = true;
                break;
            }
        }

        if (!isHeld && !ballAlreadyThere)
        {
            Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}