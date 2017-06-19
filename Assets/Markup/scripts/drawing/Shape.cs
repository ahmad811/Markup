using UnityEngine;

public class Shape : MonoBehaviour {
    public GameObject Holder;
    public float LineWidth = 0.01f;
    public Color LineColor = Color.green;

    public Material LineMaterial;

    private void Awake()
    {
        if (Holder == null)
            Holder = gameObject;
        if (LineMaterial == null)
            LineMaterial = new Material(Shader.Find("Particles/Additive"));
    }
    protected LineRenderer createNewLine(string name)
    {
        GameObject currentGO = new GameObject(name);
        currentGO.transform.parent = Holder.transform;
        LineRenderer line = currentGO.AddComponent<LineRenderer>();
        line.material = LineMaterial;
        line.startWidth = line.endWidth = LineWidth;
        line.startColor = line.endColor = LineColor;
        line.useWorldSpace = true;

        return line;
    }
}
