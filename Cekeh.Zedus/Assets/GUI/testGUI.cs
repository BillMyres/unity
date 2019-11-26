using UnityEngine;
using System.Collections;

public class testGUI : MonoBehaviour {

    public GUISkin MenuSkin;

    bool toggleTxt;
    int toolbarInt = 0, selGridInt = 0;
    string[] toolbarStrings = { "toolbar1", "toolbar2", "toolbar3" };
    string[] selStrings = { "grid1", "grid2", "grid3" };
    float hSliderValue = 0.0f, hSbarValue = 0f;

    void OnGUI() {
        GUI.skin = MenuSkin;
        GUI.BeginGroup(new Rect(0, 0, 300, 300));
        GUI.Box(new Rect(0, 0, 300, 64), "This is the title of a box");
        GUI.Button(new Rect(0, 25, 100, 20), "I am a button");
        GUI.Label(new Rect(0, 50, 100, 20), "I'm a Label!");
        toggleTxt = GUI.Toggle(new Rect(0, 75, 200, 30), toggleTxt, "I am a Toggle button");
        toolbarInt = GUI.Toolbar(new Rect(0, 110, 250, 25), toolbarInt, toolbarStrings);
        selGridInt = GUI.SelectionGrid(new Rect(0, 160, 200, 40), selGridInt, selStrings, 2);
        hSliderValue = GUI.HorizontalSlider(new Rect(0, 210, 100, 30), hSliderValue, 0.0f, 1.0f);
        hSbarValue = GUI.HorizontalScrollbar(new Rect(0, 230, 100, 30), hSbarValue, 1.0f, 0.0f, 10.0f);
        GUI.EndGroup();
    }
}
