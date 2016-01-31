using UnityEngine;
using System.Collections;

public class MinionOrders : MonoBehaviour {

    PlayerAuthorityScript _localPlayer;

    void Start()
    {
        GameObject player = LANLobbyNetworkManager.GetLocalPlayer();

        // don't do it for other players
        if (player.GetComponent<Unit_ID>().GetPlayerIndex() != GetComponent<Unit_ID>().GetPlayerIndex())
        {
            enabled = false;
            return;
        }

        _localPlayer = player.GetComponent<PlayerAuthorityScript>();
        Swipeable swipeable = GetComponent<Swipeable>();

        swipeable.onSwipe = OnSwipe;
        swipeable.onTouched = OnTouched;


    }

    void OnTouched(TouchResult touchResult)
    {
        _localPlayer.CmdOrderMinionToStop(name);
     }

    void OnSwipe(TouchResult touchResult)
    {
        Debug.Log("Swiped");
    }
}
