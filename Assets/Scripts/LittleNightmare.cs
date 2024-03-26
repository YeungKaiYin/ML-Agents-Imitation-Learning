using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class LittleNightmare : MonoBehaviour
{
    //力量,敏捷,智力,體質,外貌,意志,體型,教育,機動力
    //str, dex, int, con, app, pow, siz, edu, mov
    int hp, tou, mov;
    float dmg, heal;
    float t_dmg;
    public List<GameObject> skillSetList = new List<GameObject>();
    //GameObject aCollider;
    bool airMode = false;
    string mode = "";
    GameManager gm;
    Status sm;
    ModeSwitch ms;
    bool myTurn = false;
    Animator animator;
    //private void Awake()
    //{
    //    CreateNewTag("LittleNightmare");
    //    this.gameObject.tag = "LittleNightmare";
        
    //}

    void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        sm = gameObject.GetComponent<Status>();
        ms = GameObject.FindGameObjectWithTag("ModeSwitch").GetComponent<ModeSwitch>();
        gameObject.name = "LittleNightmare";
        ChangeStatue();
        animator = sm.GetAnimator();
        Animator_Idle();
        foreach (GameObject obj in skillSetList)
            obj.SetActive(false);
        //aCollider = gameObject.transform.Find("AttackCollider").gameObject;
        //aCollider.SetActive(false);
    }

    public void Action()
    {
        Debug.Log("nm action");
        foreach (GameObject obj in skillSetList)
            obj.SetActive(true);
        myTurn = true;
    }

    public bool AgentTurnStart()
    {
        return myTurn;
    }

    public void AgentTurnEnd()
    {
        myTurn = false;
    }

    public void ChangeStatue()
    {
        try
        {
            //StatusManager sm = GameObject.FindGameObjectWithTag("LittleNightmare").GetComponent<StatusManager>();
            //Status sm = gameObject.GetComponent<Status>();
            hp = (sm.GetCon() + sm.GetSiz());
            tou = (sm.GetCon() + sm.GetSiz()) * 1;
            sm.SetHp(hp);
            sm.SetTou(tou);
            //Debug.Log("ChangeStatue");
        }
        catch(Exception e) { Debug.Log(e); }
    }

    //敵我，範圍，hp傷害(dmg)，韌性傷害(t_dmg)，治療(heal)，buff，debuff
    public void Animator_Idle()
    {
        if (animator != null)
            animator.SetBool("MouseStand_Idle", true);
    }

    public void ModeSwitch(string mode)
    {
        this.mode = mode;
        gm.AimTheTarget_False();
        //ms.ButtonActive();
        if (animator != null)
        {

            if (mode == "Stand")
                animator.SetTrigger("MouseStand_Appear");
            else if (mode == "Cheese")
                animator.SetTrigger("Cheese_Appear");
        }
    }

    public String GetMode()
    {
        return mode;
    }

    public void UpperAction()
    {
        if(mode=="Stand")
        {
            if (animator != null)
            {
                animator.SetTrigger(sm.IsItGrounded() ? "MouseStand_UpperBladeReady" : "MouseStand_AirUpperBladeReady");
            }
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
            //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Debug.Log("dmg:" + dmg);
            gm.ActionMark(dmg, t_dmg, 2, "Upper", sm.IsItGrounded() ? "MouseStand_UpperBladeAction" : "MouseStand_AirUpperBladeAction");
        }

        if (mode == "Cheese")
        {
            if (animator != null)
            {
                animator.SetTrigger("Cheese_To_Kibble");
            }
            dmg = sm.GetInt() + (float)((double)sm.GetApp() * 0.2) + (float)((double)sm.GetEdu() * 0.5);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
            gm.ActionMark(dmg, t_dmg, 1, "Upper", "Cheese_To_Kibble");
        }
    }

    public void MiddleAction()
    {
        if (mode == "Stand")
        {
            if (animator != null)
            {
                animator.SetTrigger("MouseStand_MiddleBladeReady");
            }
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.1) + (float)((double)sm.GetDex() * 0.3);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2);
            //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Debug.Log("dmg:" + dmg);
            gm.ActionMark(dmg, t_dmg, 2, mode, "MouseStand_MiddleBladeAction");
            
        }

        if (mode == "Cheese")
        {
            if (animator != null)
            {
                animator.SetTrigger("Cheese_To_Ball");
            }
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            t_dmg = sm.GetInt() + (float)((double)sm.GetApp() * 0.2) + (float)((double)sm.GetEdu() * 0.5);
            gm.ActionMark(dmg, t_dmg, 1, "Middle", "Cheese_To_Ball");
        }
    }

    public void LowerAction()
    {
        if (mode == "Stand")
        {
            if (animator != null)
            {
                animator.SetTrigger("MouseStand_LowerBladeReady");
            }
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Debug.Log("dmg:" + dmg);
            gm.ActionMark(dmg, t_dmg, 2, true, true, mode, "MouseStand_LowerBladeAction");
            
        }

        if (mode == "Cheese")
        {
            if (animator != null)
            {
                animator.SetTrigger("Cheese_To_CatTeaser");
            }
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            t_dmg = sm.GetInt() + (float)((double)sm.GetApp() * 0.2) + (float)((double)sm.GetEdu() * 0.5);
            gm.ActionMark(dmg, t_dmg, 1, "Lower", "Cheese_To_CatTeaser");
        }
    }

    public void BladeAttack()
    {
        // 
        //do some perpare action
        // 
        if (animator != null)
        {
            animator.SetTrigger("MouseStand_Appear");
            animator.SetBool("MouseStand_Idle", false);
        }
            
        if (mode=="Lower")
        {
            //Status sm = gameObject.GetComponent<Status>();
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Debug.Log("dmg:" + dmg);
            gm.ActionMark(dmg, t_dmg, 2, true, true,mode, "MouseStand_LowerBladeAction");
            animator.SetTrigger("MouseStand_LowerBladeReady");
        }
        else if(mode == "Middle")
        try
        {
            //Status sm = gameObject.GetComponent<Status>();
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.1) + (float)((double)sm.GetDex() * 0.3);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2);
            //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Debug.Log("dmg:" + dmg);
            gm.ActionMark(dmg,t_dmg,2,mode, "MouseStand_MiddleBladeAction");
            animator.SetTrigger("MouseStand_MiddleBladeReady");

        }
        catch (Exception e) { Debug.Log(e); }
        else if (mode == "Upper")
            try
            {
                //Status sm = gameObject.GetComponent<Status>();
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
                //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                Debug.Log("dmg:" + dmg);
                gm.ActionMark(dmg, t_dmg, 2, mode,sm.IsItGrounded()? "MouseStand_UpperBladeAction" : "MouseStand_AirUpperBladeAction");
                animator.SetTrigger(sm.IsItGrounded() ? "MouseStand_UpperBladeReady" : "MouseStand_AirUpperBladeReady");
            }
            catch (Exception e) { Debug.Log(e); }

    }

    public void AgentBladeAttack(String mode,int target)
    {
        try
        {
            Debug.Log("Agent Blade Attack");
            //Status sm = gameObject.GetComponent<Status>();
            this.mode = mode;
            
            if (mode == "Lower")
            {
                ms.Lower();
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
                gm.AgentActionMark(dmg, t_dmg, target, mode);
            }
            if (mode == "Middle")
            {
                ms.Middle();
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.1) + (float)((double)sm.GetDex() * 0.3);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2);
                gm.AgentActionMark(dmg, t_dmg, target, mode);
            }
            if (mode == "Upper")
            {
                ms.Upper();
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.3) + (float)((double)sm.GetSiz() * 0.3);
                gm.AgentActionMark(dmg, t_dmg, target, mode);
            }
        }
        catch (Exception e) { Debug.Log(e); }
    }

    public void FistAttack()
    {
        // 
        //do some perpare action
        // 

        //Status sm = gameObject.GetComponent<Status>();
        if(animator!=null)
            animator.SetTrigger("Cheese_Appear");
        Debug.Log(sm.IsItGrounded());
        if (mode == "Lower")
        {
            //ms.Lower();
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
            gm.SelfActionMark(1, 0);
            animator.SetTrigger("Cheese_To_CatTeaser");
        }
        else if (mode == "Middle")
        {
            //ms.Middle();
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
            animator.SetTrigger("Cheese_To_Ball");
        }
        else if (mode == "Upper")
        {
            //ms.Upper();
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
            animator.SetTrigger("Cheese_To_Kibble");
        }

        try
        {
            //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            Debug.Log("dmg:" + dmg);
            if (mode=="Lower"&sm.IsItGrounded())
            {
                
                if(sm.FocusStack()>0)
                    gm.FocusActionMark(1, 1, dmg, t_dmg, 1, true, true,mode);
                else
                    gm.FocusActionMark(1, 1, dmg, t_dmg, 1, true, false,mode);
            }
            else if (mode == "Lower" && !sm.IsItGrounded())
            {
                gm.ActionMark(dmg, t_dmg, 1, false, true,mode);
            }
            else if (!sm.IsItGrounded())
            {
                gm.ActionMark(dmg, t_dmg, 1, false, false,mode);
            }
            else
            {
                gm.ActionMark(dmg, t_dmg, 1,mode);
            }
            
        }
        catch (Exception e) { Debug.Log(e); }
    }

    public void CheeseStrategy()
    {
        // 
        //do some perpare action
        // 

        //Status sm = gameObject.GetComponent<Status>();
        if(PlayerPrefs.GetInt("CheeseGet")==1&&sm.CheeseCount()>0)
        {
            if (animator != null)
            {
                animator.SetBool("MouseStand_Idle", false);
                animator.SetTrigger("Cheese_Appear");
            }

            Debug.Log(sm.IsItGrounded());
            if (mode == "Lower")
            {
                //ms.Lower();
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
                t_dmg = sm.GetInt() + (float)((double)sm.GetApp() * 0.2) + (float)((double)sm.GetEdu() * 0.5);
                animator.SetTrigger("Cheese_To_CatTeaser");
            }
            else if (mode == "Middle")
            {
                //ms.Middle();
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
                t_dmg = sm.GetInt() + (float)((double)sm.GetApp() * 0.2) + (float)((double)sm.GetEdu() * 0.5);
                animator.SetTrigger("Cheese_To_Ball");
            }
            else if (mode == "Upper")
            {
                //ms.Upper();
                dmg = sm.GetInt() + (float)((double)sm.GetApp() * 0.2) + (float)((double)sm.GetEdu() * 0.5);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
                animator.SetTrigger("Cheese_To_Kibble");
            }

            try
            {
                //GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                Debug.Log("dmg:" + dmg);
                if (mode == "Lower")
                    if (mode == "Lower" & sm.IsItGrounded())
                    {

                        if (sm.FocusStack() > 0)
                            gm.FocusActionMark(1, 1, dmg, t_dmg, 1, true, true, mode);
                        else
                            gm.FocusActionMark(1, 1, dmg, t_dmg, 1, true, false, mode);
                    }
                    else if (mode == "Lower" && !sm.IsItGrounded())
                    {
                        gm.ActionMark(dmg, t_dmg, 1, false, true, mode);
                    }
                    else if (!sm.IsItGrounded())
                    {
                        gm.ActionMark(dmg, t_dmg, 1, false, false, mode);
                    }
                    else
                    {
                        gm.ActionMark(dmg, t_dmg, 1, mode);
                    }

            }
            catch (Exception e) { Debug.Log(e); }
        }
        
    }

    public void AgentFistAttack(String mode,int target)
    {
        try
        {
            Debug.Log("Agent Fist Attack");
            //Status sm = gameObject.GetComponent<Status>();
            this.mode = mode;
            if (mode == "Lower")
            {
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
                gm.AgentActionMark(dmg, t_dmg, target, mode);
            }
            if (mode == "Middle")
            {
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
                gm.AgentActionMark(dmg, t_dmg, target, mode);
            }
            if (mode == "Upper")
            {
                dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
                t_dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.5);
                gm.AgentActionMark(dmg, t_dmg, target, mode);
            }
        }
        catch (Exception e) { Debug.Log(e); }
    }

    public void MagicAttack()
    {
        // 
        //do some perpare action
        // 
        Status sm = gameObject.GetComponent<Status>();
        dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
        t_dmg = 999+sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
        try
        {
            GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            if (!airMode)
            {
                Debug.Log("dmg:" + dmg);
                gm.MagicActionMark(dmg, t_dmg, 4);
            }
            else
            {
                gm.MagicActionMark(dmg, t_dmg, 4);
                //gm.PullActionMark(dmg);
            }
                
        }
        catch (Exception e) { Debug.Log(e); }
    }

    public void AgentMagicAttack(int target)
    {
        try
        {
            Debug.Log("Agent Magic Attack");
            Status sm = gameObject.GetComponent<Status>();
            dmg = sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            t_dmg = 999+sm.GetStr() + (float)((double)sm.GetCon() * 0.2) + (float)((double)sm.GetSiz() * 0.2);
            gm.AgentActionMark(dmg, t_dmg, target, "Middle");
        }
        catch (Exception e) { Debug.Log(e); }
    }

    //static void CreateNewTag(string tagName)
    //{
    //    SerializedObject tagManager = new SerializedObject(
    //        AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
    //    SerializedProperty tagsProp = tagManager.FindProperty("tags");

    //    // Check if the tag already exists
    //    bool tagExists = false;
    //    for (int i = 0; i < tagsProp.arraySize; i++)
    //    {
    //        SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
    //        if (t.stringValue == tagName)
    //        {
    //            tagExists = true;
    //            break;
    //        }
    //    }

    //    // Add the tag if it doesn't exist
    //    if (!tagExists)
    //    {
    //        tagsProp.InsertArrayElementAtIndex(tagsProp.arraySize);
    //        SerializedProperty newTag = tagsProp.GetArrayElementAtIndex(tagsProp.arraySize - 1);
    //        newTag.stringValue = tagName;
    //        tagManager.ApplyModifiedProperties();
    //    }
    //}
}
