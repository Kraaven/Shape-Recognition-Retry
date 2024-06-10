using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Splines;

public class LineInput : MonoBehaviour
{
   
    // Start is called before the first frame update
    private Spline Line;
    public float Threshold;
    public int iterations = 1; // Number of smoothing iterations
    public float smoothingFactor = 0.5f;
    
    public bool Pressed;
    void Start()
    {
        Line = GetComponent<SplineContainer>().Splines[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pressed = true;
            Line.Add(new BezierKnot(GetInputPosition()),TangentMode.AutoSmooth);
            
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {
                Pressed = false;
                if (Line.Count > 6)
                {
                    SmoothSpline(Line, iterations, smoothingFactor);

                    var ShownLine = new GameObject("Line").AddComponent<LineRenderer>();
                    ShownLine.positionCount = 60;
                    ShownLine.startWidth = 0.1f;
                    ShownLine.endWidth = 0.1f;

                    var Points = ScaleToSetSize(ConvertSplineToArray(Line, 60),0.45f);
                    ShownLine.SetPositions(TranslateToOrigin(RotateToUp(Points)));
                    ShownLine.material = new Material(Shader.Find("Unlit/Color"));
                    ShownLine.material.color = Color.black;
                }

                Line.Clear();
                Line = new Spline();
            }
        }

        var pos = GetInputPosition();
        if (Pressed && Vector3.Distance(pos, Line.Last().Position) > Threshold)
        {
            Line.Add(new BezierKnot(pos), TangentMode.AutoSmooth);
        }
    }

    Vector3 GetInputPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void SmoothSpline(Spline spline, int iterations, float factor)
    {
        for (int iter = 0; iter < iterations; iter++)
        {
            for (int i = 1; i < spline.Count - 1; i++)
            {
                Vector3 prev = spline[i - 1].Position;
                Vector3 next = spline[i + 1].Position;
                Vector3 current = spline[i].Position;

                Vector3 newPosition = current + factor * ((prev + next) / 2 - current);
                spline[i] = new BezierKnot(newPosition, spline[i].TangentIn, spline[i].TangentOut, spline[i].Rotation);
            }
        }
    }
    
    Vector3[] ConvertSplineToArray(Spline spline, int sampleCount)
    {
        Vector3[] points = new Vector3[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float t = i / (float)(sampleCount - 1); // Normalized parameter [0, 1]
            points[i] = spline.EvaluatePosition(t);
        }
        return points;
    }
    
    public Vector3[] ScaleToSetSize(Vector3[] points, float setSize)
    {
        if (points.Length < 2)
        {
            Debug.LogError("Not enough points to scale.");
            return points;
        }

        Vector3 direction = points[1] - points[0];
        float currentDistance = direction.magnitude;
        float scaleFactor = setSize / currentDistance;

        Vector3[] scaledPoints = new Vector3[points.Length];
        scaledPoints[0] = points[0];
        for (int i = 1; i < points.Length; i++)
        {
            scaledPoints[i] = points[0] + (points[i] - points[0]) * scaleFactor;
        }

        return scaledPoints;
    }
    
    public Vector3[] RotateToUp(Vector3[] points)
    {
        if (points.Length < 2)
        {
            Debug.LogError("Not enough points to rotate.");
            return points;
        }

        Vector3 direction = points[1] - points[0];
        Quaternion rotation = Quaternion.FromToRotation(direction, Vector3.up);

        Vector3[] rotatedPoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            rotatedPoints[i] = rotation * (points[i] - points[0]) + points[0];
        }

        return rotatedPoints;
    }
    
    public Vector3[] TranslateToOrigin(Vector3[] points)
    {
        if (points.Length == 0)
        {
            Debug.LogError("No points to translate.");
            return points;
        }

        Vector3[] translatedPoints = new Vector3[points.Length];
        Vector3 offset = points[0]; // Offset to move the first point to (0,0,0)
        
        for (int i = 0; i < points.Length; i++)
        {
            translatedPoints[i] = points[i] - offset;
        }

        return translatedPoints;
    }

    
}
