using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseFillGameObjectControllByAnimator : MonoBehaviour
{
    public GameObject c1,c2,c3;
    public Animator anim;

    private void Start()
    {
        if (!anim)
            anim = GetComponent<Animator>();
    }

    public void CheeseReset()
    {
        anim.SetTrigger("Reset");
        AbleCheese();
        AbleCheese2();
        AbleCheese3();
    }

    public void DisableCheese1()
    {
        c1.SetActive(false);
    }

    public void AbleCheese()
    {
        c1.SetActive(true);
    }

    public void DisableCheese2()
    {
        c2.SetActive(false);
    }

    public void AbleCheese2()
    {
        c2.SetActive(true);
    }

    public void DisableCheese3()
    {
        c3.SetActive(false);
    }

    public void AbleCheese3()
    {
        c3.SetActive(true);
    }
}
