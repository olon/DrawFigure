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

    Vector3 tempPos;

    private AnalizePoints analizePoints;

    private LibraryFigure libraryFigure;

    private List<Vector2> drawPoints;
    private List<Vector2> tempPoints;
    private List<Vector2> helpPoints;
    public List<Vector2> points;

    private int indexRemove;
    public int minPoints = 50;
    private int currentNumberFigure;
    private string currentNameFigure;

    void Awake()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.SetVertexCount(0);
        lineRenderer.material = lineRendererMaterial;
        lineRenderer.SetWidth(0.2f, 0.2f);
        libraryFigure = new LibraryFigure("task_figure", false);
    }

    void Start()
    {
        drawPoints = new List<Vector2>();
        points = new List<Vector2>();
        //helpPoints = new List<Vector2>();

        RandomizeFigure();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance._timeHelper >= 0)
        {
            GameController.Instance._timeHelper = SingletonGameManager.Instance.timeLeft - Time.timeSinceLevelLoad;

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
                InvokeRepeating("RemovePoint", 3.0f, 0.1f);
                points.Clear();
            }
            if (Input.GetMouseButtonUp(0))
            {
                CancelInvoke();
                drawPoints = null;

                if (points.Count > minPoints )
                {
                    helpPoints = new List<Vector2>(points);

                    analizePoints = new AnalizePoints(points);
                    string result = analizePoints.AnalizeFigure(libraryFigure);

                    if (currentNameFigure == result)
                    {
                        SingletonGameManager.Instance.points += 1;

                        if (SingletonGameManager.Instance.timeLeft > 20)
                            SingletonGameManager.Instance.timeLeft -= 5f;
                        if (SingletonGameManager.Instance.timeLeft <= 20 && SingletonGameManager.Instance.timeLeft > 10)
                            SingletonGameManager.Instance.timeLeft -= 2f;

                        GameController.Instance.RestartGameClick();
                    }

                }

            }

        }
        // If the time is over.
        else
        {
            if (!CanvasEndGame.transform.Find("PanelEndGame").gameObject.activeSelf)
            {
                SingletonGameManager.Instance.timeLeft = 40f;

                CanvasEndGame.transform.Find("PanelEndGame").gameObject.SetActive(true);
                CanvasEndGame.transform.Find("PanelTaskAndDrawFigure").gameObject.SetActive(false);

                int points = SingletonGameManager.Instance.points;
                CanvasEndGame.transform.Find("PanelEndGame").transform.Find("TextResult").GetComponent<Text>().text = "Your Result: " + points + " points";
                SingletonGameManager.Instance.points = 0;
            }
            else
                return;
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

    //The method for adding new points of figures in Unity.
    public void AddFigure()
    {
        if (helpPoints != null)
        {
            AnalizePoints newFigure = new AnalizePoints(helpPoints, newFigureName.text);
            libraryFigure.AddGesture(newFigure);

            testText.text = "The figure is added!!!";
        }
        else
        {
            testText.text = "The figure is not added!!!";
        }
    }

    private void RandomizeFigure()
    {
        int random = Random.Range(0, 100);

        currentNumberFigure = random % GameController.Instance.FigureName.Count;

        if (SingletonGameManager.Instance.lastFigure == currentNumberFigure)
            RandomizeFigure();
        else
        {
            currentNameFigure = GameController.Instance.FigureName[currentNumberFigure];

            drawFigure.sprite = Resources.Load<Sprite>(currentNameFigure);

            SingletonGameManager.Instance.lastFigure = currentNumberFigure;
        }


    }

}