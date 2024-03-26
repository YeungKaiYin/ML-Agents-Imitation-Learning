using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseStandGameObjectControllByAnimator : MonoBehaviour
{
    public GameObject cheese;
    public List<GameObject> ms;

    public void DisableMouseStand()
    {
        foreach (GameObject go in ms)
            go.SetActive(false);
    }

    public void AbleMouseStand()
    {
        foreach (GameObject go in ms)
            go.SetActive(true);
    }

    public void DisableCheese()
    {
        cheese.SetActive(false);
    }

    public void AbleCheese()
    {
        cheese.SetActive(true);
    }
}
