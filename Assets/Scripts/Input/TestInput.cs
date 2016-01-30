using UnityEngine;
using System.Collections;

public class TestInput : MonoBehaviour
{
	void Start ()
    {
        Swipeable swipeable = GetComponent<Swipeable>();
//        swipeable.onSwipe = OnSwipe;
//        swipeable.onTouched = OnTouched;
    }

    void OnTouched (TouchResult touchResult)
    {
        Debug.Log("Touched");
    }

    void OnSwipe (TouchResult touchResult)
    {
        Debug.Log("Swiped");
    }
}
