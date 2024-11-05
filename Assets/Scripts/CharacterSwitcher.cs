using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSwitcher : MonoBehaviour
{
    private PlayerControls _playerControls;

    // Index 0 for Amos, 1 for Bo
    PlayerMovement[] characters = new PlayerMovement[1];

    // Start is called before the first frame update
    void Awake()
    {
        BindControlEvents();
    }

    private void BindControlEvents()
    {
        _playerControls = new PlayerControls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
