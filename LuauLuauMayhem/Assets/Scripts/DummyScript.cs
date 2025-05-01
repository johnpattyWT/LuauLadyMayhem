using UnityEngine;
using System.Collections;

public class DummyScript : MonoBehaviour
{
    public Transform targetObject;
    public float targetY = -5f;
    public float liftSpeed = 2f;
    public GameObject[] prefabsToSpawn;
    public Transform[] spawnPoints;

    private bool isLifting = false;
    private bool liftTriggered = false;
    private int globalKillCount = 40;

    void Update()
    {
        // Trigger lift sequence if globalKillCount >= 40
        if (!liftTriggered && globalKillCount >= 40)
        {
            StartLiftSequence();
            liftTriggered = true;
        }
    }

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

        // For testing, just print a message instead of instantiating prefabs
        Debug.Log("Lift completed and prefabs should spawn");

        isLifting = false;
    }
}
