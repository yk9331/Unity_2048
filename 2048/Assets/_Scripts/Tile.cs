using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

    public bool mergedThisTurn = false;

    public int indRow;
    public int indCol;

    
    private int number;
    public int Number {
        get {
            return number;
        }
        set {
            number = value;
            if (number == 0) {
                SetEmpty();
            } else {
                ApplyStyle(number);
                SetVisible();
            }
        }
    }
    

    private Text tileText;
    private Image tileImage;
    private Animator anim;

    private void Awake() {
        tileText = GetComponentInChildren<Text>();
        tileImage = transform.Find("NumberedCell").GetComponent<Image>();
        anim = GetComponent<Animator>();
    }

    public void PlayMergeAnimation() {
        anim.SetTrigger("Merge");
    }
    public void PlayAppearAnimation() {
        anim.SetTrigger("Appear");
    }

    void ApplyStyleFromHolder(int index) {
        tileText.text = TileStyleHolder._instance.tileStyles[index].number.ToString();
        tileText.color = TileStyleHolder._instance.tileStyles[index].textColor;
        tileImage.color = TileStyleHolder._instance.tileStyles[index].tileColor;
    }

    void ApplyStyle(int num) {
        switch (num) {
            case 2:
                ApplyStyleFromHolder(0);
                break;
            case 4:
                ApplyStyleFromHolder(1);
                break;
            case 8:
                ApplyStyleFromHolder(2);
                break;
            case 16:
                ApplyStyleFromHolder(3);
                break;
            case 32:
                ApplyStyleFromHolder(4);
                break;
            case 64:
                ApplyStyleFromHolder(5);
                break;
            case 128:
                ApplyStyleFromHolder(6);
                break;
            case 256:
                ApplyStyleFromHolder(7);
                break;
            case 512:
                ApplyStyleFromHolder(8);
                break;
            case 1024:
                ApplyStyleFromHolder(9);
                break;
            case 2048:
                ApplyStyleFromHolder(10);
                break;
            case 4096:
                ApplyStyleFromHolder(11);
                break;
            case 8192:
                ApplyStyleFromHolder(12);
                break;
            case 16384:
                ApplyStyleFromHolder(13);
                break;
            case 32768:
                ApplyStyleFromHolder(14);
                break;
            default:
                Debug.LogError("Check the Number pass to ApplyStyle");
                break;

        }
    }

    private void SetVisible() {
        tileImage.enabled = true;
        tileText.enabled = true;
    }

    private void SetEmpty() {
        tileImage.enabled = false;
        tileText.enabled = false;
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
