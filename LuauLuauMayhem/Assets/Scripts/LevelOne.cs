using UnityEngine;
using System.Collections;

public class LevelOne : MonoBehaviour
{
    public Transform targetObject; // Object to lift (will bring children too)
    public float targetY = -5f; // Target Y position
    public float liftSpeed = 2f; // Speed of lift

   

    public GameObject[] prefabsToSpawn; // Prefabs to instantiate
    public Transform spawnPoint; // Where to spawn them (optional)

    private bool isLifting = false;

    public void StartLiftSequence()
    {
        if (!isLifting)
        {
            StartCoroutine(LiftObjectRoutine());
        }
    }

    private IEnumerator LiftObjectRoutine()
    {
        isLifting = true;

        // Smooth movement
        Vector3 startPosition = targetObject.position;
        Vector3 endPosition = new Vector3(startPosition.x, targetY, startPosition.z);

        while (Vector3.Distance(targetObject.position, endPosition) > 0.01f)
        {
            targetObject.position = Vector3.Lerp(targetObject.position, endPosition, Time.deltaTime * liftSpeed);
            yield return null;
        }

        targetObject.position = endPosition;

        // Instantiate prefabs
        foreach (GameObject prefab in prefabsToSpawn)
        {
            Vector3 spawnPos = spawnPoint ? spawnPoint.position : targetObject.position + Vector3.up * 2;
            Instantiate(prefab, spawnPos, Quaternion.identity);
        }

       

        isLifting = false;
    }
}
