using UnityEngine;
using System.Collections;

public class SpawnerController : MonoBehaviour
{
    #region parameters
    public int ownerNumber;
    public Minions spawnedCharacter;

    public SpawnersInformations spawnerInformations;

    private float beginTime;

    #endregion
    
    // Use this for initialization
    void Start () {
        Invoke("FirstSpawn", spawnerInformations.TimeBeforeFirstLaunch);
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    private void FirstSpawn()
    {
        beginTime = Time.time;
        Spawn();
    }

    private void Spawn()
    {
        Minions ennemy = Instantiate(spawnedCharacter, transform.position, transform.rotation) as Minions;
        
        ennemy.SetOwnerNumber(ownerNumber);
        
        Invoke("Spawn", spawnerInformations.spawnSpeedCurve.Evaluate((Time.time - beginTime) / spawnerInformations.maxTime));
    }
}
