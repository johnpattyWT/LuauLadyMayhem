using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelOne : MonoBehaviour
{
    public Transform targetObject;
    public float targetY = -5f;
    public float liftSpeed = 2f;

    public GameObject[] prefabsToSpawn;
    public Transform[] spawnPoints;

    private bool isLifting = false;

    public void StartLiftSequence()
    {
        Debug.Log("StartLiftSequence called");
        if (!isLifting)
        {
            StartCoroutine(LiftObjectRoutine());
        }
    }


    private IEnumerator LiftObjectRoutine()
    {
        isLifting = true;

        // Smoothly move object
        Vector3 startPosition = targetObject.position;
        Vector3 endPosition = new Vector3(startPosition.x, targetY, startPosition.z);

        while (Vector3.Distance(targetObject.position, endPosition) > 0.01f)
        {
            targetObject.position = Vector3.Lerp(targetObject.position, endPosition, Time.deltaTime * liftSpeed);
            yield return null;
        }

        targetObject.position = endPosition;

        // Convert prefabs array to a list and shuffle it
        List<GameObject> availablePrefabs = new List<GameObject>(prefabsToSpawn);
        ShuffleList(availablePrefabs);

        // Spawn one unique prefab at each spawn point
        for (int i = 0; i < spawnPoints.Length && i < availablePrefabs.Count; i++)
        {
            Transform point = spawnPoints[i];
            GameObject prefab = availablePrefabs[i];
            if (point != null && prefab != null)
            {
                Instantiate(prefab, point.position, Quaternion.identity);
            }
        }

        isLifting = false;
    }

    // Fisher–Yates shuffle
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
