using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public delegate void GameFinished();

    public GameFinished OnGameFinished { get; set; }

    public HUD hud;

    public LevelLogic levelLogic;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GameOver()
    {
        OnGameFinished();
        hud.GameOver();
    }

    public void GameCompleted()
    {
        if(OnGameFinished != null)
            OnGameFinished();
        hud.ShowWinPanel();
        Invoke("LoadMenu", 1.5f);
    }

    private void LoadMenu()
    {
        levelLogic.LoadLevel(0);
    }
}
