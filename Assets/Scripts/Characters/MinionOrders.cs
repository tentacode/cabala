﻿using UnityEngine;
using System.Collections;

public class MinionOrders : MonoBehaviour {

    Unit_ID unitID;

    void Start()
    {
        unitID = GetComponent<Unit_ID>();
    }

    bool IsInit = false;
    void Update()
    {
        if (IsInit || !unitID.IsReady())
        {
            return;
        }
        IsInit = true;

        GameObject player = GameSharedData.GetLocalPlayer();

        Debug.Log("LocalPlayer " + player.GetComponent<Unit_ID>().GetPlayerIndex() + " " + GetComponent<Unit_ID>().GetPlayerIndex());
        // don't do it for other players
        if (player.GetComponent<Unit_ID>().GetPlayerIndex() != GetComponent<Unit_ID>().GetPlayerIndex())
        {
            enabled = false;
            return;
        }

        Swipeable swipeable = GetComponent<Swipeable>();

        swipeable.onSwipe = OnSwipe;
        swipeable.onTouched = OnTouched;
    }

    void OnTouched(TouchResult touchResult)
    {
        GameSharedData.GetLocalPlayer().GetComponent<PlayerAuthorityScript>().CmdOrderMinionToStop(name);
     }

    void OnSwipe(TouchResult touchResult)
    {
        float dot = Vector2.Dot(new Vector2(0, 1), touchResult.direction);
        float angle = Vector2.Angle(new Vector2(0.5f, 0.5f), touchResult.direction);

        if (dot < 0)
        {
            angle = 360 - angle;
        }

        int trancheDangle;
        // Base
        if (angle > 270)
        {
            trancheDangle = 0;
        }
        else if (angle >= 0 && angle <= 110)
        {
            trancheDangle = 1;
        }
        else if (angle > 110 && angle <= 160)
        {
            trancheDangle = 2;
        }
        else
        {
            trancheDangle = 3;
        }

        string nameToGo = GameSharedData.GetPlayerNumberNext(GameSharedData.GetLocalPlayer().GetComponent<Unit_ID>(), trancheDangle).name;

        GameSharedData.GetLocalPlayer().GetComponent<PlayerAuthorityScript>().CmdOrderMinionToMoveTo(name, nameToGo);
    }
}