using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Globalization;
using System;

public class LibraryFigure
{
    string libraryFigureName;
    string libraryFigureFilename;
    string libraryFigurePath;
    string resourcesFigurePath;
    XmlDocument xmlLibrary = new XmlDocument();
    List<AnalizePoints> libraryFigurePoints = new List<AnalizePoints>();

    public List<AnalizePoints> LibraryFigurePoints { get { return libraryFigurePoints; } }

    public LibraryFigure(string libraryFigureName, bool forceCopy = false)
    {
        this.libraryFigureName = libraryFigureName;
        this.libraryFigureFilename = libraryFigureName + ".xml";
        this.libraryFigurePath = Path.Combine(Application.persistentDataPath, libraryFigureFilename);
        this.resourcesFigurePath = Path.Combine(Path.Combine(Application.dataPath, "Resources"), libraryFigureFilename);

        CopyToPersistentPath(forceCopy);
        LoadLibraryFigure();
    }

    public void LoadLibraryFigure()
    {
        string xmlContents = "";
        string floatSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

#if !UNITY_EDITOR
        xmlContents = FileHelper.Read(libraryFigurePath);
#else
        xmlContents = Resources.Load<TextAsset>(libraryFigureName).text;
#endif

        xmlLibrary.LoadXml(xmlContents);

        // Get "figure" elements
        XmlNodeList xmlList = xmlLibrary.GetElementsByTagName("figure");

        // Parse "figure" elements and add them to library
        foreach (XmlNode xmlGestureNode in xmlList)
        {
            string figureName = xmlGestureNode.Attributes.GetNamedItem("name_figure").Value;

            GameController.Instance.FigureName.Add(figureName);

            XmlNodeList xmlPoints = xmlGestureNode.ChildNodes;
            List<Vector2> figurePoints = new List<Vector2>();

            foreach (XmlNode point in xmlPoints)
            {
                Vector2 figurePoint = new Vector2();
                figurePoint.x = (float)System.Convert.ToDouble(point.Attributes.GetNamedItem("x").Value.Replace(",", floatSeparator).Replace(".", floatSeparator));
                figurePoint.y = (float)System.Convert.ToDouble(point.Attributes.GetNamedItem("y").Value.Replace(",", floatSeparator).Replace(".", floatSeparator));
                figurePoints.Add(figurePoint);
            }

            AnalizePoints analizePoints = new AnalizePoints(figurePoints, figureName);
            libraryFigurePoints.Add(analizePoints);
        }
    }

    void CopyToPersistentPath(bool forceCopy)
    {
        if (!FileHelper.Exists(libraryFigurePath) || (FileHelper.Exists(libraryFigurePath) && forceCopy))
        {
            string fileContents = Resources.Load<TextAsset>(libraryFigureName).text;
            FileHelper.Write(libraryFigurePath, fileContents);
        }
    }

    public bool AddGesture(AnalizePoints figure)
    {

        // Create the xml node to add to the xml file
        XmlElement rootElement = xmlLibrary.DocumentElement;
        XmlElement figureNode = xmlLibrary.CreateElement("figure");
        figureNode.SetAttribute("name_figure", figure.FigureName);

        foreach (Vector2 v in figure.Points)
        {
            XmlElement figurePoint = xmlLibrary.CreateElement("point");
            figurePoint.SetAttribute("x", v.x.ToString());
            figurePoint.SetAttribute("y", v.y.ToString());

            figureNode.AppendChild(figurePoint);
        }

        // Append the node to xml file contents
        rootElement.AppendChild(figureNode);

        try
        {
            this.libraryFigurePoints.Add(figure);

#if !UNITY_EDITOR
            FileHelper.Write(libraryFigurePath, xmlLibrary.OuterXml);
#elif UNITY_EDITOR
            FileHelper.Write(resourcesFigurePath, xmlLibrary.OuterXml);
#endif

            return true;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }

    }
}
