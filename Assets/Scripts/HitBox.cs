using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitBox : MonoBehaviour
{
    public GameObject enemy;
    Button bt;

    void Start()
    {
        bt = gameObject.GetComponent<Button>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
