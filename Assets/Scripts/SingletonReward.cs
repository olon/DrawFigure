using UnityEngine;
using System.Collections;

public class SingletonReward : MonoBehaviour {

    public static SingletonReward Instance;

    public float _timeHelper;

    void Start()
    {
        Instance = this;
    }

}
