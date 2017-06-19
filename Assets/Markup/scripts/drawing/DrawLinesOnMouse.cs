using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HoloToolkit.Unity;

public class DrawLinesOnMouse : DrawLinesBase
{
    private LineRenderer currentLine;
    private List<Vector3> pointsList;

    private Vector3 mousePos;
    
    public AudioSource Test;

    //	-----------------------------------	
    void Awake()
    {
        pointsList = new List<Vector3>();
        if(LineMaterial==null)
            LineMaterial = new Material(Shader.Find("Particles/Additive"));
    }
    
    private void Update()
    {
        if (lineType == LineType.Curve)
        {
            UpdateCurvedLine();
        }
        else if (lineType == LineType.Straight)
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
                currentLine = createNewStraightLineLine();
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
        }
        // Drawing line when mouse is moving(presses)
        if (Input.GetMouseButton(0) && currentLine)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            if (pointsList.Count == 0 || !MathUtilsExt.V3Equal(pointsList[pointsList.Count - 1], mousePos))
            {
                pointsList.Add(mousePos);
                currentLine.positionCount = pointsList.Count;
                currentLine.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
            }
        }
    }
   
}
