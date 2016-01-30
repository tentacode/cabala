using UnityEngine;
using System.Collections;

public class Cultist : MonoBehaviour
{
    public InvocationCircleControler parentCircle;

    [SerializeField]
    private Unit_ID _unitID;

    void OnTriggerEnter(Collider other)
    {
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

    public void Die()
    {
        parentCircle.cultistDeath();
        gameObject.SetActive(false);
    }
}
