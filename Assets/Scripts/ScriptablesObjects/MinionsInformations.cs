using UnityEngine;
using System.Collections;

[System.Serializable]
public class MinionsInformations : ScriptableObject
{
    public int baseLifePoints;

    [Tooltip("In seconds")]
    public float attackSpeed;

    [Tooltip("first attack in each fight, in seconds")]
    public float firstAttackSpeed;

    public int damages;

    [Tooltip("Damage dealt to units weak againts this unit")]
    public float damageMultiplicator;
}
