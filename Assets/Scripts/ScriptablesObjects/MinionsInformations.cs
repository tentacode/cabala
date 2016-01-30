using UnityEngine;
using System.Collections;

[System.Serializable]
public class MinionsInformations : ScriptableObject
{
    public int baseLifePoint;

    [Tooltip("In seconds")]
    public float attackSpeed;
    
    public int damages;

    [Tooltip("Damage dealt to units weak againts this unit")]
    public float damageMultiplicator;
}
