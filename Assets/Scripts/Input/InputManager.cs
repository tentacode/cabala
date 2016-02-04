using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TouchInfo
{
    public Vector2 touchPos;
    public int touchID;
    public GameObject touchedObject;
    public float time;
    
    public TouchInfo(Vector2 touchPos, int touchId, GameObject _touchedObject, float _time)
    {
        this.touchID = touchId;
        this.touchPos = touchPos;
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
    public float clickDistanceThreshold = 40f;

    List<TouchInfo> touchInfos = new List<TouchInfo>();

    Vector2 _origineMouse;

    bool mouseOnClick = false;
    
	void Update () 
    {
        var touches = Input.touches;

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IPHONE)
        foreach (Touch touch in touches) {
            if (touch.phase == TouchPhase.Began) {
                AddTouch(touch);
            } else if (touch.phase == TouchPhase.Ended) {
                Resolve(touch.position, touch.fingerId);
            }
        }
#else
        if (Input.GetButtonDown("Fire1"))
        {
            AddMouse();
            
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            Resolve(Input.mousePosition, -1);
        }
#endif
	}

    void AddMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitGameObject = hit.collider.gameObject;
            Swipeable swipeable = hitGameObject.GetComponent<Swipeable>();

            if (swipeable)
            {
                touchInfos.Add(new TouchInfo(Input.mousePosition, -1, hitGameObject, Time.time));
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
                touchInfos.Add(new TouchInfo(touch.position, touch.fingerId, hitGameObject, Time.time));
            }
        }
    }

    void Resolve(Vector2 touchPos, int fingerID)
    {
        TouchInfo touchInfo = touchInfos.Find(t => t.touchID == fingerID);
        if (touchInfo == null) {
            return;
        }

        Vector3 startPosition = touchInfo.touchPos;
        Vector3 newPosition = touchPos;

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
