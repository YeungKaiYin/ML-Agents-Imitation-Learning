using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Test : MonoBehaviour
{
    public GameManager gm;
    public GameObject go,startPosition,endPosition;
    float moveSpeed=3;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator MovePos(GameObject endPosition)
    {
        float startTime = Time.time;
        float journeyLength = Vector3.Distance(startPosition.transform.position, endPosition.transform.position);

        while (gameObject.transform.position != endPosition.transform.position)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fracJourney = distCovered / journeyLength;
            gameObject.transform.position = Vector3.Lerp(startPosition.transform.position, endPosition.transform.position, fracJourney);
            yield return null;
        }
    }
    public void StartMovingPos()
    {
        StartCoroutine(MovePos(endPosition));
    }

        GameObject findChildFromParent(string parentName, string childNameToFind)
    {
        string childLocation = parentName + "/" + childNameToFind;
        GameObject childObject = GameObject.Find(childLocation);
        return childObject;
    }
}
