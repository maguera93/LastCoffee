using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    private PlayerBehaviour m_player;
    private HUD m_hud;
    private bool pause;

	// Use this for initialization
	void Start () {
        m_player = FindObjectOfType<PlayerBehaviour>();
        m_hud = FindObjectOfType<HUD>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        m_player.Shoot(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        m_player.Move(new Vector2(Input.GetAxis("HorizontalWASD"), Input.GetAxis("VerticalWASD")));
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause = !pause;
            m_hud.Pause(pause);
        }
    }
}
