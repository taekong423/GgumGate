﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Item))]
public class ItemEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
