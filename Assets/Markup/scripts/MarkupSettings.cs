using HoloToolkit.Unity;
using UnityEngine;

public class MarkupSettings : Singleton<MarkupSettings> {
    public Color PenColor = Color.white;
    public float PenWidth = 0.01f;
    public enum DrawMode { Hand, CursorOnTap};
    public DrawMode Mode = DrawMode.Hand;

    public enum Shapes { Line, Rect};
    public Shapes ActiveShape = Shapes.Line;

    public Material LineMaterial;
}
