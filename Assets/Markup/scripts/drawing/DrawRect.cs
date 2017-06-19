using UnityEngine;

using System.Collections.Generic;

using UnityEngine.VR.WSA.Input;


public class DrawRect : DrawLinesBase
{
    public bool Fill = false;
    public Vector3 startPoint = Vector3.zero, endPoint = Vector3.zero, normal=Vector3.zero;

    private Vector3 manipulationDelta, myPos;
    private GestureRecognizer manipulationGestureRecognizer;

    private LineRenderer rect;
    private int rectNumber = 0;

    //
    TextMesh debug;
    //	-----------------------------------	
    void Awake()
    {
        manipulationGestureRecognizer = new GestureRecognizer();
        manipulationGestureRecognizer.SetRecognizableGestures(GestureSettings.ManipulationTranslate);
        manipulationGestureRecognizer.ManipulationCanceledEvent += ManipulationGestureRecognizer_ManipulationCanceledEvent;
        manipulationGestureRecognizer.ManipulationCompletedEvent += ManipulationGestureRecognizer_ManipulationCompletedEvent;
        manipulationGestureRecognizer.ManipulationStartedEvent += ManipulationGestureRecognizer_ManipulationStartedEvent;
        manipulationGestureRecognizer.ManipulationUpdatedEvent += ManipulationGestureRecognizer_ManipulationUpdatedEvent;

        debug = GameObject.Find("Debug").GetComponent<TextMesh>();
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
        OnDrawing(cumulativeDelta, headRay.origin);
    }

    private void ManipulationGestureRecognizer_ManipulationStartedEvent(InteractionSourceKind source, Vector3 cumulativeDelta, Ray headRay)
    {
        StartDrawing(cumulativeDelta, headRay.origin);
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
        if (rect == null)
        {
            rect = createNewLine("Rect#" + (++rectNumber));
            rect.loop = true;
            rect.positionCount = 4;
        }

        manipulationDelta = cumulativeDelta;
        myPos = CursorManager.Instance.ActiveCursor.transform.position;
        normal = -(Camera.main.transform.position + Camera.main.transform.forward).normalized;
        //startPoint.z = CursorManager.Instance.ActiveCursor.transform.forward.z;
        startDraw = false;
    }
    private bool startDraw;
    void EndDrawing()
    {
        CursorManager.Instance.ActivateCursor("default");
        rect = null;
    }
    void CancelDrawing()
    {
        if(rect)
        {
            GameObject.Destroy(rect.gameObject);
        }
        EndDrawing();
    }

    void OnDrawing(Vector3 cumulativeDelta, Vector3 headPosition)
    {
        if (rect!=null)
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
            }
     
            myPos += move;
            if (!startDraw )
            {
                startPoint = MathUtilsExt.ProjectPointOnPlane(normal, myPos, myPos);
                endPoint = startPoint;
                ////???? AHMAD   FIXME    
                startDraw = true;
            }
            CursorManager.Instance.ActiveCursor.transform.position = myPos;
            //endPoint = myPos;
            endPoint = MathUtilsExt.ProjectPointOnPlane(normal, startPoint, myPos);
            //startPoint.z= endPoint.z;
            endPoint.z = startPoint.z;
            debug.text = "Start: " + startPoint.ToString();
            debug.text += "\nEnd : " + endPoint.ToString();
            debug.text += "\nFWD : " + Camera.main.transform.forward;

            rect.SetPosition(0, startPoint);
            rect.SetPosition(1, new Vector3(endPoint.x, startPoint.y, startPoint.z));
            rect.SetPosition(2, endPoint);
            rect.SetPosition(3, new Vector3(startPoint.x, endPoint.y, startPoint.z));
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
