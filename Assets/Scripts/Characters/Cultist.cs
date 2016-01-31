using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Cultist : NetworkBehaviour
{
    public InvocationCircleControler parentCircle;

    [SerializeField]
    private Unit_ID _unitID;

    void OnTriggerEnter(Collider other)
    {
        if (!isServer)
        {
            return;
        }

        if (other.tag == "Minion")
        {
            if (other.GetComponent<Unit_ID>().GetPlayerIndex() != _unitID.GetPlayerIndex())
            {
                other.GetComponent<Destructible>().CmdTakeDamage(1000);
                Die();

            }
        }
    }

    public void init(InvocationCircleControler parent)
    {
        parentCircle = parent;
    }

    private void Die()
    {
        GetComponent<MonoSoundable>().playSound();
        parentCircle.cultistDeath();
        gameObject.SetActive(false);
    }
}
