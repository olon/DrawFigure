using UnityEngine;
using System.Collections;

public class SingletonGameManager
{

    private static SingletonGameManager instance;

    public int points = 0;
    public float timeLeft = 40f;
    public int lastFigure = -1;

    private SingletonGameManager() { }

    public static SingletonGameManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SingletonGameManager();

            return instance;
        }

        private set { }
    }

}
