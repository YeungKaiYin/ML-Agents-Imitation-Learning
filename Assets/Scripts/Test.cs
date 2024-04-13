using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector3> curvePoints;
    public float count = 0;
    public float xDistance = 0.5f;

    void Start()
    {
        curvePoints = new List<Vector3>();
        curvePoints.Add(new Vector3((count), 0, 0));
        curvePoints.Add(new Vector3(count, 2, 0));
        count += xDistance;
        curvePoints.Add( new Vector3(count, -1, 0));
        count += xDistance;
        curvePoints.Add(new Vector3(count, 0, 0));
        count += xDistance;
    }

    private void FixedUpdate()
    {
        curvePoints.Add(new Vector3(count, UnityEngine.Random.Range(-10.0f, 10.0f), 0));
        count += xDistance;
        lineRenderer.positionCount = curvePoints.Count;
        lineRenderer.SetPositions(curvePoints.ToArray());
    }
}
