using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents.Policies;

public class BattleState : MonoBehaviour
{
    public bool battleState = false;

    public bool BattleStateCount()
    {
        return battleState;
    }

    public void BattleStateSet(bool tf)
    {
        battleState = tf;
    }

    
}
