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

        setupMinionSounds();
	}

    private void setupMinionSounds()
    {
        minionSounds = soundInfos.getAccordingMinionSounds(GetComponent<Minions>().minionType);
    }

    public void PlaySound(MinionAction action)
    {
        if(minionSounds == null)
        {
            setupMinionSounds();
        }

        ActionSound actionSound = minionSounds.getActionSoundFromAction(action);
        Debug.Log("Playing Sound " + actionSound.sound.name);
        LaunchSound(actionSound.sound, actionSound.mixerGroup);
    }
}
