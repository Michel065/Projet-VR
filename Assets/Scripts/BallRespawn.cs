using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class BallRespawn : MonoBehaviour
{
    public GameObject ballPrefab; // ta balle en prefab
    public Transform spawnPoint;
    public float respawnDelay = 2.5f; // délai avant nouvelle balle

    private XRGrabInteractable grabInteractable;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnRelease(SelectExitEventArgs args)
    {
        StartCoroutine(SpawnNewBall());
    }

    IEnumerator SpawnNewBall()
    {
        yield return new WaitForSeconds(respawnDelay);
        Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
    }
}