using HoloToolkit.Unity;
using HoloToolkit.Unity.InputModule;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : Singleton<CursorManager>
{
    [Serializable]
    public struct CursorData
    {
        public GameObject cursor;
        public string id;
    }

    public CursorData[] cursors;
    public GameObject ActiveCursor { get; private set; }
    
    private void Start()
    {
        foreach (CursorData data in cursors)
        {
            if (data.cursor.activeSelf)
            {
                ActiveCursor = data.cursor;
                break;
            }
        }
    }
    public GameObject[] AllCursors
    {
        get
        {
            GameObject[] cs = new GameObject[cursors.Length];
            for(int i=0;i<cursors.Length;i++)
            {
                cs[i] = cursors[i].cursor;
            }
            return cs;
        }
    }
    public void ActivateCursor(string id)
    {
        //Current cursor position, to be used for the new cursor to be positioned in.
        Vector3 curPos = ActiveCursor != null ? ActiveCursor.transform.position : Vector3.zero;
        //the cursor to be activated
        ActiveCursor = null;
        GameObject cursor=null;
        //first deactivate all cursros
        foreach(CursorData data in cursors)
        {
            if (data.id.Equals(id))
            {
                cursor = data.cursor;
            }
            data.cursor.SetActive(false);
        }
        //activate the one
        if (cursor!=null)
        {
            //re-poisition the new cursor in same loca of prev one
            //and activate it.
            cursor.transform.position = curPos;
            cursor.SetActive(true);
            cursor.transform.position = curPos;
            ActiveCursor = cursor;
        }
    }
}
