using UnityEngine;

using System.Collections.Generic;

using UnityEngine.VR.WSA.Input;


public class DrawLines : DrawLinesBase
{
    private LineRenderer currentLine;
    private List<Vector3> pointsList;

    private Vector3 manipulationDelta, myPos;
    private GestureRecognizer manipulationGestureRecognizer;

    //	-----------------------------------	
    void Awake()
    {
        pointsList = new List<Vector3>();

        manipulationGestureRecognizer = new GestureRecognizer();
        manipulationGestureRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
        manipulationGestureRecognizer.ManipulationCanceledEvent += ManipulationGestureRecognizer_ManipulationCanceledEvent;
        manipulationGestureRecognizer.ManipulationCompletedEvent += ManipulationGestureRecognizer_ManipulationCompletedEvent;
        manipulationGestureRecognizer.ManipulationStartedEvent += ManipulationGestureRecognizer_ManipulationStartedEvent;
        manipulationGestureRecognizer.ManipulationUpdatedEvent += ManipulationGestureRecognizer_ManipulationUpdatedEvent;
    }
    private void OnEnable()
    {
        ActiveGestureManager.Instance.ActiveGesture = manipulationGestureRecognizer;
    }
    private void OnDisable()
    {
        EndDrawing();
        ActiveGestureManager.Instance.ActiveGesture = null;
    }

    private void OnDestroy()
    {
        ActiveGestureManager.Instance.RemoveAll(manipulationGestureRecognizer);

        manipulationGestureRecognizer.ManipulationCanceledEvent -= ManipulationGestureRecognizer_ManipulationCanceledEvent;
        manipulationGestureRecognizer.ManipulationCompletedEvent -= ManipulationGestureRecognizer_ManipulationCompletedEvent;
        manipulationGestureRecognizer.ManipulationStartedEvent -= ManipulationGestureRecognizer_ManipulationStartedEvent;
        manipulationGestureRecognizer.ManipulationUpdatedEvent -= ManipulationGestureRecognizer_ManipulationUpdatedEvent;
        
        manipulationGestureRecognizer.Dispose();
    }

    private void ManipulationGestureRecognizer_ManipulationUpdatedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
       OnDrawing(cumulativeDelta,headRay.origin);
    }

    private void ManipulationGestureRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        StartDrawing(cumulativeDelta,headRay.origin);
    }

    private void ManipulationGestureRecognizer_ManipulationCompletedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        EndDrawing();
    }

    private void ManipulationGestureRecognizer_ManipulationCanceledEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        CancelDrawing();
    }
  

    void StartDrawing(Vector3 cumulativeDelta, Vector3 headPosition)
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

        manipulationDelta = cumulativeDelta;
        myPos = CursorManager.Instance.ActiveCursor.transform.position;
    }
    void EndDrawing()
    {
        CursorManager.Instance.ActivateCursor("default");
        currentLine = null;
        pointsList = new List<Vector3>();
    }
    void CancelDrawing()
    {
        if (currentLine)
        {
            //   GameObject.Destroy(currentLine.gameObject);
        }
        EndDrawing();
    }

    void OnDrawing(Vector3 cumulativeDelta, Vector3 headPosition)
    {
        if (currentLine)
        {
            Vector3 move = cumulativeDelta - manipulationDelta;
            Vector3 handPosition;
            if (GazeGestureUtils.GetManipulationHandPosition(out handPosition))
            {
                Vector3 fromHeadToModel = myPos - headPosition;
                Vector3 fromHeadToHand = handPosition - headPosition;

                float moveAmplifier = fromHeadToModel.magnitude / fromHeadToHand.magnitude;

                if (moveAmplifier > 1)
                {
                    move *= moveAmplifier;
                }
                myPos += move;
            }

            CursorManager.Instance.ActiveCursor.transform.position = myPos;

            Vector3 pos = myPos;
            if (pointsList.Count == 0 || !MathUtilsExt.V3Equal(pointsList[pointsList.Count - 1], pos))
            {
                pointsList.Add(pos);
                currentLine.positionCount = pointsList.Count;
                currentLine.SetPosition(pointsList.Count - 1, pointsList[pointsList.Count - 1]);
            }
        }
        else
        {
            StartDrawing(cumulativeDelta, headPosition);
        }
        manipulationDelta = cumulativeDelta;
    }
    private void Update()
    {
        /*if (isDrawActive)
        {
            OnDrawing();
        }*/
    }

}
