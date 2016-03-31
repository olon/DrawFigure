using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SaveAndAnalizePoints : MonoBehaviour {

    public LineRenderer lineRend;
    public GameObject ButtonStartGame;

    private RuntimePlatform platform;

    public List<Vector2> points = new List<Vector2>();
    private Vector3 positionKey = Vector2.zero;

    private int vertexCount = 0;

    void Start()
    {
        platform = Application.platform;
    }

    void Update()
    {
        if (ButtonStartGame.activeInHierarchy)
            return;

        if (platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                positionKey = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
        }
        else {
            if (Input.GetMouseButton(0))
            {
                positionKey = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
                
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            //InvokeRepeating("RemovePoint", 3.0f, 1.0f);
            points.Clear();
            lineRend.SetVertexCount(0);
            vertexCount = 0;
        }

        if (Input.GetMouseButton(0))
            {
                points.Add(new Vector2(positionKey.x, -positionKey.y));
                lineRend.SetVertexCount(++vertexCount);
                lineRend.SetPosition(vertexCount - 1, WorldCoordinate(positionKey));
               // InvokeRepeating("RemovePoint", 1.0f, 0.02f);
            }

        //if (Input.GetMouseButtonUp(0))
        //{
        //    Gesture g = new Gesture(points);
        //    Result result = g.Recognize(gl, true);

        //    message = result.Name + "; " + result.Score;
        //}
    }

    private Vector3 WorldCoordinate(Vector3 point)
    {
        Vector3 worldCoordinate = new Vector3(point.x, point.y, 10);
        return Camera.main.ScreenToWorldPoint(worldCoordinate);
    }

}
