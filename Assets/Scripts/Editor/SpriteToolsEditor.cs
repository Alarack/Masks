using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteTools))]
public class SpriteToolsEditor : Editor
{
    private SpriteTools _spriteTools;

    public override void OnInspectorGUI()
    {
        _spriteTools = (SpriteTools)target;



        if(GUILayout.Button("Randomize Sprites"))
        {
            _spriteTools.RandomizeSprites();
        }



        base.OnInspectorGUI();



    }


}
