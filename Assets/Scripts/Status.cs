using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Status : MonoBehaviour
{
    GameManager gm;
    public ButtonManager bm;
    public Animator animator;
    //力量,敏捷,智力,體質,外貌,意志,體型,教育,機動力
    //str, dex, int, con, app, pow, siz, edu, mov
    public int s_str, s_dex, s_int, s_con, s_app, s_pow, s_siz, s_edu, s_mov;
    [SerializeField]
    float hp, tou;
    int speed, timeToTurnStart;
    [SerializeField]
    string enemy_code;
    //bool myTurn = false;
    int focusStack = 0;
    bool mustHit = false;

    bool breaked = false;
    bool breakProtect = false;
    bool dying = false;
    bool grounded = true;
    bool propsGet = false;
    LittleNightmare nm;
    Enemy01 e01;
    Slider tou_slider;
    int cheeseCount = 0;

    public TextMeshProUGUI tmpText;
    public Animator cheeseAnimator;
    public GameObject effect_Spiral;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        //GetStatus();
        hp = s_con + s_siz;
        tou = (s_con + s_siz) / 3;
        speed = (s_dex - s_siz / 5) * 10;
        timeToTurnStart = 10000 / speed;
        //Debug.Log(timeToTurnStart + " " + nextTimeToTurnStart);
        tou_slider =gameObject.transform.Find("Toughness").gameObject.GetComponent<Slider>();
        if (gameObject.tag == "Player")
            bm = GameObject.FindGameObjectWithTag("ButtonManager").GetComponent<ButtonManager>();
        StatusRest();
        //delect it later
        PlayerPrefs.SetInt("CheeseGet", 1);
    }

    public void Start()
    {
        //tmpText.text = "0";
    }

    public void StatusRest()
    {
        focusStack = 0;
        IsItBreaked(false);
        IsItBreakProtect(false);
        KnockUpClear();
        IsItDying(false);
        IsItGrounded(true);
        if (gameObject.tag == "Player")
            CheeseGet();
    }

    public void CharaterAction()
    {
        if (gameObject.name == "LittleNightmare")
            gameObject.GetComponent<LittleNightmare>().Action();
        if (gameObject.name == "LittleNightmare")
            Debug.Log("LN CharacterAction");
        if (gameObject.name == "Enemy01")
            gameObject.GetComponent<Enemy01>().Action();
        if (gameObject.tag == "Player")
            bm.p_ButtonActivate();
    }

    private void CheeseGet()
    {
        if(PlayerPrefs.GetInt("CheeseGet")==1)
            cheeseCount = 3;
    }

    public int CheeseCount()
    {
        return cheeseCount;
    }

    public void CheeseUse()
    {
        if(cheeseCount>0)
        {
            if(cheeseAnimator!=null)
                cheeseAnimator.SetTrigger("consume");
            Debug.Log("CheeseCount :"+cheeseCount);
            cheeseCount -= 1;
        }
    }

    public Animator GetAnimator()
    {
        if (animator != null)
            return animator;
        else
            return null;
    }

    public void SetAnimator_Trigger(String anim)
    {
        animator.SetTrigger(anim);
    }

    public void SetAnimator_Bool(String anim,bool tf)
    {
        if(anim.Contains("Mouse")&&animator!=null)
            animator.SetBool("MouseStand_Idle",tf);
    }

    public int FocusStack()
    {
        return focusStack;
    }

    public int FocusStackRelease()
    {
        int finalStack = focusStack;
        focusStack = 0;
        tmpText.text = focusStack.ToString();
        return finalStack;
    }

    public void FocusAdd()
    {
        focusStack++;
    }

    public void FocusAdd(int stack)
    {
        focusStack+=stack;
        tmpText.text = focusStack.ToString();
    }

    public bool MustHit()
    {
        return mustHit;
    }

    public void MustHit(bool tf)
    {
        mustHit = tf;
    }

    public bool PropsGet()
    {
        return propsGet;
    }

    public void PropsGet(bool tf)
    {
        propsGet = tf;
    }

    public bool IsItBreaked()
    {
        return breaked;
    }

    public void IsItBreaked(bool tf)
    {
        breaked = tf;
        if (tf&&IsItBreakProtect()==false)
        {
            IsItBreakProtect(true);
            effect_Spiral.SetActive(true);
        }
        else if(!tf)
        {
            IsItBreakProtect(false);
            effect_Spiral.SetActive(false);
        }   
    }

    public bool IsItBreakProtect()
    {
        return breakProtect;
    }

    public void IsItBreakProtect(bool tf)
    {
        breakProtect = tf;
    }

    public bool IsItGrounded()
    {
        return grounded;
    }

    public void IsItGrounded(bool tf)
    {
        grounded = tf;
        if (tf == false)
            knockUpCount = 500;
        else
            knockUpCount = 0;
        //Debug.Log("knockup isitground " + tf);
    }

    int knockUpCount;
    public int KnockUpCount()
    {
        return knockUpCount;
    }

    //public void KnockUpIncrease()
    //{
    //    if (IsItGrounded())
    //        knockUpCount++;
    //}

    public void KnockUpDecrease(int time)
    {
        //Debug.Log("KnockUp Decrease " + knockUpCount + " " + time);
        if (knockUpCount > 0)
            knockUpCount-=time;
        if (knockUpCount <= 0)
            IsItGrounded(true);
        
    }

    public void KnockUpClear()
    {
        knockUpCount=0;
        IsItGrounded(true);
        //Debug.Log("KnockUp Clear");
    }


    public bool IsItDying()
    {
        return dying;
    }

    public void IsItDying(bool tf)
    {
        dying = tf;
    }

    public void TurnStart()
    {
        //myTurn = true;
        if(IsItBreaked())
        {
            tou_slider.value = 1;
            IsItBreaked(false);
            IsItBreakProtect(false);
        }
        CharaterAction();
    }

    public void TurnEnd()
    {
        //if(knockUpCount>0)
        //    knockUpCount--;
        //KnockUpDecrease();
        if (gameObject.tag == "Player")
            bm.p_ButtonDisable();
        //myTurn = false;
    }
    
    //getter method
    #region
    public int GetStr()
    {
        //s_str = ConvertStringToNumber(PlayerPrefs.GetString("str"));
        //s_str = PlayerPrefs.GetInt(enemy_code+"_str");
        return s_str;
    }

    public int GetDex()
    {
        //s_dex = ConvertStringToNumber(PlayerPrefs.GetString("dex"));
        //s_dex = PlayerPrefs.GetInt(enemy_code+"_dex");
        return s_dex;
    }

    public int GetInt()
    {
        //s_int = ConvertStringToNumber(PlayerPrefs.GetString("int"));
        //s_int = PlayerPrefs.GetInt(enemy_code+"_int");
        return s_int;
    }

    public int GetCon()
    {
        //s_con = ConvertStringToNumber(PlayerPrefs.GetString("con"));
        //s_con = PlayerPrefs.GetInt(enemy_code+"_con");
        return s_con;
    }

    public int GetApp()
    {
        //s_app = ConvertStringToNumber(PlayerPrefs.GetString("app"));
        //s_app = PlayerPrefs.GetInt(enemy_code+"_app");
        return s_app;
    }

    public int GetPow()
    {
        //s_pow = ConvertStringToNumber(PlayerPrefs.GetString("pow"));
        //s_pow = PlayerPrefs.GetInt(enemy_code+"_pow");
        return s_pow;
    }

    public int GetSiz()
    {
        //s_siz = ConvertStringToNumber(PlayerPrefs.GetString("siz"));
        //s_siz = PlayerPrefs.GetInt(enemy_code+"_siz");
        return s_siz;
    }

    public int GetEdu()
    {
        //s_edu = ConvertStringToNumber(PlayerPrefs.GetString("edu"));
        //s_edu = PlayerPrefs.GetInt(enemy_code+"_edu");
        return s_edu;
    }

    public int GetMov()
    {
        //s_mov = ConvertStringToNumber(PlayerPrefs.GetString("mov"));
        //s_mov = PlayerPrefs.GetInt(enemy_code+"_mov");
        return s_mov;
    }
    #endregion

    public void SetHp(int hp_value)
    {
        hp = hp_value;
        //Debug.Log("SetHp = " + hp);
    }

    public float GetHp()
    {
        return hp;
    }

    public void SetTou(int tou_value)
    {
        //Debug.Log(tou);
        tou = tou_value;
    }

    public float GetTou()
    {
        
        return tou;
    }

    public void SetSpeed(int speed_value)
    {
        speed = speed_value;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public void SetTimeToTurnStart(int ttts_value)
    {
        timeToTurnStart = ttts_value;
    }

    public int GetTimeToTurnStart()
    {
        return timeToTurnStart;
    }

    void GetStatus()
    {
        if (s_app == 0 && s_con == 0 && s_dex == 0 && s_edu == 0 && s_int == 0 && s_mov == 0)
            SetStatue();
    }

    public void SetStatue()
    {
        //str, dex, int, con, app, pow, siz, edu, mov
        s_app = 5;
        s_con = 5;
        s_dex = 5;
        s_edu = 5;
        s_int = 5;
        s_str = 5;
        s_pow = 5;
        s_siz = 5;
        GetStatus();
    }

    int ConvertStringToNumber(string str)
    {
        string t="";
        int result = 0;
        for (int i = 0; i <= str.Length - 1; i++)
        {
            switch (str[i])
            {
                case 'A':
                    t += "0";
                    break;
                case 'B':
                    t += "1";
                    break;
                case 'C':
                    t += "2";
                    break;
                case 'D':
                    t += "3";
                    break;
                case 'E':
                    t += "4";
                    break;
                case 'F':
                    t += "5";
                    break;
                case 'G':
                    t += "6";
                    break;
                case 'H':
                    t += "7";
                    break;
                case 'I':
                    t += "8";
                    break;
                case 'J':
                    t += "9";
                    break;
                default:
                    t += "0";
                    break;
            }
        }
        try { result = int.Parse(t); }
        catch (Exception e) { Debug.Log(e); }
        return result;
    }

    public static string ConvertNumberToString(int number)
    {
        string s_num = number.ToString();
        string t = "";
        for (int i = 0; i <= s_num.Length - 1; i++)
        {
            switch (s_num[i])
            {
                case '0':
                    t += "A";
                    break;
                case '1':
                    t += "B";
                    break;
                case '2':
                    t += "C";
                    break;
                case '3':
                    t += "D";
                    break;
                case '4':
                    t += "E";
                    break;
                case '5':
                    t += "F";
                    break;
                case '6':
                    t += "G";
                    break;
                case '7':
                    t += "H";
                    break;
                case '8':
                    t += "I";
                    break;
                case '9':
                    t += "J";
                    break;
                default:
                    t += "";
                    break;
            }
        }
        //Debug.Log(t);
        return t;
    }

    GameObject findChildFromParent(string parentName, string childNameToFind)
    {
        string childLocation = parentName + "/" + childNameToFind;
        GameObject childObject = GameObject.Find(childLocation);
        return childObject;
    }
}
