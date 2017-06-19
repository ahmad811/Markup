using UnityEngine;

using System.Collections.Generic;

using UnityEngine.VR.WSA.Input;


public class DrawLinesOnCursor : DrawLinesBase
{
    private LineRenderer currentLine;
    private List<Vector3> pointsList;

    private bool isDrawActive = false;
    private GestureRecognizer tapGestureRecognizer;

    //	-----------------------------------	
    void Awake()
    {
        pointsList = new List<Vector3>();

        tapGestureRecognizer = new GestureRecognizer();
        tapGestureRecognizer.SetRecognizableGestures(GestureSettings.Hold);
        tapGestureRecognizer.HoldStartedEvent += TapGestureRecognizer_HoldStartedEvent;
        tapGestureRecognizer.HoldCompletedEvent += TapGestureRecognizer_HoldCompletedEvent;
        tapGestureRecognizer.HoldCanceledEvent += TapGestureRecognizer_HoldCanceledEvent;
    }
    private void OnEnable()
    {
        ActiveGestureManager.Instance.ActiveGesture = tapGestureRecognizer;
    }
    private void OnDisable()
    {
        EndDrawing();
        ActiveGestureManager.Instance.ActiveGesture = null;
    }

    private void OnDestroy()
    {
        ActiveGestureManager.Instance.RemoveAll(tapGestureRecognizer);

        tapGestureRecognizer.HoldStartedEvent -= TapGestureRecognizer_HoldStartedEvent;
        tapGestureRecognizer.HoldCompletedEvent -= TapGestureRecognizer_HoldCompletedEvent;
        tapGestureRecognizer.HoldCanceledEvent -= TapGestureRecognizer_HoldCanceledEvent;
        
        tapGestureRecognizer.Dispose();
    }
    private void TapGestureRecognizer_HoldCanceledEvent(InteractionSourceKind source, Ray headRay)
    {
        CancelDrawing();
    }

    private void TapGestureRecognizer_HoldCompletedEvent(InteractionSourceKind source, Ray headRay)
    {
        EndDrawing();
    }

    private void TapGestureRecognizer_HoldStartedEvent(InteractionSourceKind source, Ray headRay)
    {
        StartDrawing();
    }

    void StartDrawing()
    {
        CursorManager.Instance.ActivateCursor("pen");
        //check if there is no line renderer created
        if (currentLine == null)
        {
            //create the line
            if (lineType == LineType.Curve)
            {

                currentLine = createNewCurvedLine();
            }
            else if (lineType == LineType.Straight)
            {
                currentLine = createNewStraightLineLine();
            }
        }
        pointsList = new List<Vector3>();
        isDrawActive = true;
    }
    void EndDrawing()
    {
        isDrawActive = false;
        CursorManager.Instance.ActivateCursor("default");
        currentLine = null;
        pointsList = new List<Vector3>();
    }
    void CancelDrawing()
    {
        if (currentLine)
        {
            GameObject.Destroy(currentLine.gameObject);
        }
        EndDrawing();
    }
    void OnDrawing()
    {
        if (currentLine)
        {
            Vector3 pos = CursorManager.Instance.ActiveCursor.transform.position;
            //pos.z = 0;
            if (pointsList.Count == 0 || !MathUtilsExt.V3Equal(pointsList[pointsList.Count - 1], pos))
            {
                pointsList.Add(pos);
                currentLine.positionCount = pointsList.Count;
                currentLine.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
            }
        }
        else
        {
            StartDrawing();
        }

    }
    private void Update()
    {
        if (isDrawActive)
        {
            OnDrawing();
        }
    }
}
