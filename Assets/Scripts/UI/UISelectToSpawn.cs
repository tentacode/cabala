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

        _unit_ID.GetComponent<PlayerAuthorityScript>().CmdSpawnUnit(_typeToSpawn);
        Debug.Log("Touched");
    }



    void OnSwipe(TouchResult touchResult)
    {
        Debug.Log("Swiped");
    }
}
