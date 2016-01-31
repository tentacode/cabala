using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIDeath : MonoBehaviour {

    [SerializeField]
    private List<GameObject> _objectDeaths;

    [SerializeField]
    private Button _buttonGameOver;

    [SerializeField]
    private Button _buttonGameOver2;

    [SerializeField]
    private Text _text;

    void Start()
    {

        foreach (var c in _objectDeaths)
         {
             c.SetActive(false) ;
         }
    }

    public void Activate(bool v)
    {
        foreach (var c in _objectDeaths)
        {
            c.SetActive(c);
        }

        if (GameSharedData.GetLocalPlayer().GetComponent<PlayerAuthorityScript>().cultisteLife > 0)
        {
            _text.text = "You Win !";

            _buttonGameOver.gameObject.SetActive(true);
            _buttonGameOver2.gameObject.SetActive(true);
        }
        else
        {
            _text.text = "";

            _buttonGameOver.gameObject.SetActive(false);
            _buttonGameOver2.gameObject.SetActive(false);
        }
        
    }

    void Update()
    {
        transform.localPosition = Vector3.zero;
    }

    public void ButtonGameAgain()
    {
        Activate(false);
        
    }

    public void ButtonGoLobby()
    {
        GameObject.Find("NetworkLobbyManager").GetComponent<LANLobbyNetworkManager>().SendReturnToLobby();
    }
}
