using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public GameObject PanelGameController;

    public bool helpTimerFlage = false;
    public float _timeHelper;
    public int points = 0;

    public float Time { get; set; }

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if ((int)_timeHelper < 1 && _timeHelper > 0)
            PanelGameController.transform.Find("TextTimeLeft").GetComponent<Text>().color = Color.red;

        PanelGameController.transform.Find("TextTimeLeft").GetComponent<Text>().text = "Time Left: " + _timeHelper.ToString("00");
        PanelGameController.transform.Find("TextPoints").GetComponent<Text>().text = "Points: " + points;
    }

    public void RestartGameClick()
    {
        Application.LoadLevel("StartGame");
    }
}

