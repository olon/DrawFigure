using UnityEngine;
using System.Collections;

public class PanelStartMenu : MonoBehaviour {

    public void StartGameClick()
    {
        Application.LoadLevel("StartGame");
    }

}
