using System.Collections;
using UnityEditor;
using UnityEngine;


//[CustomEditor(typeof(Lazer1))]
//[CanEditMultipleObjects]
public class Lazer1EditorScript : Editor
{
    //public bool preview = false;
    public SerializedProperty gapDist;
    public SerializedProperty Wall1;
    public SerializedProperty Wall2;
    public SerializedProperty StartPos;
    public SerializedProperty EndPos;
    Lazer1 lazer;

    //bool init = false;

    //private void CallbackFunction()
    //{
    //    lazer.Animate();
    //}

    //void OnEnable()
    //{

    //    EditorApplication.update += CallbackFunction;
    //}

    public override void OnInspectorGUI()
    {
        //if (!init)
        //{
        //    init = true;
        //    OnEnable();
        //}

        lazer = (Lazer1)target;

        //lazer.animate = EditorGUILayout.Toggle("Preview Effect", lazer.animate);

        lazer.gapDist = EditorGUILayout.FloatField("Gap Distance", lazer.gapDist);

        lazer.heightOffGround = EditorGUILayout.FloatField("Dist above ground", lazer.heightOffGround);
        
        /*
        lazer.StartPos = EditorGUILayout.Vector3Field("StartPos", lazer.StartPos);
        lazer.EndPos = EditorGUILayout.Vector3Field("EndPos", lazer.EndPos);
        */

        lazer.animationTime = EditorGUILayout.FloatField("Speed", lazer.animationTime);

		if(GUI.changed)
			EditorUtility.SetDirty(lazer);
    }


}
