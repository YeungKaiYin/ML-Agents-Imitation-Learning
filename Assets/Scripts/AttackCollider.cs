using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour
{
    int dmg, maxDmg, minDmg, heal;
    int t_dmg;
    GameManager gm;

    private void Awake()
    {
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ActionMark(float dmg)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        //AimTheTarget();

    }

    public void ActionMark(float dmg, float t_dmg)
    {
        this.dmg = Mathf.RoundToInt(dmg);
        this.t_dmg = Mathf.RoundToInt(t_dmg);
        //AimTheTarget();
    }

    public void ActionMark(char target, float dmg)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("?? "+gameObject.tag+" "+collision.tag);
        if(gameObject.tag=="PlayerCollider"&& collision.tag =="GetHitCollider")
        {
            Debug.Log("pColliderHit");
            gm.GM_pColliderHit();
        }
            
        if (gameObject.tag == "PlayerCollider" && collision.tag == "MissCollider")
        {
            Debug.Log("pColliderMiss");
            gm.GM_pColliderMiss();
        }
            
        //if (collision.tag == "Player")
        //    gm.GM_eColliderHit();
        if (gameObject.tag != "PlayerCollider" && collision.tag == "GetHitCollider")
        {
            Debug.Log("eColliderHit");
            gm.GM_eColliderHit();
        }
            
        if (gameObject.tag != "PlayerCollider" && collision.tag == "MissCollider")
        {
            Debug.Log("eColliderMiss");
            gm.GM_eColliderMiss();
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("!!");
        //gm.GM_pColliderHit();
    }
}
