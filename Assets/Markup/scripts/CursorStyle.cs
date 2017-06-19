using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorStyle : MonoBehaviour {

    public Color PenColor;

    public void SetColor(Color c)
    {
        PenColor = c;
        GameObject go = transform.FindChild("pen/Cone_Node").gameObject;
        if (go != null)
        {
            go.GetComponent<MeshRenderer>().material.color = c;
        }
    }
}
