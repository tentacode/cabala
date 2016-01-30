using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInfo
{
    public Touch touch;
    public GameObject touchedObject;
    public float time;
    
    public TouchInfo(Touch _touch, GameObject _touchedObject, float _time)
    {
        touch = _touch;
        touchedObject = _touchedObject;
        time = _time;
    }
}

public class TouchResult
{
    public float distance;
    public float deltaTime;
    public Vector3 direction;

    public TouchResult(float _distance, float _deltaTime, Vector3 _direction)
    {
        distance = _distance;
        deltaTime = _deltaTime;
        direction = _direction;
    }
}

public class InputManager : MonoBehaviour
{
    public float clickDistanceThreshold = 80f;

    List<TouchInfo> touchInfos = new List<TouchInfo>();
    
	void Update () 
    {
        var touches = Input.touches;
        
        foreach (Touch touch in touches) {
            if (touch.phase == TouchPhase.Began) {
                AddTouch(touch);
            } else if (touch.phase == TouchPhase.Ended) {
                Resolve(touch);
            }
        }
	}

    void AddTouch(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position); 
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitGameObject = hit.collider.gameObject;
            Swipeable swipeable = hitGameObject.GetComponent<Swipeable>();

            if (swipeable) {
                touchInfos.Add(new TouchInfo(touch, hitGameObject, Time.time));
            }
        }
    }

    void Resolve(Touch touch)
    {
        TouchInfo touchInfo = touchInfos.Find(t => t.touch.fingerId == touch.fingerId);
        if (touchInfo == null) {
            return;
        }

        Vector3 startPosition = touchInfo.touch.position;
        Vector3 newPosition = touch.position;

        float distance = Vector3.Distance(startPosition, newPosition);
        float deltaTime = Time.time - touchInfo.time;

        Vector3 direction = newPosition - startPosition;
        direction.Normalize();

        TouchResult touchResult = new TouchResult(distance, deltaTime, direction);

        Swipeable swipeable = touchInfo.touchedObject.GetComponent<Swipeable>();
        if (distance < clickDistanceThreshold) {
            swipeable.Touched(touchResult);
        } else {
            swipeable.Swipe(touchResult);
        }

        touchInfos.Remove(touchInfo);
    }
}
