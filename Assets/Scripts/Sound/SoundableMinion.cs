using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class SoundableMinion : SoundableElement
{
    public MinionsSoundsInformations soundInfos;

    private MinionSounds minionSounds;
    
	override protected void Start ()
    {
        base.Start();

        minionSounds = soundInfos.getAccordingMinionSounds(GetComponent<Minions>().minionType);
	}

    public void PlaySound(MinionAction action)
    {
        ActionSound actionSound = minionSounds.getActionSoundFromAction(action);
        LaunchSound(actionSound.sound, actionSound.mixerGroup);
    }
}
