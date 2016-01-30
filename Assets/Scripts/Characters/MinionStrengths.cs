using UnityEngine;
using System;

[Serializable]
public class MinionStrength
{
    public MinionType minion;
    public MinionType strongAgainst;
}

public class MinionStrengths : MonoBehaviour
{
    public MinionStrength[] minionStrengths;

    public bool amIStrongAgainst(MinionType me, MinionType opponent)
    {
        for(int i = 0; i < minionStrengths.Length; i++)
        {
            if((minionStrengths[i].minion == me) && (minionStrengths[i].strongAgainst == opponent))
            {
                return true;
            }
        }
        return false;
    }
	
}
