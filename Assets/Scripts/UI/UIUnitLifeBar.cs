using UnityEngine;
using System.Collections;

public class UIUnitLifeBar : MonoBehaviour {

    private GameObject lifeBar;

    private Destructible _destructible;

	// Use this for initialization
	void Start () {
        _destructible = GetComponent<Destructible>();
        lifeBar = transform.FindChild("LifeBar").gameObject;
	}

    void LateUpdate()
    {
        if (lifeBar == null)
        {
            return;
        }

        lifeBar.transform.localScale = new Vector3(lifeBar.transform.localScale.x,
                                                   lifeBar.transform.localScale.y,
                                                   (float)_destructible.GetLife() / (float)_destructible.GetMaxLife());
    }
}
