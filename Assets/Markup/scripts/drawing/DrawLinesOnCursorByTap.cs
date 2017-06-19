using UnityEngine;

using System.Collections.Generic;

using UnityEngine.VR.WSA.Input;


public class DrawLinesOnCursorByTap : DrawLinesBase
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
        tapGestureRecognizer.SetRecognizableGestures(GestureSettings.Tap);
        tapGestureRecognizer.TappedEvent += TapGestureRecognizer_TappedEvent;
    }
    private void OnEnable()
    {
        ActiveGestureManager.Instance.ActiveGesture=tapGestureRecognizer;
    }
    private void OnDisable()
    {
        ActiveGestureManager.Instance.ActiveGesture = null;
        EndDrawing();
    }
    private void OnDestroy()
    {
        ActiveGestureManager.Instance.RemoveAll(tapGestureRecognizer);
        tapGestureRecognizer.TappedEvent -= TapGestureRecognizer_TappedEvent;

        tapGestureRecognizer.Dispose();
    }
    private void TapGestureRecognizer_TappedEvent(InteractionSourceKind source, int tapCount, Ray headRay)
    {
        isDrawActive = !isDrawActive;
        if (isDrawActive)
        {
            StartDrawing();
        }
        else
        {
            EndDrawing();
        }
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
    }
    void EndDrawing()
    {
        //Test.Play();
        currentLine = null;
        pointsList = new List<Vector3>();
        CursorManager.Instance.ActivateCursor("default");
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
        else {
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
