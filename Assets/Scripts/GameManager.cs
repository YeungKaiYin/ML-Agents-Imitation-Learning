using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    int[] e_index=new int[4];
    public List<GameObject> enemy = new List<GameObject>();
    List<GameObject> e_marker = new List<GameObject>();
    List<GameObject> e_hpSlider = new List<GameObject>();
    List<GameObject> e_touSlider = new List<GameObject>();
    List<float> e_hp = new List<float>();
    List<float> e_tou = new List<float>();
    List<GameObject> e_clickBox = new List<GameObject>();
    List<Status> esList = new List<Status>();

    public List<GameObject> player = new List<GameObject>();
    List<GameObject> p_marker = new List<GameObject>();
    List<GameObject> p_hpSlider = new List<GameObject>();
    List<GameObject> p_touSlider = new List<GameObject>();
    List<float> p_hp = new List<float>();
    List<float> p_tou = new List<float>();
    List<GameObject> p_clickBox = new List<GameObject>();
    List<Status> psmList = new List<Status>();
    public GameObject battlePosLeft, battlePosRight,pPos,ePos1, ePos2, ePos3, ePos4;
    public GameObject battlePosLeftUp, battlePosRightUp, pPosUp, ePos1Up, ePos2Up, ePos3Up, ePos4Up;
    List<GameObject> ePosBox = new List<GameObject>();
    List<GameObject> ePosBoxUp = new List<GameObject>();
    List<GameObject> pAttackCollider = new List<GameObject>();
    List<GameObject> eAttackCollider = new List<GameObject>();
    List<GameObject> eMissCollider = new List<GameObject>();
    List<GameObject> eGetHitCollider = new List<GameObject>();

    string animName;
    //int attackFrom,attackTo;
    int buffStack;
    float dmg, maxDmg, minDmg,heal;
    float t_dmg;
    //bool haveAction=false;
    //bool aoe=false;
    bool knockup = false;
    bool jumpTf = false;
    //bool focus = false;
    int amountOfEnemyDefeat = 0;
    String mode="";
    String anim="";

    public TurnManager tm;
    public ButtonManager bm;

    GameObject findChildFromParent(string parentName, string childNameToFind)
    {
        string childLocation = parentName + "/" + childNameToFind;
        GameObject childObject = GameObject.Find(childLocation);
        return childObject;
    }

    void Start()
    {
        ePosBox.Add(ePos1);
        ePosBox.Add(ePos2);
        ePosBox.Add(ePos3);
        ePosBox.Add(ePos4);
        ePosBoxUp.Add(ePos1Up);
        ePosBoxUp.Add(ePos2Up);
        ePosBoxUp.Add(ePos3Up);
        ePosBoxUp.Add(ePos4Up);
        if (GameObject.FindGameObjectWithTag("TurnManager"))
            tm = GameObject.FindGameObjectWithTag("TurnManager").GetComponent<TurnManager>();

        int pi = 0;
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player.Add(GameObject.FindGameObjectWithTag("Player"));
            p_marker.Add(findChildFromParent(player[pi].name, "Marker"));
            p_clickBox.Add(findChildFromParent(player[pi].name, "ClickBox"));
            p_hpSlider.Add(findChildFromParent(player[pi].name, "Health"));
            p_touSlider.Add(findChildFromParent(player[pi].name, "Toughness"));
            pAttackCollider.Add(findChildFromParent(player[pi].name, "AttackCollider"));
            pAttackCollider[pi].SetActive(false);
            //e_hp.Add(PlayerPrefs.GetFloat("e1"));
            p_clickBox[pi].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(PlayerGetClick);
            psmList.Add(player[pi].GetComponent<Status>());
            p_hp.Add(psmList[pi].GetHp());
            p_tou.Add(psmList[pi].GetTou());
            pi++;
        }

        foreach (GameObject ob in p_marker)
        {
            ob.SetActive(false);
        }
        foreach (GameObject ob in p_clickBox)
        {
            ob.SetActive(false);
        }

        foreach (GameObject ob in e_marker)
        {
            ob.SetActive(false);
        }
        foreach (GameObject ob in e_clickBox)
        {
            ob.SetActive(false);
        }
        //GetEnemy();
        e_marker = new List<GameObject>();
        e_clickBox = new List<GameObject>();
        e_hpSlider = new List<GameObject>();
        e_touSlider = new List<GameObject>();
        esList = new List<Status>();
        e_hp = new List<float>();
        e_tou = new List<float>();
        int lc = 0;
        //marker.Clear();
        for(int i=1;i<5;i++)
        {
            if (GameObject.FindGameObjectWithTag("Enemy" + i))
            {
                enemy.Add(GameObject.FindGameObjectWithTag("Enemy" + i));
            }
        }
        
        for (int i = 0; i < enemy.Count; i++)
        {
            e_index[i] = i;
            e_marker.Add(enemy[lc].transform.Find("Marker").gameObject);
            e_clickBox.Add(enemy[lc].transform.Find("ClickBox").gameObject);
            e_hpSlider.Add(enemy[lc].transform.Find("Health").gameObject);
            e_touSlider.Add(enemy[lc].transform.Find("Toughness").gameObject);
            eAttackCollider.Add(enemy[lc].transform.Find("AttackCollider").gameObject);
            eAttackCollider[lc].SetActive(false);
            eMissCollider.Add(enemy[lc].transform.Find("MissCollider").gameObject);
            eMissCollider[lc].SetActive(false);
            eGetHitCollider.Add(enemy[lc].transform.Find("GetHitCollider").gameObject);
            eGetHitCollider[lc].SetActive(false);
            //e_hp.Add(PlayerPrefs.GetFloat("e1"));
            switch (lc)
            {
                case 0:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy1GetClick);
                    break;
                case 1:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy2GetClick);
                    break;
                case 2:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy3GetClick);
                    break;
                case 3:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy4GetClick);
                    break;
            }

            esList.Add(enemy[lc].GetComponent<Status>());
            e_hp.Add(esList[lc].GetHp());
            e_tou.Add(esList[lc].GetTou());
            //enemy[lc].GetComponent<Collider2D>().enabled = false;
            lc++;
        }
        PositionUpdate();
        tm.TurnManagerStrat();
        //PullEnemy(3);
    }

    void Update()
    {
        
    }

    public void GetEnemy()
    {
        e_marker = new List<GameObject>();
        e_clickBox = new List<GameObject>();
        e_hpSlider = new List<GameObject>();
        e_touSlider = new List<GameObject>();
        esList = new List<Status>();
        e_hp = new List<float>();
        e_tou = new List<float>();
        int lc = 0;
        //marker.Clear();
        for(int i=0; i < enemy.Count;i++)
        {
            e_index[i] = i;
            e_marker.Add(enemy[lc].transform.Find("Marker").gameObject);
            e_clickBox.Add(enemy[lc].transform.Find("ClickBox").gameObject);
            e_hpSlider.Add(enemy[lc].transform.Find("Health").gameObject);
            e_touSlider.Add(enemy[lc].transform.Find("Toughness").gameObject);
            eAttackCollider.Add(enemy[lc].transform.Find("AttackCollider").gameObject);
            eAttackCollider[lc].SetActive(false);
            eMissCollider.Add(enemy[lc].transform.Find("MissCollider").gameObject);
            eMissCollider[lc].SetActive(false);
            eGetHitCollider.Add(enemy[lc].transform.Find("GetHitCollider").gameObject);
            eGetHitCollider[lc].SetActive(false);
            //e_hp.Add(PlayerPrefs.GetFloat("e1"));
            switch (lc)
            {
                case 0:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy1GetClick);
                    break;
                case 1:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy2GetClick);
                    break;
                case 2:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy3GetClick);
                    break;
                case 3:
                    e_clickBox[lc].GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Enemy4GetClick);
                    break;
            }
            
            esList.Add(enemy[lc].GetComponent<Status>());
            e_hp.Add(esList[lc].GetHp());
            e_tou.Add(esList[lc].GetTou());
            //enemy[lc].GetComponent<Collider2D>().enabled = false;
            lc++;
        }
    }

    int lastMark = 0;
    bool magicTF=false;

    void MagicAim()
    {
        magicTF = true;
        if (p_marker[0].activeInHierarchy)
        {
            foreach (GameObject a in p_clickBox)
                a.SetActive(false);
            p_marker[0].SetActive(false);
        }

        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        e_marker[lastMark].SetActive(false);
        foreach (GameObject a in e_clickBox)
            a.SetActive(true);
        try
        {
            e_marker[lastMark].SetActive(true);
        }
        catch
        {
            lastMark = 0;
            e_marker[0].SetActive(true);
        }
    }

    void AimTheTarget()
    {
        if (p_marker[0].activeInHierarchy)
        {
            foreach (GameObject a in p_clickBox)
                a.SetActive(false);
            p_marker[0].SetActive(false);
        }

        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        e_marker[lastMark].SetActive(false);
        foreach (GameObject a in e_clickBox)
            a.SetActive(true);
        foreach (GameObject a in e_marker)
            a.SetActive(false);
    }

    public void AimTheTarget_False()
    {
        if(p_marker[0].activeInHierarchy)
        {
            foreach (GameObject a in p_clickBox)
                a.SetActive(false);
            p_marker[0].SetActive(false);
        }
        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        foreach (GameObject a in e_marker)
            a.SetActive(false);
    }

    void AimTheTarget(int area)
    {
        //int lm = Array.IndexOf(e_index, lastMark);
        if (p_marker[0].activeInHierarchy)
        {
            foreach (GameObject a in p_clickBox)
                a.SetActive(false);
            p_marker[0].SetActive(false);
        }

        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        foreach (GameObject a in e_marker)
            a.SetActive(false);
        for (int i=0;i<area;i++)
        {
            e_clickBox[e_index[i]].SetActive(true);
        }
        try
        {
            e_marker[lastMark].SetActive(true);
        }
        catch
        {
            lastMark = 0;
            e_marker[0].SetActive(true);
        }
    }

    void AimTheTarget_Release(int area)
    {
        int lm = Array.IndexOf(e_index, lastMark);
        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        foreach (GameObject a in e_marker)
            a.SetActive(false);
        //e_marker[lastMark].SetActive(false);
        for (int i = 0; i < area; i++)
        {
            e_clickBox[e_index[i]].SetActive(true);
        }

        if (lm <= area - 1)
            e_marker[lastMark].SetActive(true);
        else
        {
            lastMark = area - 1;
            e_marker[e_index[lastMark]].SetActive(true);
        }
    }

    void AimSelf()
    {
        foreach (GameObject a in p_clickBox)
            a.SetActive(false);
        p_marker[0].SetActive(false);
        foreach (GameObject a in p_clickBox)
            a.SetActive(true);
        p_marker[0].SetActive(true);
    }

    public void Enemy1GetClick()
    {
        lastMark = 0;
        if (e_marker[lastMark].activeSelf==true)
            Action(lastMark);
        else
        {
            foreach (GameObject a in e_marker)
                a.SetActive(false);
            e_marker[lastMark].SetActive(true);
        }
    }

    public void Enemy2GetClick()
    {
        lastMark = 1;
        if (e_marker[lastMark].activeSelf == true)
            Action(lastMark);
        else
        {
            foreach (GameObject a in e_marker)
                a.SetActive(false);
            e_marker[lastMark].SetActive(true);
        }
    }

    public void Enemy3GetClick()
    {
        lastMark = 2;
        if (e_marker[lastMark].activeSelf == true)
            Action(lastMark);
        else
        {
            foreach (GameObject a in e_marker)
                a.SetActive(false);
            e_marker[lastMark].SetActive(true);
        }
    }

    public void Enemy4GetClick()
    {
        lastMark = 3;
        if (e_marker[lastMark].activeSelf==true)
            Action(lastMark);
        else
        {
            foreach (GameObject a in e_marker)
                a.SetActive(false);
            e_marker[lastMark].SetActive(true);
        }
    }

    public void PlayerGetClick()
    {
        lastMark = 0;
        if (p_marker[lastMark].activeSelf == true)
            ActionToSelf(lastMark);
        else
        {
            foreach (GameObject a in p_marker)
                a.SetActive(false);
            p_marker[lastMark].SetActive(true);
            p_marker[lastMark].GetComponent<Image>().color = new(10,195,17,255);
        }
    }

    //public void 

    //check ¼Ä§Ú¡A½d³ò¡Ahp¶Ë®`(dmg)¡A¶´©Ê¶Ë®`(t_dmg)¡AªvÀø(heal)¡Abuff¡Adebuff
    void Action(int markerIndex)
    {
            
        if(psmList[0].GetAnimator())
        {
            if (anim.Contains("Cheese"))
                psmList[0].CheeseUse();

            if (anim.Contains("Mouse"))
            {
                psmList[0].SetAnimator_Trigger("Action");
                psmList[0].SetAnimator_Bool(anim, true);
            }
            else
                psmList[0].SetAnimator_Trigger("Action");
            Debug.Log(anim);
        }

        if(psmList[0].IsItGrounded())
        {
            player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            player[0].GetComponent<Transform>().position = battlePosLeftUp.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        //player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        if (esList[markerIndex].IsItGrounded())
            enemy[markerIndex].GetComponent<Transform>().position = battlePosRight.transform.position;
        else
            enemy[markerIndex].GetComponent<Transform>().position = battlePosRightUp.transform.position;
        //do the action
        //player[0].GetComponent<Animator>().Play(animName);
        foreach (GameObject a in e_marker)
            a.SetActive(false);
        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        foreach (GameObject a in p_marker)
            a.SetActive(false);
        foreach (GameObject a in p_clickBox)
            a.SetActive(false);
        // do the attack animation...
        //
        if (magicTF)
        {
            P_HitSuccess();
        }
        else
        {
            eGetHitCollider[lastMark].SetActive(true);
            eMissCollider[lastMark].SetActive(true);
            pAttackCollider[0].SetActive(true);
        }
    }

    void AgentAction(int markerIndex)
    {
        if (markerIndex > enemy.Count - 1)
        {
            dmg = 0;
            t_dmg = 0;
            Invoke("GM_AgentTurnEnd", 1f);
            return;
        }

        if (psmList[0].IsItGrounded())
        {
            player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            player[0].GetComponent<Transform>().position = battlePosLeftUp.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        //player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        if (esList[markerIndex].IsItGrounded())
            enemy[markerIndex].GetComponent<Transform>().position = battlePosRight.transform.position;
        else
            enemy[markerIndex].GetComponent<Transform>().position = battlePosRightUp.transform.position;
        //do the action
        //player[0].GetComponent<Animator>().Play(animName);
        foreach (GameObject a in e_marker)
            a.SetActive(false);
        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        foreach (GameObject a in p_marker)
            a.SetActive(false);
        foreach (GameObject a in p_clickBox)
            a.SetActive(false);
        // do the attack animation...
        //
        if (magicTF)
        {
            P_HitSuccess();
        }
        else
        {
            eGetHitCollider[lastMark].SetActive(true);
            eMissCollider[lastMark].SetActive(true);
            pAttackCollider[0].SetActive(true);
        }
    }

    void ActionToSelf(int markerIndex)
    {
        if (psmList[0].IsItGrounded())
        {
            player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        else
        {
            player[0].GetComponent<Transform>().position = battlePosLeftUp.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        }
        foreach (GameObject a in e_marker)
            a.SetActive(false);
        foreach (GameObject a in e_clickBox)
            a.SetActive(false);
        foreach (GameObject a in p_marker)
            a.SetActive(false);
        foreach (GameObject a in p_clickBox)
            a.SetActive(false);
        if (buffStack > 0)
        {
            psmList[0].FocusAdd(buffStack);
            //do some Focus anim
        }

        float pConvert = 1 / psmList[0].GetHp();
        float pConvertTou = 1 / psmList[0].GetTou();
        if (heal > 0)
        {
            p_hp[markerIndex] += heal;
            heal = 0;
            p_hpSlider[0].GetComponent<Slider>().value = pConvert * p_hp[0];
        }

        Invoke("GM_SelfTurnEnd", 1f);
    }

    void JumpAttack()
    {

    }

    int eTurnIndex;
    void eAction(int markerIndex)
    {
        //Debug.Log("eAction"+" "+markerIndex);
        if(!esList[markerIndex].MustHit())
        {
            if (psmList[0].IsItGrounded())
            {
                Debug.Log("eAction" + " " + markerIndex);
                player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
                player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }
            else
            {
                Debug.Log("eAction" + " " + markerIndex);
                player[0].GetComponent<Transform>().position = battlePosLeftUp.transform.position;
                player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            }
            //player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            if (esList[markerIndex].IsItGrounded())
                enemy[markerIndex].GetComponent<Transform>().position = battlePosRight.transform.position;
            else
                enemy[markerIndex].GetComponent<Transform>().position = battlePosRightUp.transform.position;
        }
        else
        {
            player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            enemy[markerIndex].GetComponent<Transform>().position = battlePosRight.transform.position;
        }
        
        //do the action
        //player[0].GetComponent<Animator>().Play(animName);

        // do the attack animation...
        //
        //enemy[markerIndex].GetComponent<Collider2D>().enabled = true;
        eAttackCollider[markerIndex].SetActive(true);
        eTurnIndex = markerIndex;
    }

    void eLanding(int markerIndex)
    {
        esList[markerIndex].IsItGrounded(true);
        enemy[markerIndex].GetComponent<Transform>().position = battlePosRight.transform.position;
        eTurnIndex = markerIndex;
    }

    public void GM_pColliderHit()
    {
        P_HitSuccess();
    }

    void P_HitSuccess()
    {
        if (esList[lastMark].GetAnimator())
        {
            esList[lastMark].SetAnimator_Trigger("Cat_GetHit");
            esList[lastMark].SetAnimator_Bool("Cat_Idle", false);
        }
            
        pAttackCollider[0].SetActive(false);
        //Debug.Log(pAttackCollider[0]);
        float convert = 1 / esList[lastMark].GetHp();
        float convertTou = 1 / esList[lastMark].GetTou();
        float pConvert = 1 / psmList[0].GetHp();
        float pConvertTou = 1 / psmList[0].GetTou();

        int focusStack = psmList[0].FocusStackRelease();
        t_dmg = t_dmg + focusStack * 2f;
        dmg = dmg + focusStack * 2f;
        e_tou[lastMark] = e_touSlider[lastMark].GetComponent<Slider>().value/convertTou;

        if (t_dmg >= 0)
            e_tou[lastMark] -= t_dmg;
        t_dmg = 0;
        e_touSlider[lastMark].GetComponent<Slider>().value = convertTou * e_tou[lastMark];
        if (e_touSlider[lastMark].GetComponent<Slider>().value <= 0)
        {
            esList[lastMark].IsItBreaked(true);
            tm.Break(enemy[lastMark]);
        }
            
        //knock up and jump
        if (e_touSlider[lastMark].GetComponent<Slider>().value <= 0 && knockup)
        {
            enemy[lastMark].GetComponent<Transform>().position = battlePosRightUp.transform.position;
            esList[lastMark].IsItGrounded(false);
        }
        if (!psmList[0].IsItGrounded())
        {
            player[0].GetComponent<Transform>().position = battlePosLeftUp.transform.position;
        }

        //knock down and fall
        if (!knockup && !jumpTf && !psmList[0].IsItGrounded()&&mode=="Upper")
        {
            enemy[lastMark].GetComponent<Transform>().position = battlePosRight.transform.position;
            esList[lastMark].IsItGrounded(true);
            player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            psmList[0].IsItGrounded(true);
            dmg += esList[lastMark].GetHp() * 0.1f;
        }

        Debug.Log("Player dmg: " + dmg);
        Debug.Log("e_hp " + e_hp[lastMark]);
        if (e_tou[lastMark] > 0 && dmg >= 0)
            e_hp[lastMark] -= dmg * 0.7f;
        else if (dmg >= 0 && !esList[lastMark].IsItGrounded())
            e_hp[lastMark] -= dmg * 1.5f;
        else if (dmg >= 0)
            e_hp[lastMark] -= dmg;
        p_tou[0] += psmList[0].GetTou() / (5 - focusStack);
        if (p_tou[0] > psmList[0].GetTou())
            p_tou[0] = psmList[0].GetTou();

        dmg = 0;
        p_touSlider[0].GetComponent<Slider>().value = pConvertTou * p_tou[0];
        e_hpSlider[lastMark].GetComponent<Slider>().value = convert * e_hp[lastMark];

        if (e_hp[lastMark] <= 0)
        {
            if (esList[lastMark].GetAnimator())
            {
                StartCoroutine(CatLoseAnimation());
            }
                
            amountOfEnemyDefeat++;
            esList[lastMark].IsItDying(true);
            tm.Dying(enemy[lastMark]);
            if(amountOfEnemyDefeat>=enemy.Count)
                try
                {
                    GameReset();
                    TurnBasedAgent tba = player[0].GetComponent<TurnBasedAgent>();
                    tba.AgentVictory();
                }
                catch (Exception e) { Debug.Log(e); }
            try 
            {
                e_hpSlider[lastMark].SetActive(false);
                e_touSlider[lastMark].SetActive(false);
                //e_clickBox.RemoveAt(lastMark);
                //e_marker.RemoveAt(lastMark);
            }
            catch(Exception e) { Debug.Log(e); }
            
        }
        
        //if (!esList[0].IsItGrounded())
        //    esList[0].KnockUpIncrease();

        if (jumpTf)
            psmList[0].IsItGrounded(false);
        else if(mode=="Upper")
            psmList[0].IsItGrounded(true);
        jumpTf = false;

        if (pull&&!esList[lastMark].IsItBreaked())
        {
            PullEnemy(lastMark);
            pull = false;
        }
        knockup = false;
        Invoke("GM_TurnEnd", 0.3f);
    }

    IEnumerator CatLoseAnimation()
    {
        yield return new WaitForSeconds(.5f);
        if (anim.Contains("Kibble"))
            esList[lastMark].SetAnimator_Trigger("Cat_Eat_Kibble");
        else if (anim.Contains("Ball"))
            esList[lastMark].SetAnimator_Trigger("Cat_Chase_Ball");
        else if (anim.Contains("Wand"))
            esList[lastMark].SetAnimator_Trigger("Cat_Play_CatTesaserWand");
    }

    public void GM_eColliderHit()
    {
        E_HitSuccess();
    }

    void E_HitSuccess()
    {
        eAttackCollider[eTurnIndex].SetActive(false);
        float convert = 1 / psmList[0].GetHp();
        float convertTou = 1 / psmList[0].GetTou();
        Debug.Log("Enemy dmg: " + dmg);
        if (dmg >= 0)
            if (p_tou[0] > 0)
            {
                float touBuffer = p_tou[0];
                if (dmg > psmList[0].GetTou() * 0.3)
                {
                    dmg = psmList[0].GetTou() * 0.3f;
                    p_tou[0] -= dmg;
                    dmg -= touBuffer;
                }
                else
                {
                    p_tou[0] -= dmg;
                    dmg -= touBuffer;
                }
            }
        p_hp[0] -= dmg;
        dmg = 0;
        p_hpSlider[0].GetComponent<Slider>().value = convert * p_hp[0];
        //Debug.Log(e_hp[markerIndex].GetComponent<Slider>().value);
        if (t_dmg >= 0)
            p_tou[0] -= t_dmg;
        t_dmg = 0;
        p_touSlider[0].GetComponent<Slider>().value = convertTou * p_tou[0];

        if (p_hp[0] <= 0)
        {
            psmList[0].IsItDying(true);
            tm.Dying(player[0]);
            try
            {
                GameReset();
                TurnBasedAgent tba = player[0].GetComponent<TurnBasedAgent>();
                tba.AgentDefeat();
            }
            catch(Exception e) { Debug.Log(e); }
        }
        Invoke("GM_eTurnEnd", 1f);
    }

    public void GM_pColliderMiss()
    {
        if (mode!="Lower" && psmList[0].IsItGrounded())
        {
            pAttackCollider[0].SetActive(false);
            dmg = 0;
            t_dmg = 0;
            Invoke("GM_TurnEnd", 1f);
        }
        else if(mode=="Lower" && psmList[0].IsItGrounded())
        {
            player[0].GetComponent<Transform>().position = battlePosLeftUp.transform.position;
            //psmList[0].IsItGrounded(false);
        }

        if (!psmList[0].IsItGrounded() && mode == "Lower")
        {
            pAttackCollider[0].SetActive(false);
            dmg = 0;
            t_dmg = 0;
            Invoke("GM_TurnEnd", 1f);
        }
        else if(!psmList[0].IsItGrounded() && mode == "Middle")
        {
            pAttackCollider[0].SetActive(false);
            dmg = 0;
            t_dmg = 0;
            Invoke("GM_TurnEnd", 1f);
        }
        else if(mode == "Upper" && !psmList[0].IsItGrounded())
        {
            player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            //psmList[0].IsItGrounded(true);
        }
        knockup = false;
        jumpTf = false;
    }

    public void GM_eColliderMiss()
    {
        eAttackCollider[eTurnIndex].SetActive(false);
        dmg = 0;
        t_dmg = 0;
        Invoke("GM_eTurnEnd", 1f);
    }

    public void GM_TurnEnd()
    {
        if (esList[lastMark].GetAnimator())
            esList[lastMark].SetAnimator_Bool("Cat_Idle", true);

        eGetHitCollider[lastMark].SetActive(false);
        eMissCollider[lastMark].SetActive(false);

        magicTF = false;

        if (psmList[0].IsItGrounded())
            player[0].GetComponent<Transform>().position = pPos.transform.position;
        else
            player[0].GetComponent<Transform>().position = pPosUp.transform.position;
        player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //enemy[lastMark].GetComponent<Collider2D>().enabled = false;
        if(esList[lastMark].IsItGrounded())
            enemy[lastMark].GetComponent<Transform>().position = ePosBox[e_index[lastMark]].transform.position;
        else
            enemy[lastMark].GetComponent<Transform>().position = ePosBoxUp[e_index[lastMark]].transform.position;
        tm.TurnEnd();
    }

    public void GM_AgentTurnEnd()
    {
        tm.TurnEnd();
    }

    public void GM_eTurnEnd()
    {
        if (esList[lastMark].GetAnimator())
            esList[lastMark].SetAnimator_Bool("Cat_Idle", true);

        if (psmList[0].IsItGrounded())
            player[0].GetComponent<Transform>().position = pPos.transform.position;
        else
            player[0].GetComponent<Transform>().position = pPosUp.transform.position;
        player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        //enemy[eTurnIndex].GetComponent<Collider2D>().enabled = false;
        if (esList[eTurnIndex].IsItGrounded())
            enemy[eTurnIndex].GetComponent<Transform>().position = ePosBox[e_index[eTurnIndex]].transform.position;
        else
            enemy[eTurnIndex].GetComponent<Transform>().position = ePosBoxUp[e_index[eTurnIndex]].transform.position;
        tm.TurnEnd();
    }

    public void GM_SelfTurnEnd()
    {
        if (psmList[0].IsItGrounded())
            player[0].GetComponent<Transform>().position = pPos.transform.position;
        else
            player[0].GetComponent<Transform>().position = pPosUp.transform.position;
        player[0].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        
        tm.TurnEnd();
    }

    public void landing()
    {
        //Debug.Log("Landing");
        for(int i=0;i<enemy.Count;i++)
        {
            if(enemy[i].GetComponent <Status>().IsItGrounded())
                if(enemy[i].GetComponent<Transform>().position == ePosBoxUp[e_index[i]].transform.position)
                    enemy[i].GetComponent<Transform>().position = ePosBox[e_index[i]].transform.position;
                else if(enemy[i].GetComponent<Transform>().position == battlePosRightUp.transform.position)
                    enemy[i].GetComponent<Transform>().position = battlePosRight.transform.position;
            
        }
        if (psmList[0].IsItGrounded())
        {
            if (player[0].GetComponent<Transform>().position == pPosUp.transform.position)
                player[0].GetComponent<Transform>().position = pPos.transform.position;
            else if (player[0].GetComponent<Transform>().position == battlePosLeftUp.transform.position)
                player[0].GetComponent<Transform>().position = battlePosLeft.transform.position;
            //Debug.Log("Landing psLGrounded");
        }
            
    }

    void PositionUpdate()
    {
        Debug.Log(enemy.Count + "testtt " + esList.Count);
        for (int i = 0; i < enemy.Count; i++)
        {
            if (esList[e_index[i]].IsItGrounded())
            {
                enemy[e_index[i]].GetComponent<Transform>().position = ePosBox[i].transform.position;
            }
            else
                enemy[e_index[i]].GetComponent<Transform>().position = ePosBoxUp[i].transform.position;
            if (psmList[0].IsItGrounded())
                player[0].GetComponent<Transform>().position = pPos.transform.position;
            else
                player[0].GetComponent<Transform>().position = pPosUp.transform.position;
        }
    }

    void PullEnemy(int index)
    {
        if (e_index[index] != -1)
        {
            int[] tempArr=new int[4];
            for(int i = 0; i <4; i++)
            {
                tempArr[i] = e_index[i];
            }
            //Debug.Log(tempArr[0] +""+ tempArr[1]+ "" + tempArr[2]+ "" + tempArr[3]);
            int index2 = Array.IndexOf(e_index, index);
            int i3;
            if (index2 - 2 < 0)
                i3 = 0;
            else
                i3 = index2-2;
            //Debug.Log(i3);
            int i4 = index2-i3;
            e_index[i3] = index;
            if (i4 == 1)
            {
                e_index[i3+1] = tempArr[0];
            }
            else
            {
                e_index[i3 + 1] = tempArr[i3];
                e_index[i3 + 2] = tempArr[i3+1];
                Debug.Log(i3 + 1);
            }
            //for (int i = 0; i < 4; i++)
            //{


            //    if (i == i3 && e_index[i] == index)
            //        break;
            //    else if(e_index[i] == tempArr[i3])
            //        e_index[i] = tempArr[i + 1];
            //    else if()
            //        e_index[++i] = tempArr[i - 1];

            //}

        }
        PositionUpdate();
    }

    public void SelfActionMark(int buffStack, float heal)
    {
        this.buffStack = buffStack;
        AimSelf();
    }

    public void FocusActionMark(int buffStack, float heal, float dmg, float t_dmg, int area, bool knockup, bool jump,String mode)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        this.knockup = knockup;
        jumpTf = jump;
        this.buffStack = buffStack;
        this.mode = mode;
        AimSelf();
        AimTheTarget_Release(1);
    }

    public void MagicActionMark(float dmg, float t_dmg, int area)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        MagicAim();
    }

    bool pull = false;
    public void PullActionMark(float dmg)
    {
        pull = true;
        this.dmg = Mathf.RoundToInt(dmg);
        AimTheTarget();
    }

    public void ActionMark(float dmg)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        AimTheTarget();
        
    }

    public void ActionMark(float dmg, float t_dmg)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        AimTheTarget();
    }

    public void ActionMark(float dmg, float t_dmg, int area,String mode)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        this.mode = mode;
        AimTheTarget(area);
    }

    public void ActionMark(float dmg, float t_dmg, int area, String mode,String anim)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        this.mode = mode;
        this.anim = anim;
        AimTheTarget(area);
    }

    public void AgentActionMark(float dmg, float t_dmg, int target,String mode)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        if (mode == "Upper")
        {
            jumpTf = false;
        }
        if (mode == "Lower")
        {
            jumpTf = true;
        }
        if (mode == "Middle"&&!psmList[0].IsItGrounded())
        {
            jumpTf = true;
        }
        AgentAction(target);
    }

    public void ActionMark(float dmg, float t_dmg, int area, bool knockup, bool jump, String mode)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        this.knockup = knockup;
        jumpTf = jump;
        this.mode = mode;
        AimTheTarget(area);
    }

    public void ActionMark(float dmg, float t_dmg, int area, bool knockup, bool jump, String mode, String anim)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        this.knockup = knockup;
        jumpTf = jump;
        this.mode = mode;
        this.anim = anim;
        AimTheTarget(area);
    }

    public void ActionMark(string animationName, float dmg, float t_dmg, int area)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        animName = animationName;
        AimTheTarget(area);
    }

    public void eActionMark(int code,float dmg)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        if (!esList[code].GetAnimator())
            eAction(code);
        else
            StartCoroutine(eActionContinue(code));

    }

    IEnumerator eActionContinue(int code)
    {
        yield return new WaitForSeconds(1f);
        eAction(code);
    }

    public void eActionMark(int code, float dmg,int area)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        eAction(code);
    }

    public void eLandingMark(int code)
    {
        eLanding(code);
    }

    bool fTime = true;
    public void GameReset()
    {
        //p_hpSlider.Add(findChildFromParent(player[pi].name, "Health"));
        //p_touSlider.Add(findChildFromParent(player[pi].name, "Toughness"));
        //if (PlayerPrefs.GetInt("Cheese") == 1)
        //    bm.p_MagicButtonActivate(true);
        //else
        //    bm.p_MagicButtonActivate(false);
        foreach (Status ob in psmList)
        {
            ob.StatusRest();
        }
        foreach (Status ob in esList)
        {
            ob.StatusRest();
        }

        foreach (GameObject ob in p_hpSlider)
        {
            ob.SetActive(true);
            ob.GetComponent<Slider>().value=1;
        }
        for (int i = 0; i < p_tou.Count; i++)
        {
            p_hp[i] = psmList[i].GetHp();
        }

        foreach (GameObject ob in p_touSlider)
        {
            ob.SetActive(true);
            ob.GetComponent<Slider>().value = 1;
        }
        for(int i = 0; i < p_tou.Count; i++)
        {
            p_tou[i] = psmList[i].GetTou();
        }

        foreach (GameObject ob in e_hpSlider)
        {
            ob.SetActive(true);
            ob.GetComponent<Slider>().value = 1;
        }
        for (int i = 0; i < p_tou.Count; i++)
        {
            e_hp[i] = esList[i].GetHp();
        }

        foreach (GameObject ob in e_touSlider)
        {
            ob.SetActive(true);
            ob.GetComponent<Slider>().value = 1;
        }
        for (int i = 0; i < p_tou.Count; i++)
        {
            e_tou[i] = esList[i].GetTou();
        }

        foreach (GameObject ob in p_marker)
        {
            ob.SetActive(false);
        }
        foreach (GameObject ob in p_clickBox)
        {
            ob.SetActive(false);
        }

        foreach (GameObject ob in e_marker)
        {
            ob.SetActive(false);
        }
        foreach (GameObject ob in e_clickBox)
        {
            ob.SetActive(false);
        }
        if(!fTime)
            PositionUpdate();
        tm.TurnManagerStrat();
        fTime = false;
    }

    public int PlayerAmount()
    {
        return player.Count;
    }

    public int EnemyAmount()
    {
        return enemy.Count;
    }

    public (float,float,bool,bool,bool) PlayerState(int id)
    { 
        return (p_hp[id],p_tou[id], psmList[id].IsItGrounded(),
            psmList[id].IsItBreaked(), psmList[id].IsItDying());
    }

    public (float, float, bool, bool, bool) EnemyState(int id)
    {
        return (e_hp[id], e_tou[id], esList[id].IsItGrounded(),
            esList[id].IsItBreaked(), esList[id].IsItDying());
    }

    public class StateCollection
    {
        public float hp;
        public float tou;
        public bool grounded;
        public bool breaked;
        public bool dying;
    }
}
