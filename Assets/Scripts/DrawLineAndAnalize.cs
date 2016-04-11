using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLineAndAnalize : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Material lineRendererMaterial;

    private AnalizePoints analizePoints;

    private List<Vector2> drawPoints;
    private List<Vector2> tempPoints;
    public List<Vector2> points;

    private float timeCount;
    private int indexRemove;
    public int minPoints = 10;

    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.SetWidth(0.2f, 0.2f);
    }

    // Use this for initialization
    void Start()
    {
        
        drawPoints = new List<Vector2>();
        points = new List<Vector2>();
        timeCount = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if (drawPoints != null)
        {
            lineRenderer.SetVertexCount(drawPoints.Count);
            for (int i = 0; i < drawPoints.Count; i++)
            {
                lineRenderer.SetPosition(i, drawPoints[i]);
            }
        }
        else
        {
            lineRenderer.SetVertexCount(0);
        }
           

        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("AddPoint", .02f, .02f);
            InvokeRepeating("RemovePoint", 2.0f, 0.02f);
            points.Clear();
        }
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke();
            drawPoints = null;

            if(points.Count > minPoints)
            {
                analizePoints = new AnalizePoints(points);
            }

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