using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class UISelectToSpawn : NetworkBehaviour{

    [SerializeField]
    private Unit_ID _unit_ID;

    [SerializeField]
    private MinionType _typeToSpawn;

    void Start()
    {
        Swipeable swipeable = GetComponent<Swipeable>();
                swipeable.onSwipe = OnSwipe;
                swipeable.onTouched = OnTouched;
    }

    void OnTouched(TouchResult touchResult)
    {

        CmdSpawnUnit();
        Debug.Log("Touched");
    }

    [Command]
    private void CmdSpawnUnit()
    {
        Unit_ID.FindPlayer(_unit_ID.GetPlayerIndex()).GetComponent<SpawnerController>().spawnedCharacter = _typeToSpawn;
    }

    void OnSwipe(TouchResult touchResult)
    {
        Debug.Log("Swiped");
    }
}
