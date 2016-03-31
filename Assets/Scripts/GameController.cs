using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public GameObject ButtonStartGame;
    public GameObject TextTimeLeft;

    public void StartGame()
    {
        ButtonStartGame.SetActive(false);
        TextTimeLeft.SetActive(true);
    }
}

