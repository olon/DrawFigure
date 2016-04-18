using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameObject PanelGameController;

    public List<string> FigureName = new List<string>();

    public float _timeHelper;

    public float Time { get; set; }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if ((int)_timeHelper < 10 && _timeHelper > 0)
            PanelGameController.transform.Find("TextTimeLeft").GetComponent<Text>().color = Color.red;

        PanelGameController.transform.Find("TextTimeLeft").GetComponent<Text>().text = "Time Left: " + _timeHelper.ToString("00");
        PanelGameController.transform.Find("TextPoints").GetComponent<Text>().text = "Points: " + SingletonGameManager.Instance.points;
    }

    public void RestartGameClick()
    {
        Application.LoadLevel("StartGame");
    }
}

