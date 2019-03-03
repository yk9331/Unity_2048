using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputManager : MonoBehaviour {

    private float fingerStartTime = 0.0f;
    private Vector2 fingerStartPos = Vector2.zero;

    private bool isSwip = false;
    private float minSwipDist = 50.0f;
    private float maxSwipTime = 1.5f;

    private GameManager gameManager;

    private void Awake() {
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameManager.currentState == GameState.Playing && Input.touchCount > 0) {
            foreach(Touch touch in Input.touches) {
                switch (touch.phase) {
                    case TouchPhase.Began:
                        isSwip = true;
                        fingerStartTime = Time.time;
                        fingerStartPos = touch.position;
                        break;
                    case TouchPhase.Canceled:
                        isSwip = false;
                        break;
                    case TouchPhase.Ended:
                        float gestureTime = Time.time - fingerStartTime;
                        float gestureDist = (touch.position - fingerStartPos).magnitude;
                        if (isSwip && gestureTime < maxSwipTime && gestureDist > minSwipDist) {
                            Vector2 direction = touch.position - fingerStartPos;
                            Vector2 swipeType = Vector2.zero;

                            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) {
                                swipeType = Vector2.right * Mathf.Sign(direction.x);
                            }else {
                                swipeType = Vector2.up * Mathf.Sign(direction.y);
                            }

                            if(swipeType.x != 0.0f){
                                if (swipeType.x > 0.0f) {
                                    gameManager.Move(MoveDirection.RIGHT);
                                } else {
                                    gameManager.Move(MoveDirection.LEFT);
                                }
                            } 
                            
                            if(swipeType.y!=0.0f) {
                                if (swipeType.y > 0.0f) {
                                    gameManager.Move(MoveDirection.UP);
                                } else {
                                    gameManager.Move(MoveDirection.DOWN);
                                }
                            }
                        }
                        break;
                }
            }
        }
	}
}
