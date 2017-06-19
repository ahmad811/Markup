using UnityEngine;
using UnityEngine.VR.WSA.Input;

public abstract class DrawLinesBase : Shape{
    private int currLines = 0;
    
    public enum LineType
    {
        Curve,
        Straight
    }
    public LineType lineType = LineType.Curve;
   
    //Line Creation

    protected LineRenderer createNewStraightLineLine()
    {
        LineRenderer line = createNewLine("Line#"+(++currLines));
        line.positionCount = 2;
        return line;
    }
    protected LineRenderer createNewCurvedLine()
    {
        LineRenderer line = createNewLine("Curve#" + (++currLines));
        line.positionCount = 0;
        return line;
    }
}