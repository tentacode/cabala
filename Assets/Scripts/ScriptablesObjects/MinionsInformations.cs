using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MinionsInformations : ScriptableObject
{
    [Header("Combat Stats")]
    public int baseLifePoints;

    [Tooltip("In seconds")]
    public float attackSpeed;

    [Tooltip("first attack in each fight, in seconds")]
    public float firstAttackSpeed;

    public int damages;

    [Tooltip("Damage dealt to units weak againts this unit")]
    public float damageMultiplicator;

    [Header("health bars teams material")]
    public Material[] healBarTeamMaterials;

    [Header("Minions Strenghts")]
    public MinionStrengths minionStrengths;
}
