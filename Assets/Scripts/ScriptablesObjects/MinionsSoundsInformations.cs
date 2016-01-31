using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinionsSoundsInformations : ScriptableObject
{
    public MinionSounds[] minionSounds;

    public MinionSounds getAccordingMinionSounds(MinionType minionType)
    {
        for(int i = 0; i < minionSounds.Length; i++)
        {
            if(minionSounds[i].minionType == minionType)
            {
                return minionSounds[i];
            }
        }
        return null;
    }
}
