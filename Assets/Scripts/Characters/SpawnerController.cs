using UnityEngine;
using System.Collections;

public class SpawnerController : MonoBehaviour
{
    #region parameters
    public int ownerNumber;
    public Ennemy spawnedCharacter;
    public AnimationCurve spawnRateCurve;
    public float TimeBeforeFirstLaunch;
    public float maxTime;

    private float beginTime;

    #endregion
    
    // Use this for initialization
    void Start () {
        Invoke("FirstSpawn", TimeBeforeFirstLaunch);
    }
	
	// Update is called once per frame
	void Update () {
        float x = spawnRateCurve.Evaluate(Time.time - beginTime);
	}

    private void FirstSpawn()
    {
        beginTime = Time.time;
        Spawn();
    }

    private void Spawn()
    {
        Ennemy ennemy = Instantiate(spawnedCharacter, transform.position, transform.rotation) as Ennemy;
        
        ennemy.SetOwnerNumber(ownerNumber);


        Invoke("Spawn", spawnRateCurve.Evaluate((Time.time - beginTime) / maxTime));
    }
}
