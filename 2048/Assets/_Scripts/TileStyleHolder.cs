using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileStyle {
    public int number;
    public Color32 tileColor;
    public Color32 textColor;
}

public class TileStyleHolder : MonoBehaviour {

    //SINGLETON
    public static TileStyleHolder _instance;
    public TileStyle[] tileStyles;


    private void Awake() {
        _instance = this;
    }

}
