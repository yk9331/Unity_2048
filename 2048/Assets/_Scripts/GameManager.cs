using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
    Playing,
    GameOver,
    WaititgForMoveToEnd
}


public class GameManager : MonoBehaviour {

    public GameState currentState;
    [Range(0, 1f)]
    public float delay;
    private bool moveMade;
    private bool[] lineMoveComplete = new bool[4] { true, true, true, true };

    public Text GameOverText;
    public Text GameOverScoreText;
    public GameObject gameOverPanel;
    public GameObject continueBtn;

    private Tile[,] allTiles = new Tile[4, 4];

    private List<Tile[]> columns = new List<Tile[]>();
    private List<Tile[]> rows = new List<Tile[]>();

    private List<Tile> emptyTiles = new List<Tile>();

    //private bool isContinue;

	// Use this for initialization
	void Start () {
        //isContinue = false;
        Tile[] allTilesOneDim = GameObject.FindObjectsOfType<Tile>();
        foreach(Tile t in allTilesOneDim) {
            t.Number = 0;
            allTiles[t.indRow, t.indCol]=t;
            emptyTiles.Add(t);
        }
        columns.Add(new Tile[] { allTiles[0, 0], allTiles[1, 0], allTiles[2, 0], allTiles[3, 0] });
        columns.Add(new Tile[] { allTiles[0, 1], allTiles[1, 1], allTiles[2, 1], allTiles[3, 1] });
        columns.Add(new Tile[] { allTiles[0, 2], allTiles[1, 2], allTiles[2, 2], allTiles[3, 2] });
        columns.Add(new Tile[] { allTiles[0, 3], allTiles[1, 3], allTiles[2, 3], allTiles[3, 3] });

        rows.Add(new Tile[] { allTiles[0, 0], allTiles[0, 1], allTiles[0, 2], allTiles[0, 3] });
        rows.Add(new Tile[] { allTiles[1, 0], allTiles[1, 1], allTiles[1, 2], allTiles[1, 3] });
        rows.Add(new Tile[] { allTiles[2, 0], allTiles[2, 1], allTiles[2, 2], allTiles[2, 3] });
        rows.Add(new Tile[] { allTiles[3, 0], allTiles[3, 1], allTiles[3, 2], allTiles[3, 3] });
        Generate();
        Generate();
    }

    bool MakeOneMoveDownIndex(Tile[] LineOfTiles) {        
        for (int i = 0; i < LineOfTiles.Length-1; i++) {
            //Move Blocks
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i + 1].Number != 0) {
                LineOfTiles[i].Number = LineOfTiles[i + 1].Number;
                LineOfTiles[i + 1].Number = 0;
                return true;
            }

            //Merge Blocks
            if(LineOfTiles[i].Number!=0 && LineOfTiles[i].Number==LineOfTiles[i+1].Number&&
               !LineOfTiles[i].mergedThisTurn && !LineOfTiles[i + 1].mergedThisTurn) {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i + 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                LineOfTiles[i].PlayMergeAnimation();
                ScoreTracker._instance.Score += LineOfTiles[i].Number;
                if(LineOfTiles[i].Number == 2048) {
                    YouWon();
                }
                return true;
            }
        }
        return false;
    }

    bool MakeOneMoveUPIndex(Tile[] LineOfTiles) {        
        for (int i = LineOfTiles.Length-1; i >0; i--) {
            //MoveBlock
            if (LineOfTiles[i].Number == 0 && LineOfTiles[i - 1].Number != 0) {
                LineOfTiles[i].Number = LineOfTiles[i - 1].Number;
                LineOfTiles[i - 1].Number = 0;
                return true;
            }

            //Merge Blocks
            if (LineOfTiles[i].Number != 0 && LineOfTiles[i].Number == LineOfTiles[i - 1].Number &&
               !LineOfTiles[i].mergedThisTurn && !LineOfTiles[i - 1].mergedThisTurn) {
                LineOfTiles[i].Number *= 2;
                LineOfTiles[i - 1].Number = 0;
                LineOfTiles[i].mergedThisTurn = true;
                LineOfTiles[i].PlayMergeAnimation();
                ScoreTracker._instance.Score += LineOfTiles[i].Number;
                if (LineOfTiles[i].Number == 2048) {
                    YouWon();
                }
                return true;
            }
        }
        return false;
    }

    IEnumerator MoveOneLineDownIndexCoroutine(Tile[]line,int index) {
        lineMoveComplete[index] = false;
        while (MakeOneMoveDownIndex(line)) {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }
    IEnumerator MoveOneLineUpIndexCoroutine(Tile[] line, int index) {
        lineMoveComplete[index] = false;
        while (MakeOneMoveUPIndex(line)) {
            moveMade = true;
            yield return new WaitForSeconds(delay);
        }
        lineMoveComplete[index] = true;
    }

    private void ResetMergedFlage() {
        foreach(Tile t in allTiles) {
            t.mergedThisTurn = false;
        }
    } 

    void Generate() {
        if (emptyTiles.Count > 0) {
            int indexForNewNumber = Random.Range(0, emptyTiles.Count);
            int randomNum = Random.Range(0, 10);
            if (randomNum == 0) {
                emptyTiles[indexForNewNumber].Number = 4;
            } else {
                emptyTiles[indexForNewNumber].Number = 2;                
            }
            emptyTiles[indexForNewNumber].PlayAppearAnimation();
            emptyTiles.RemoveAt(indexForNewNumber);
        }
    }
	
	// Update is called once per frame
	/*void Update () {
        if (Input.GetKeyDown(KeyCode.G)) {
            Generate();
        }
	}*/

    private void UpdateEmptyTiles() {
        emptyTiles.Clear();
        foreach(Tile t in allTiles) {
            if(t.Number == 0) {
                emptyTiles.Add(t);
            }
        }
    }
    IEnumerator MoveCoroutine(MoveDirection md) {
        //print(md.ToString() + " move.");
        currentState = GameState.WaititgForMoveToEnd;
        switch (md) {
            case MoveDirection.UP:
                for (int i = 0; i < columns.Count; i++) {
                    StartCoroutine(MoveOneLineDownIndexCoroutine(columns[i], i));
                }
                break;
            case MoveDirection.DOWN:
                for(int i = 0; i < columns.Count; i++) {
                    StartCoroutine(MoveOneLineUpIndexCoroutine(columns[i],i));
                }                
                break;
            case MoveDirection.LEFT:
                for (int i = 0; i < rows.Count; i++) {
                    StartCoroutine(MoveOneLineDownIndexCoroutine(rows[i], i));
                }
                break;
            case MoveDirection.RIGHT:
                for (int i = 0; i < rows.Count; i++) {
                    StartCoroutine(MoveOneLineUpIndexCoroutine(rows[i], i));
                }
                break;
            default:
                Debug.LogWarning("CheckMoveIndex");
                break;

        }

        while (!(lineMoveComplete[0] && lineMoveComplete[1] && lineMoveComplete[2] && lineMoveComplete[3]))
            yield return null;
       

        if (moveMade) {
            UpdateEmptyTiles();
            Generate();
            if (!CanMove()) {
                GameOver();
            }   
        } 
        currentState = GameState.Playing;
        StopAllCoroutines();
        
    }
    public void Move(MoveDirection md) {
        //print(md.ToString() + " move.");
        moveMade = false;
        ResetMergedFlage();
        if (delay > 0) {
            StartCoroutine(MoveCoroutine(md));
        }else {
            for (int i = 0; i < rows.Count; i++) {
                switch (md) {
                    case MoveDirection.UP:
                        while (MakeOneMoveDownIndex(columns[i])) { moveMade = true; }
                        break;
                    case MoveDirection.DOWN:
                        while (MakeOneMoveUPIndex(columns[i])) { moveMade = true; }
                        break;
                    case MoveDirection.LEFT:
                        while (MakeOneMoveDownIndex(rows[i])) { moveMade = true; }
                        break;
                    case MoveDirection.RIGHT:
                        while (MakeOneMoveUPIndex(rows[i])) { moveMade = true; }
                        break;
                    default:
                        Debug.LogWarning("CheckMoveIndex");
                        break;
                }                
            }
            if (moveMade) {
                UpdateEmptyTiles();
                Generate();
                if (!CanMove()) {
                    GameOver();
                }
            }
        }        
    }
    

    public void OnNewGameBtnClick() {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    bool CanMove() {
        if (emptyTiles.Count > 0) {
            return true;
        }else {
            //Check Columns
            for(int i = 0; i < columns.Count; i++) {
                for(int j = 0; j < rows.Count - 1; j++) {
                    if (allTiles[j,i].Number == allTiles[j + 1, i].Number)
                        return true;
                }
            }

            //Check Rows
            for (int i = 0; i < rows.Count; i++) {
                for (int j = 0; j < columns.Count - 1; j++) {
                    if (allTiles[i,j].Number == allTiles[i, j+1].Number)
                        return true;
                }
            }
        }
        return false;
    }

    private void GameOver() {
        GameOverScoreText.text = ScoreTracker._instance.Score.ToString();
        continueBtn.SetActive(false);
        GameOverText.text = "GAME OVER";
        gameOverPanel.SetActive(true);

    }

    private void YouWon() {
        GameOverScoreText.text = ScoreTracker._instance.Score.ToString();
        continueBtn.SetActive(true);
        GameOverText.text = "YOU WON";
        gameOverPanel.SetActive(true);
    }

    public void OnContinueClick() {
        gameOverPanel.SetActive(false);
    }
}
