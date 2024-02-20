using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class TurnManager : MonoBehaviour
{
    GameManager gm;
    public List<GameObject> participant = new List<GameObject>();
    List<Status> psList = new List<Status>();

    List<int> e_speed = new List<int>();
    List<int> p_speed = new List<int>();
    int lc = 0;

    List<ObjectAndTime> sortedList = new List<ObjectAndTime>();
    //public List<GameObject> test = new List<GameObject>();
    List<ObjectAndTime> otList = new List<ObjectAndTime>();

    public GameObject[] box = new GameObject[8];
    public List<GameObject> sortedBox = new List<GameObject>();
    int boxAmount=4;
    public List<int> knockUpCount = new List<int>();
    
    int roundDefault = 250;
    int round;
    int roundCount = 0;

    void Start()
    {
        TurnManagerStrat();
    }

    public void SpeedGen()
    {
        for (int i = 0; i < participant.Count; i++)
        {
            int countSort = 0;
            for (int i2=0;i2<16;i2++)
            {
                ObjectAndTime ot = new ObjectAndTime();
                ot.go = participant[i];
                ot.TimeToTurn = 10000 / e_speed[i];
                ot.TimeToTurnBuffer = 10000 / e_speed[i];
                //Debug.Log(ot.TimeToTurn + " " + ot.TimeToTurnBuffer);
                for (int i3=0;i3<countSort;i3++)
                {
                    ot.TimeToTurn += ot.TimeToTurnBuffer;
                }
                otList.Add(ot);
                countSort++;
            }
        }
        SpeedCount();
    }

    public void SpeedCount()
    {
        SortTheList();
        if(sortedBox.Count==0)
        for (int i = 0; i < boxAmount; i++)
        {
            GameObject ob = Instantiate(sortedList[i].go.transform.Find("Box").gameObject);
                ob.transform.SetParent(this.gameObject.transform, true);
                ob.transform.position = box[i].transform.position;
            ob.GetComponent<RectTransform>().localScale = box[i].GetComponent<RectTransform>().localScale;
            sortedBox.Add(ob);
        }
        //else if(sortedBox.Count>= boxAmount -1&& sortedBox.Count< boxAmount)
        //{
        //    for(int i=0;i<sortedBox.Count;i++)
        //    {
        //        sortedBox[i].transform.position = box[i].transform.position;
        //    }
        //    GameObject ob = Instantiate(sortedList[boxAmount-1].go.transform.Find("Box").gameObject);
        //    ob.transform.parent = this.gameObject.transform;
        //    ob.transform.position = box[boxAmount-1].transform.position;
        //    ob.GetComponent<RectTransform>().localScale = box[boxAmount-1].GetComponent<RectTransform>().localScale;
        //    sortedBox.Add(ob);
        //}
        else
        {
            sortedBox.Clear();
            for (int i = 0; i < boxAmount; i++)
            {
                GameObject ob = Instantiate(sortedList[i].go.transform.Find("Box").gameObject);
                ob.transform.SetParent(this.gameObject.transform);
                ob.transform.position = box[i].transform.position;
                ob.GetComponent<RectTransform>().localScale = box[i].GetComponent<RectTransform>().localScale;
                sortedBox.Add(ob);
            }
        }

        if (fTurn)
        {
            TurnStart();
            fTurn = false;
        }
    }

    void SortTheList()
    {
        //Debug.Log(sortedList.Count);
        if(sortedList.Count==0)
        sortedList = otList;
        sortedList = sortedList.OrderBy(o => o.TimeToTurn).ToList();
    }

    bool fTurn=true;
    GameObject goBuffer;
    public void TurnStart()
    {
        try
        {
            int ft = sortedList[0].TimeToTurn;
            foreach (ObjectAndTime o in sortedList)
            {
                o.TimeToTurn -= ft;
                
                //Debug.Log(o.go.tag+" "+o.go.name+" "+o.TimeToTurn);
            }
            //if (round > ft)
            //    round -= ft;
            //else
            //{
            //    ft -= round;
            //    round = 0;
            //}
            //if (round == 0)
            //{
            //    roundCount++;
            //    round = roundDefault;
            //    round -= ft;
            //}
            sortedList[0].go.GetComponent<Status>().TurnStart();
            Debug.Log(sortedList[0].go.tag+" Action TurnManager");
            if(goBuffer!=sortedList[0].go)
            {
                sortedList[0].go.GetComponent<Status>().KnockUpClear();
                Debug.Log("KnockUpClear");
            }

            for (int i = 0; participant.Count > i; i++)
            {
                if (participant[i].GetComponent<Status>().KnockUpCount() >= 0)
                    participant[i].GetComponent<Status>().KnockUpDecrease();
            }
            gm.landing();
            //Debug.Log(sortedList[0].go.tag + " Turn Start");
            //TurnEnd();
        }
        catch (Exception e) 
        { 
            Debug.Log("TurnStart Error in TurnManager "+e);
        }
        
            
    }
    
    public int RoundNumber()
    {
        return roundCount;
    }

    ObjectAndTime removeBuffer;
    public void TurnEnd()
    {
        try
        {
            sortedList[0].go.GetComponent<Status>().TurnEnd();
            goBuffer = sortedList[0].go;
            foreach(ObjectAndTime otfe in sortedList)
            {
                if (otfe.go.tag == goBuffer.tag)
                    otfe.TimeToTurn += otfe.TimeToTurnBuffer;
           }
            removeBuffer = sortedList[0];
            GameObject.Destroy(sortedBox[0]);
            sortedBox.RemoveAt(0);
            SpeedCount();
            TurnStart();
        }
        catch (Exception e) { Debug.Log(e); }

    }

    public void Break(GameObject go)
    {
        if(go.GetComponent<Status>().IsItBreaked()==false)
        {
            foreach (ObjectAndTime ot in sortedList)
            {
                if (ot.go.tag == go.tag)
                    ot.TimeToTurn += ot.TimeToTurnBuffer;
            }
            go.GetComponent<Status>().IsItBreaked(true);
        }
        SpeedCount();
    }

    public void Dying(GameObject go)
    {
        if (go.GetComponent<Status>().IsItDying() == false)
        {
            sortedList.RemoveAll(oat => oat.go.tag == go.tag);
            
            go.GetComponent<Status>().IsItDying(true);
        }
        SpeedCount();
    }

    public void TurnManagerStrat()
    {
        participant = new List<GameObject>();
        psList = new List<Status>();

        e_speed = new List<int>();
        p_speed = new List<int>();
        lc = 0;

        sortedList = new List<ObjectAndTime>();
        //public List<GameObject> test = new List<GameObject>();
        otList = new List<ObjectAndTime>();

        box = new GameObject[8];
        sortedBox = new List<GameObject>();
        boxAmount = 4;
        knockUpCount = new List<int>();

        roundDefault = 250;
        int round;
        int roundCount = 0;

        round = roundDefault;
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        for (int i = 0; i < box.Length; i++)
        {
            //Debug.Log(this.gameObject.transform.GetChild(i).gameObject);
            box[i] = this.gameObject.transform.GetChild(i).gameObject;
        }

        if (GameObject.FindGameObjectWithTag("Enemy1"))
        {
            participant.Add(GameObject.FindGameObjectWithTag("Enemy1"));
            psList.Add(participant[lc].GetComponent<Status>());
            e_speed.Add(psList[lc].GetSpeed());
            boxAmount++;
            lc++;
        }

        if (GameObject.FindGameObjectWithTag("Enemy2"))
        {
            participant.Add(GameObject.FindGameObjectWithTag("Enemy2"));
            psList.Add(participant[lc].GetComponent<Status>());
            e_speed.Add(psList[lc].GetSpeed());
            boxAmount++;
            lc++;
        }

        if (GameObject.FindGameObjectWithTag("Enemy3"))
        {
            participant.Add(GameObject.FindGameObjectWithTag("Enemy3"));
            psList.Add(participant[lc].GetComponent<Status>());
            e_speed.Add(psList[lc].GetSpeed());
            boxAmount++;
            lc++;
        }

        if (GameObject.FindGameObjectWithTag("Enemy4"))
        {
            participant.Add(GameObject.FindGameObjectWithTag("Enemy4"));
            psList.Add(participant[lc].GetComponent<Status>());
            e_speed.Add(psList[lc].GetSpeed());
            boxAmount++;
            lc++;
        }

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            participant.Add(GameObject.FindGameObjectWithTag("Player"));
            psList.Add(participant[lc].GetComponent<Status>());
            e_speed.Add(psList[lc].GetSpeed());
            lc++;
        }
        for (int i = 0; i < lc; i++)
            knockUpCount.Add(0);
        SpeedGen();
    }

    GameObject findChildFromParent(string parentName, string childNameToFind)
    {
        string childLocation = parentName + "/" + childNameToFind;
        GameObject childObject = GameObject.Find(childLocation);
        return childObject;
    }
}

public class ObjectAndTime
{
    public GameObject go;
    public int TimeToTurn;
    public int TimeToTurnBuffer;
}