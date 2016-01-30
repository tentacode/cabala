using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnersInformations : ScriptableObject
{
    [Tooltip("Spawn Speed in seconds")]
    public AnimationCurve spawnSpeedCurve;

    public float TimeBeforeFirstLaunch;
    public float maxTime;
}
