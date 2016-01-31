using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ActionSound
{
    public AudioClip sound;
    public MinionAction action;
}

public class MinionSounds : MonoBehaviour
{
    public ActionSound[] minionSounds;

    public AudioClip getSoundFromAction(MinionAction action)
    {
        for (int i = 0; i < minionSounds.Length; i++)
        {
            if (minionSounds[i].action == action)
            {
                return minionSounds[i].sound;
            }
        }
        return null;
    }
}
