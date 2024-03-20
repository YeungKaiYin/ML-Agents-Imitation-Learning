using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBoxPos : MonoBehaviour
{
    public GameObject startPosition, endPosition;
    float moveSpeed = 3;

    IEnumerator MovePos(GameObject endPosition)
    {
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition.transform.position, endPosition.transform.position);

        while (gameObject.transform.position != endPosition.transform.position)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / journeyLength;
            gameObject.transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, fracJourney);
            gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, endPosition.GetComponent<RectTransform>().localScale, fracJourney);
            yield return null;
        }
    }
    public void StartMovingPos(GameObject go)
    {
        startPosition = gameObject;
        StartCoroutine(MovePos(go));
    }
}
