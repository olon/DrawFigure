using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestLine : MonoBehaviour
{

    public LineRenderer lineRenderer;
    private List<Vector2> drawPoints;
    private List<Vector2> tempPoints;
    public List<Vector2> points;
    private float timeCount;
    private int indexRemove;

    // Use this for initialization
    void Start()
    {
        drawPoints = new List<Vector2>();
        points = new List<Vector2>();
        //lineRenderer = GetComponent<LineRenderer>();
        timeCount = 0;
        lineRenderer.SetWidth(0.2f, 0.2f);

    }

    // Update is called once per frame
    void Update()
    {

        if (drawPoints != null)
        {//myPoints != null
            lineRenderer.SetVertexCount(drawPoints.Count);
            for (int i = 0; i < drawPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, drawPoints[i]);
            }
        }
        else
        {
           // CancelInvoke();
            //drawPoints = null;
            lineRenderer.SetVertexCount(0);
        }
           

        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("AddPoint", .02f, .02f);
            InvokeRepeating("RemovePoint", 1.0f, 0.02f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke();
            drawPoints = null;
           //points = null;
        }
    }//end Update

    private void RemovePoint()
    {
        drawPoints.RemoveAt(0);
    }


    private void AddPoint()
    {
        tempPoints = new List<Vector2>();

        if (drawPoints != null)
        {
            for (int j = 0; j < drawPoints.Count; j++)
                tempPoints.Add(drawPoints[j]);
        }

        Vector3 tempPos = new Vector3();
        tempPos = Input.mousePosition;
        tempPos.z = 5;

        tempPoints.Add(Camera.main.ScreenToWorldPoint(tempPos));
        points.Add(new Vector2(tempPos.x, -tempPos.y));


        drawPoints = tempPoints;
    }

}