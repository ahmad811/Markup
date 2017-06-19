using UnityEngine;

public class MarkupManager : MonoBehaviour {
    public GameObject ShapeHolder;
    public GameObject ColorPallete;
    private Shape drawer;

    private void Start()
    {
        //this done uin unity 
        //ColorPallete.GetComponent<GazeableColorPicker>().OnColorChanged.AddListener(OnColorChange);
        drawer=gameObject.AddComponent<DrawLines>();
        drawer.Holder = ShapeHolder;
        drawer.enabled = false;

        drawer =gameObject.AddComponent<DrawLinesOnCursorByTap>();
        drawer.Holder = ShapeHolder;
        drawer.enabled = false;

        drawer = gameObject.AddComponent<DrawRect>();
        drawer.Holder = ShapeHolder;
        drawer.enabled = false;

        UpdateDrawer();
    }
    private void UpdateDrawer()
    {
        drawer.enabled = false;
        if (MarkupSettings.Instance.ActiveShape == MarkupSettings.Shapes.Line)
        {
            if (MarkupSettings.Instance.Mode == MarkupSettings.DrawMode.Hand)
            {
                drawer = gameObject.GetComponent<DrawLines>();
            }
            else if (MarkupSettings.Instance.Mode == MarkupSettings.DrawMode.CursorOnTap)
            {
                drawer = gameObject.GetComponent<DrawLinesOnCursorByTap>();
            }
        }
        else if(MarkupSettings.Instance.ActiveShape==MarkupSettings.Shapes.Rect)
        {
            drawer = gameObject.GetComponent<DrawRect>();
        }
        drawer.LineColor = MarkupSettings.Instance.PenColor;
        drawer.LineWidth = MarkupSettings.Instance.PenWidth;
        drawer.LineMaterial = MarkupSettings.Instance.LineMaterial;

        drawer.enabled = true;
    }
    public void OnColorChange(Color c)
    {
        ColorPallete.SetActive(false);
        MarkupSettings.Instance.PenColor = c;
        UpdateDrawer();

        ShapeHolder.SetActive(true);
        
        GameObject[] cursors = CursorManager.Instance.AllCursors;
        foreach (GameObject cursor in cursors)
        {
            CursorStyle ps = cursor.GetComponent<CursorStyle>();
            if (ps)
            {
                ps.SetColor(c);
            }
        }
    }
    void ShowColors()
    {
        drawer.enabled = false;
        ColorPallete.SetActive(true);
        ColorPallete.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 3f;
        ColorPallete.transform.rotation = new Quaternion(0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
    }
    void HideColors()
    {
        ColorPallete.SetActive(false);
        UpdateDrawer();
    }
    void ShowMarkup()
    {
        ShapeHolder.SetActive(true);
        UpdateDrawer();
    }
    void HideMarkup()
    {
       drawer.enabled = false;
        ShapeHolder.SetActive(false);
    }
    /*
    void TapMode()
    {
       MarkupSettings.Instance.Mode = MarkupSettings.DrawMode.CursorOnTap;
        UpdateDrawer();
        ShapeHolder.SetActive(true);
    }
    void HandMode()
    {
        MarkupSettings.Instance.Mode = MarkupSettings.DrawMode.Hand;
        UpdateDrawer();
        ShapeHolder.SetActive(true);
    }
    */
    void LineMode()
    {
        MarkupSettings.Instance.ActiveShape = MarkupSettings.Shapes.Line;
        UpdateDrawer();
    }
    void RectMode()
    {
        MarkupSettings.Instance.ActiveShape = MarkupSettings.Shapes.Rect;
        UpdateDrawer();
    }
    void ThinPen()
    {
        MarkupSettings.Instance.PenWidth *= .75f;
        UpdateDrawer();
    }
    void ThickPen()
    {
        MarkupSettings.Instance.PenWidth *= 1.25f;
        UpdateDrawer();
    }
}
