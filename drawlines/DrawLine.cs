using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    private LineRenderer currentLine;
    private List<Vector3> pointsList;

    private Vector3 mousePos;
    private Material material;
    private int currLines = 0;

    public float LineWidth = 0.1f;
    public Color LineColor = Color.green;
    public enum LineType
    {
        Curve,
        Straight
    }
    public LineType lineType;
    //	-----------------------------------	
    void Awake()
    {
        pointsList = new List<Vector3>();
        material = new Material(Shader.Find("Particles/Additive"));
    }
    
    private void Update()
    {
        if(lineType == LineType.Curve)
        {
            UpdateCurvedLine();
        }
        else if(lineType==LineType.Straight)
        {
            UpdateStraightLine();
        }
    }
    private void UpdateStraightLine()
    {
        //Create new Line on left mouse click(down)
        if (Input.GetMouseButtonDown(0))
        {
            //check if there is no line renderer created
            if (currentLine == null)
            {
                //create the line
                currentLine= createNewStraightLineLine();
            }
            //get the mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //set the z co ordinate to 0 as we are only interested in the xy axes
            mousePos.z = 0;
            //set the start point and end point of the line renderer
            currentLine.SetPosition(0, mousePos);
            currentLine.SetPosition(1, mousePos);
        }
        //if line renderer exists and left mouse button is click exited (up)
        else if (Input.GetMouseButtonUp(0) && currentLine)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            //set the end point of the line renderer to current mouse position
            currentLine.SetPosition(1, mousePos);
            //set line as null once the line is created
            currentLine = null;
            currLines++;
        }
        //if mouse button is held clicked and line exists
        else if (Input.GetMouseButton(0) && currentLine)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            //set the end position as current position but dont set line as null as the mouse click is not exited
            currentLine.SetPosition(1, mousePos);
        }
    }
    //	-----------------------------------	
    void UpdateCurvedLine()
    {
        // If mouse button down, remove old line and set its color to green
        if (Input.GetMouseButtonDown(0))
        {
            //check if there is no line renderer created
            if (currentLine == null)
            {
                //create the line
                currentLine = createNewStraightLineLine();
            }
            pointsList = new List<Vector3>();
        }
        else if (Input.GetMouseButtonUp(0) && currentLine)
        {
            //set line as null once the line is created
            currentLine = null;
            currLines++;
        }
        // Drawing line when mouse is moving(presses)
        if (Input.GetMouseButton(0) && currentLine)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (pointsList.Count==0 || !V3Equal(pointsList[pointsList.Count-1],mousePos))
            {
                pointsList.Add(mousePos);
                currentLine.positionCount = pointsList.Count;
                currentLine.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            }
        }
    }
    //Line Creation

    LineRenderer createNewStraightLineLine()
    {
        LineRenderer line = createNewLine("Line");
        line.positionCount = 2;
        return line;
    }
    LineRenderer createNewCurvedLine()
    {
        LineRenderer line = createNewLine("Curve");
        line.positionCount = 0;
        return line;
    }
    LineRenderer createNewLine(string prefix = "Line")
    {
        GameObject currentGO = new GameObject(prefix + currLines);
        currentGO.transform.parent = gameObject.transform;
        LineRenderer line = currentGO.AddComponent<LineRenderer>();
        line.material = material;
        line.startWidth = line.endWidth = LineWidth;
        line.startColor = line.endColor = LineColor;
        line.useWorldSpace = true;

        return line;
    }

    //
    public bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.001;
    }
}