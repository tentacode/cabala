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
        float dot = Vector2.Dot(new Vector2(-0.5f, 0.5f), touchResult.direction);

        // Base
        if (dot < 0)
        {
            _localPlayer.CmdOrderMinionToMoveTo(name, _localPlayer.name);
        }
        else
        {
            float angle = Vector2.Angle(new Vector2(0.5f, 0.5f), touchResult.direction);

            int trancheDangle = Mathf.FloorToInt(angle / 60);

            trancheDangle++;
            Debug.Log(trancheDangle);

            string nameToGo = GameSharedData.GetPlayerNumberNext(_localPlayer.GetComponent<Unit_ID>(), trancheDangle).name;
            Debug.Log(nameToGo);

            _localPlayer.CmdOrderMinionToMoveTo(name, nameToGo);
        }

        

        Debug.Log("Swiped");
    }
}
