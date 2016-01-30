using UnityEngine;
using System.Collections;

public class Swipeable : MonoBehaviour
{
    public delegate void TouchDelegate(TouchResult touchResult);

    public TouchDelegate onSwipe;
    public TouchDelegate onTouched;

    public void Swipe(TouchResult touchResult)
    {
        if (onSwipe != null) {
            onSwipe(touchResult);
        } else {
            Debug.LogError("Delegate for onSwipe was not defined on GameObject: " + gameObject.name);
        }
    }

    public void Touched(TouchResult touchResult)
    {
        if (onTouched != null) {
            onTouched(touchResult);
        } else {
            Debug.LogError("Delegate for onTouched was not defined on GameObject: " + gameObject.name);
        }
    }
}
