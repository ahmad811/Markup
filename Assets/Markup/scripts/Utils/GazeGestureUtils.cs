
using UnityEngine;
using UnityEngine.VR.WSA.Input;

class GazeGestureUtils
{
    public static bool GetManipulationHandPosition(out Vector3 handPosition)
    {
        InteractionSourceState[] sources = InteractionManager.GetCurrentReading();

        foreach (InteractionSourceState state in sources)
        {
            if (state.source.kind == InteractionSourceKind.Hand)
            {
                if (state.pressed)
                {
                    Vector3 position;
                    if (state.properties.location.TryGetPosition(out position))
                    {
                        handPosition = position;
                        return true;
                    }
                }
            }
        }

        handPosition = Vector3.zero;
        return false;
    }
    private static Vector3 nick = new Vector3(0, -0.2f, 0);
    public static Vector3 GetHandPivotPosition()
    {
        Vector3 pivot = Camera.main.transform.position + nick - Camera.main.transform.forward * 0.2f; // a bit lower and behind
        return pivot;
    }
}