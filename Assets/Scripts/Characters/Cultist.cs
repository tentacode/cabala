using UnityEngine;
using System.Collections;

public class Cultist : MonoBehaviour
{
    public InvocationCircleControler parentCircle;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Minion")
        {
            if (other.GetComponent<Minions>().ownerIndex != parentCircle.ownerIndex)
            {
                other.GetComponent<Minions>().Die();
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
