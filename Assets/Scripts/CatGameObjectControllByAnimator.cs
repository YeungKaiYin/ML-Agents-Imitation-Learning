using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatGameObjectControllByAnimator : MonoBehaviour
{
    public GameObject e1, e2, e3,e4,ctw,cf,cb;
    public List<GameObject> catSeat;

    public void DisableCatSeat()
    {
        foreach (GameObject go in catSeat)
            go.SetActive(false);
    }

    public void AbleCatSeat()
    {
        foreach (GameObject go in catSeat)
            go.SetActive(true);
    }

    public void DisableCTW()
    {
        ctw.SetActive(false);
    }

    public void AbleCTW()
    {
        ctw.SetActive(true);
    }

    public void DisableCF()
    {
        cf.SetActive(false);
    }

    public void AbleCF()
    {
        cf.SetActive(true);
    }

    public void DisableCB()
    {
        cb.SetActive(false);
    }

    public void AbleCB()
    {
        cb.SetActive(true);
    }

    public void DisableAttackEffect1()
    {
        e1.SetActive(false);
    }

    public void AbleAttackEffect1()
    {
        e1.SetActive(true);
    }

    public void DisableAttackEffect2()
    {
        e2.SetActive(false);
    }

    public void AbleAttackEffect2()
    {
        e2.SetActive(true);
    }

    public void DisableAttackEffect3()
    {
        e3.SetActive(false);
    }

    public void AbleAttackEffect3()
    {
        e3.SetActive(true);
    }

    public void DisableDizzyEffect()
    {
        e4.SetActive(false);
    }

    public void AbleDizzykEffect()
    {
        e4.SetActive(true);
    }
}
