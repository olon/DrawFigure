using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawLineAndAnalize : MonoBehaviour
{
    public Text testText;

    private LineRenderer lineRenderer;
    public Material lineRendererMaterial;
    public GameObject CanvasEndGame;
    public Image drawFigure;
    public InputField newFigureName;
   // public RectTransform drawField;
    Vector3 tempPos;

    private AnalizePoints analizePoints;

    private LibraryFigure libraryFigure;

    private List<Vector2> drawPoints;
    private List<Vector2> tempPoints;
    public List<Vector2> points;

    private int indexRemove;
    public int minPoints = 10;
    private static float _timeLeft = 60f;

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
        libraryFigure = new LibraryFigure("task_figure", true);
        drawPoints = new List<Vector2>();
        points = new List<Vector2>();
        RandomizeFigure();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance._timeHelper >= 0)
        {
            GameController.Instance._timeHelper = _timeLeft - Time.timeSinceLevelLoad;

            //if (Input.touchCount > 0)
            //{
            //    tempPos = new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            //}

            //else
            //{
            //    if (Input.GetMouseButton(0))
            //    {
            //        tempPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
            //    }
            //}



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




            //if (RectTransformUtility.RectangleContainsScreenPoint(drawField, tempPos, Camera.main))
            //{

                if (Input.GetMouseButtonDown(0))
                {
                    InvokeRepeating("AddPoint", .02f, .02f);
                    InvokeRepeating("RemovePoint", 3.0f, 0.08f);
                    points.Clear();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    CancelInvoke();
                    drawPoints = null;

                    if (points.Count > minPoints)
                    {
                        analizePoints = new AnalizePoints(points);
                        string result = analizePoints.AnalizeFigure(libraryFigure);
                    testText.text = result;
                    }

                }
            
        }
        else
        {
            CanvasEndGame.transform.Find("PanelEndGame").gameObject.SetActive(true);
            CanvasEndGame.transform.Find("PanelGameController").gameObject.SetActive(false);
            CanvasEndGame.transform.Find("PanelEndGame").transform.Find("TextResult").GetComponent<Text>().text = "Your Result: " + GameController.Instance.points + " points";
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

        tempPos = new Vector3();
        tempPos = Input.mousePosition;
        tempPos.z = 5;

        tempPoints.Add(Camera.main.ScreenToWorldPoint(tempPos));
        points.Add(new Vector2(tempPos.x, -tempPos.y));

        drawPoints = tempPoints;
    }

    public void AddFigure()
    {
        AnalizePoints newFigure = new AnalizePoints(points, newFigureName.text);
        libraryFigure.AddGesture(newFigure);
        //SetMessage(newGestureName.text + " has been added to the library");
    }

    private void AnalizeFigure()
    {

    }

    private void RandomizeFigure()
    {
        int random = Random.Range(0, 100);

        switch (random%5)
        {
            case 0:
                drawFigure.sprite = Resources.Load<Sprite>("circle");
                break;
            case 1:
                drawFigure.sprite = Resources.Load<Sprite>("hexagon");
                break;
            case 2:
                drawFigure.sprite = Resources.Load<Sprite>("pentagon");
                break;
            case 3:
                drawFigure.sprite = Resources.Load<Sprite>("square");
                break;
            case 4:
                drawFigure.sprite = Resources.Load<Sprite>("triangle");
                break;
        }

    }

}