using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class LineContainer : MonoBehaviour
{
    public Vector3[] ShapeData;
    public string Shapename;
    public bool DEBUG;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/Shapes");
        ShapeData = JsonConvert.DeserializeObject<Vector3[]>(File.ReadAllText(Application.dataPath + $"/Shapes/{Shapename}.txt"));
        
        // var ShownLine = new GameObject("Line").AddComponent<LineRenderer>();
        // ShownLine.positionCount = 60;
        // ShownLine.startWidth = 0.1f;
        // ShownLine.endWidth = 0.1f;
        //
        // ShownLine.SetPositions(ShapeData);
        // ShownLine.material = new Material(Shader.Find("Unlit/Color"));
        // ShownLine.material.color = Color.white;
    }
    
    public void CheckData(Vector3[] Sample)
    {
        
        float[] Deviations = new float[60];
        float DeviAverage = 0;
        
        for (int i = 0; i < 60; i++)
        {
            Deviations[i] = Vector3.Distance(Sample[i], ShapeData[i]);
            DeviAverage += Deviations[i];
        }
        
        DeviAverage /= 60;
        
        Debug.Log($"{DeviAverage} is the Mean Deviation");
        // Debug.Log(String.Join(", ",Deviations));

        if (DEBUG)
        {
            var ShownLine = new GameObject("Line").AddComponent<LineRenderer>();
            ShownLine.positionCount = 60;
            ShownLine.startWidth = 0.1f;
            ShownLine.endWidth = 0.1f;

            var Chart = new Vector3[60];

            for (int i = 0; i < 60; i++)
            {
                Chart[i] = new Vector3((float)i/5, Mathf.Pow(Deviations[i],4), 0);
            }
            ShownLine.SetPositions(Chart);
            ShownLine.material = new Material(Shader.Find("Unlit/Color"));
            ShownLine.material.color = Color.white;
            
            var ShownLine2 = new GameObject("Line").AddComponent<LineRenderer>();
            ShownLine2.positionCount = 60;
            ShownLine2.startWidth = 0.1f;
            ShownLine2.endWidth = 0.1f;
            
            ShownLine2.SetPositions(ShapeData);
            ShownLine2.material = new Material(Shader.Find("Unlit/Color"));
            ShownLine2.material.color = Color.red;
        }
        

    }
}
