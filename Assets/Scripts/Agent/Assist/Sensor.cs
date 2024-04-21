using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{
    Vector2 contactPoint, outPoint=(new Vector2 (999,999));
    public Agent_Level2 al2;
    public Agent_Level3 al3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Wall")
        {
            contactPoint = collision.ClosestPoint(transform.position);
        }
            
        Debug.Log("Collision contact point: " + contactPoint);

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            contactPoint = outPoint;
            Debug.Log("Collision contact point Exit: " + contactPoint);
        }
;    }

    public Vector2 GetContactPoint()
    {
        return contactPoint;
    }

    void SendContactPoint()
    {
        if (al2 != null)
            al2.ReceiveContactPoint(contactPoint);
        if (al3 != null)
            al3.ReceiveContactPoint(contactPoint);
    }
}
