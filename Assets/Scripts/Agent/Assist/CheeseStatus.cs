using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseStatus : MonoBehaviour
{
    //public static CheeseStatus instance;
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    public int cheeseAmount = 0;

    public int CheeseCount()
    {
        return cheeseAmount;
    }

    public void CheeseGet()
    {
        cheeseAmount++;
    }

    public void CheeseSet(int i)
    {
        cheeseAmount = i;
    }
}