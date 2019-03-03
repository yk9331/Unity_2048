using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveDirection {
    LEFT, RIGHT, UP, DOWN
}

public class InputManager : MonoBehaviour {
    private GameManager gameManager;

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(gameManager.currentState == GameState.Playing) {
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                gameManager.Move(MoveDirection.RIGHT);
            } else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                gameManager.Move(MoveDirection.LEFT);
            } else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                gameManager.Move(MoveDirection.UP);
            } else if (Input.GetKeyDown(KeyCode.DownArrow)) {
                gameManager.Move(MoveDirection.DOWN);
            }
        }
        
    }
}
