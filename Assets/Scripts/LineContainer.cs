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
    
    
    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory(Application.dataPath + "/Shapes");
        ShapeData = JsonConvert.DeserializeObject<Vector3[]>(File.ReadAllText(Application.dataPath + $"/Shapes/{Shapename}.txt"));
        
        var ShownLine = new GameObject("Line").AddComponent<LineRenderer>();
        ShownLine.positionCount = 60;
        ShownLine.startWidth = 0.1f;
        ShownLine.endWidth = 0.1f;
        
        ShownLine.SetPositions(ShapeData);
        ShownLine.material = new Material(Shader.Find("Unlit/Color"));
        ShownLine.material.color = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
