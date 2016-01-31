using UnityEngine;
using System.Collections;

public class MinionParticlesController : MonoBehaviour
{
    public ParticleSystem ghostAttack;
    public ParticleSystem warriorAttack;
    public ParticleSystem wizardAttack;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void displayAttack(MinionType attackerType)
    {
        switch(attackerType)
        {
            case MinionType.Ghost:
                ghostAttack.Emit(50);
                break;

            case MinionType.Warrior:
                warriorAttack.Emit(1);
                break;

            case MinionType.Wizard:
                wizardAttack.Emit(20);
                break;
        }
    }
}
