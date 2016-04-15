using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnalizePoints {

    private int numberOfPoints = 64;
    private float sizeOfSquare = 250f;

    public List<Vector2> Points { get; set; }

    public string FigureName { get; set; }

    public List<float> Vector { get; set; }

    public float CenterAngel { get; set; }

    public AnalizePoints(List<Vector2> points, string figureName = "")
    {
        this.Points = points;
        this.FigureName = figureName;
        this.CenterAngel = AnalizePoints.GetCenterAngle(points);
        this.Points = this.Resample(numberOfPoints);
        this.Points = this.HelpRotate(-this.CenterAngel);
        this.Points = this.HelpScale(sizeOfSquare);
        this.Points = this.HelpTranslate(Vector2.zero);
        this.Vector = this.HelpVector();
    }

    public string AnalizeFigure(LibraryFigure libraryFigure)
    {

        if (this.Points.Count <= 2)
        {
            return "Not enough points captured";
        }
        else
        {
            List<AnalizePoints> libraryPoints = libraryFigure.LibraryFigurePoints;

            float bestDistance = float.MaxValue;
            int matchedFigure = -1;

            for (int i = 0; i < libraryPoints.Count; i++)
            {
                float distance = 0;

                //if (useProtractor)
                //{
                    // See ProtractorAlgorithm() method's comments to find out more about it.
                    distance = ProtractorAlgorithm(libraryPoints[i].Vector, this.Vector);
                //}
                //else
                //{
                //    // See DollarOneAlgorithm() method's comments to find out more about it.
                //    distance = DollarOneAlgorithm(libraryPoints[i], -this.ANGLE_RANGE, +this.ANGLE_RANGE, this.ANGLE_PRECISION);
                //}

                // If distance is better than the best distance take it as the best distance, 
                // and gesture as the recognized one.
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    matchedFigure = i;
                }
            }

            // No match, score zero. If there is a match, send the name of the recognized gesture and a score.
            if (matchedFigure == -1)
            {
                return "No match";
            }
            else
            {
                return libraryPoints[matchedFigure].FigureName;
            }
        }
    }


    // This algorithm uses the nearest neighbor approach. Protractor converts gestures into 
    // equal length vectors and calculates their optimal angular distance. 

    public float ProtractorAlgorithm(List<float> figureVector, List<float> otherFigureVector)
    {
        float a = 0f;
        float b = 0f;

        for (int i = 0; i < figureVector.Count; i += 2)
        {
            a += figureVector[i] * otherFigureVector[i] + figureVector[i + 1] * otherFigureVector[i + 1];
            b += figureVector[i] * otherFigureVector[i + 1] - figureVector[i + 1] * otherFigureVector[i];
        }

        float angle = Mathf.Atan(b / a);
        return Mathf.Acos(a * Mathf.Cos(angle) + b * Mathf.Sin(angle));
    }

    // Creates a vector representation for the gesture.
    public List<float> HelpVector()
    {
        float sum = 0f;
        List<float> helpVector = new List<float>();

        for (int i = 0; i < this.Points.Count; i++)
        {
            helpVector.Add(this.Points[i].x);
            helpVector.Add(this.Points[i].y);
            sum += Mathf.Pow(this.Points[i].x, 2) + Mathf.Pow(this.Points[i].y, 2);
        }

        float helpMagnitude = Mathf.Sqrt(sum);

        for (int i = 0; i < helpVector.Count; i++)
        {
            helpVector[i] /= helpMagnitude;
        }

        return helpVector;
    }

    // Move the gesture so that it can fit into predefined bounding box. 
    public List<Vector2> HelpTranslate(Vector2 point)
    {

        Vector2 center = AnalizePoints.GetCenterPoints(this.Points);
        List<Vector2> helpTranslated = new List<Vector2>();

        for (int i = 0; i < this.Points.Count; i++)
        {
            float x = this.Points[i].x + point.x - center.x;
            float y = this.Points[i].y + point.y - center.y;
            helpTranslated.Add(new Vector2(x, y));
        }

        return helpTranslated;
    }

    //Scale the gesture so that it can fit into predefined bounding box.
    public List<Vector2> HelpScale(float size)
    {

        Rect box = AnalizePoints.GetBox(this.Points);
        List<Vector2> helpScaled = new List<Vector2>();

        for (int i = 0; i < this.Points.Count; i++)
        {
            float x = this.Points[i].x * (size / box.width);
            float y = this.Points[i].y * (size / box.height);
            helpScaled.Add(new Vector2(x, y));
        }

        return helpScaled;
    }

    // Calculate the bounding box.
    public static Rect GetBox(List<Vector2> points)
    {

        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;

        for (int i = 0; i < points.Count; i++)
        {
            minX = Mathf.Min(minX, points[i].x);
            minY = Mathf.Min(minY, points[i].y);
            maxX = Mathf.Max(maxX, points[i].x);
            maxY = Mathf.Max(maxY, points[i].y);
        }

        return new Rect(minX, minY, maxX - minX, maxY - minY);
    }

    // Indicative angle is at zero degrees.
    public List<Vector2> HelpRotate(float angle)
    {
        Vector2 center = AnalizePoints.GetCenterPoints(this.Points);
        float cos = Mathf.Cos(angle);
        float sin = Mathf.Sin(angle);
        List<Vector2> helpRotatedPoints = new List<Vector2>();

        for (int i = 0; i < this.Points.Count; i++)
        {
            float x = (this.Points[i].x - center.x) * cos - (this.Points[i].y - center.y) * sin + center.x;
            float y = (this.Points[i].x - center.x) * sin + (this.Points[i].y - center.y) * cos + center.y;
            helpRotatedPoints.Add(new Vector2(x, y));
        }

        return helpRotatedPoints;
    }

    // Resample the point list so that the list has numberOfPoints number of points and points are equidistant to each other.
    public List<Vector2> Resample(int number)
    {
        float incr = AnalizePoints.GetLength(this.Points) / (number - 1);
        float distanceMoved = 0.0f;

        List<Vector2> resampled = new List<Vector2>();
        resampled.Add(this.Points[0]);

        for (int i = 1; i < this.Points.Count; i++)
        {
            float distance = Vector2.Distance(this.Points[i - 1], this.Points[i]);

            if (distanceMoved + distance >= incr)
            {
                float x = this.Points[i - 1].x + ((incr - distanceMoved) / distance) * (this.Points[i].x - this.Points[i - 1].x);
                float y = this.Points[i - 1].y + ((incr - distanceMoved) / distance) * (this.Points[i].y - this.Points[i - 1].y);
                Vector2 q = new Vector2(x, y);
                resampled.Add(q);
                this.Points.Insert(i, q);
                distanceMoved = 0.0f;
            }
            else
            {
                distanceMoved += distance;
            }
        }

        if (resampled.Count == number - 1)
        {
            resampled.Add(this.Points[this.Points.Count - 1]);
        }

        return resampled;
    }

    // Sum of distance between each points
    public static float GetLength(List<Vector2> points)
    {
        float length = 0;

        for (int i = 1; i < points.Count; i++)
        {
            length += Vector2.Distance(points[i - 1], points[i]);
        }

        return length;
    }

    // The angle between the center and the first point on standard cartesian coordinate system.
    public static float GetCenterAngle(List<Vector2> points)
    {
        Vector2 centerAngel = AnalizePoints.GetCenterPoints(points);
        return Mathf.Atan2(centerAngel.y - points[0].y, centerAngel.x - points[0].x);
    }

    // Calculate the center of the points
    public static Vector2 GetCenterPoints(List<Vector2> points)
    {
        Vector2 centerPoints = Vector2.zero;

        for (int i = 0; i < points.Count; i++)
        {
            centerPoints += points[i];
        }

        return centerPoints / points.Count;
    }
}
