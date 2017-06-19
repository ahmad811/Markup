using HoloToolkit.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.WSA.Input;

public class ActiveGestureManager : Singleton<ActiveGestureManager> {
    
    private SemiStack<GestureRecognizer> gestures;
    protected override void Awake()
    {
        base.Awake();
        gestures = new SemiStack<GestureRecognizer>();
    }

    public GestureRecognizer ActiveGesture
    {
        get{ return gestures.Peek(); }
        set
        {
            //unregister the active one
            GestureRecognizer active = gestures.Peek();
            StopGesture(active);
            
            //if not null, push it and activate it
            if (value != null)
            {
                gestures.Push(value);
                StartGesture(value);
            }
            //deactivate me
            else
            {
                //remove me - note, i already stopped in the first 'if'
                gestures.Pop();
                //activate prev one
                GestureRecognizer active1 = gestures.Peek();
                StopGesture(active1);

            }
        }
    }
    public void RemoveAll(GestureRecognizer gesture)
    {
        List< GestureRecognizer> items= gestures.Remove(gesture);
        foreach(GestureRecognizer g in items)
        {
            StopGesture(g);
        }
    }
    private void StartGesture(GestureRecognizer g)
    {
        if (g != null)
        {
            g.CancelGestures();
            g.StartCapturingGestures();
        }
    }
    private void StopGesture(GestureRecognizer g)
    {
        if (g != null)
        {
            g.CancelGestures();
            g.StopCapturingGestures();
        }
    }
    protected override void OnDestroy()
    {
        while(gestures.Count>0)
        {
            gestures.Pop();
        }
    }
}
