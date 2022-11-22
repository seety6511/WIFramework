using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WIFramework.UI;

[CanEditMultipleObjects]
[CustomEditor(typeof(UISimpleAnimator))]
public class UIAnimationEditor : Editor
{
    UISimpleAnimator anim;
    private void OnEnable()
    {
        anim = target as UISimpleAnimator;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("SetStartState"))
            anim.SaveOrigin();
        if (GUILayout.Button("SetTargetState"))
            anim.SaveTarget();
        
        if (GUILayout.Button("Reset"))
            anim.SetOrigin();
        if (GUILayout.Button("PreviewTargetState"))
            anim.SetTarget();
    }
}
