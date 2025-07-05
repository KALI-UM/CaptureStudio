using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameObjectToTexture))]
public class GameObjectToTextureEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var gobj2Tex = (GameObjectToTexture)target;
        if (GUILayout.Button("RenderTexture To " + gobj2Tex.textureFileFormat.ToString()))
        {
            gobj2Tex.StartCapture();
        }

        base.OnInspectorGUI();
    }
}
