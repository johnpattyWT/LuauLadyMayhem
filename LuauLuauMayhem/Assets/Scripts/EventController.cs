using UnityEngine;
using System.Collections;

public class EventController : MonoBehaviour
{
    public Transform liftPlatform;
    public int killThreshold = 40;
    private bool _triggered;

    private void Update()
    {
        if (!_triggered && GameCore.Instance.globalKills >= killThreshold)
        {
            _triggered = true;
            StartCoroutine(MoveLift());
        }
    }

    private IEnumerator MoveLift()
    {
        Vector3 targetPos = liftPlatform.position + Vector3.up * 10f;
        while (Vector3.Distance(liftPlatform.position, targetPos) > 0.1f)
        {
            liftPlatform.position = Vector3.MoveTowards(liftPlatform.position, targetPos, Time.deltaTime * 2f);
            yield return null;
        }
    }
}