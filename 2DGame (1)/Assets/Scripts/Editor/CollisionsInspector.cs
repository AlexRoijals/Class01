using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Collisions))]
public class CollisionsInspector : Editor
{

    static bool stateFoldout;
    static bool drawDefaultInspector;

    public override void OnInspectorGUI()
    {
        Collisions collisions = (Collisions)target;

        GUIStyle booleanText = new GUIStyle();

        GUIStyle normalText = new GUIStyle();

        EditorGUILayout.Space();
        EditorGUI.indentLevel = 1;

        stateFoldout = EditorGUILayout.Foldout(stateFoldout, "State", true, EditorStyles.toolbarDropDown);

        if(stateFoldout)
        {
            EditorGUILayout.BeginVertical(EditorStyles.textArea);

            EditorGUI.indentLevel = 2;
            // collisions.isGrounded = EditorGUILayout.Toggle("Is Grounded", collisions.isGrounded, EditorStyles.booleanText);

            //---------------------------------------------------------------------------

            //Ground

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            normalText.normal.textColor = Color.green;
            EditorGUILayout.LabelField("GROUND", normalText);

            EditorGUILayout.Space();

            if(collisions.isGrounded) booleanText.normal.textColor = Color.green;
            if(!collisions.isGrounded) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Is Grounded", booleanText);

            if (collisions.wasGrounded) booleanText.normal.textColor = Color.green;
            if (!collisions.wasGrounded) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Was Grounded", booleanText);

            if(collisions.justGOTGrounded) booleanText.normal.textColor = Color.green;
            if(!collisions.justGOTGrounded) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Just GOT Grounded", booleanText);

            if(collisions.justNOTGrounded) booleanText.normal.textColor = Color.green;
            if(!collisions.justNOTGrounded) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Just NOT Grounded", booleanText);

            if(collisions.isFalling) booleanText.normal.textColor = Color.green;
            if(!collisions.isFalling) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Is falling", booleanText);

            //---------------------------------------------------------------------------

            //Ceiling

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            normalText.normal.textColor = Color.blue;
            EditorGUILayout.LabelField("CEILING", normalText);

            EditorGUILayout.Space();

            if(collisions.isTouchingCeiling) booleanText.normal.textColor = Color.blue;
            if(!collisions.isTouchingCeiling) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Is touching ceiling", booleanText);

            if(collisions.wasTouchingCeiling) booleanText.normal.textColor = Color.blue;
            if(!collisions.wasTouchingCeiling) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("WAS touching ceiling", booleanText);

            if(collisions.justTouchedCeiling) booleanText.normal.textColor = Color.blue;
            if(!collisions.justTouchedCeiling) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("JUST touched ceiling", booleanText);

            if(collisions.justDidntTouchCeiling) booleanText.normal.textColor = Color.blue;
            if(!collisions.justDidntTouchCeiling) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Just didn't touch ceiling", booleanText);

            //---------------------------------------------------------------------------

            //Walls

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            normalText.normal.textColor = Color.white;
            EditorGUILayout.LabelField("WALLS", normalText);

            EditorGUILayout.Space();

            if(collisions.isTouchingWall) booleanText.normal.textColor = Color.white;
            if(!collisions.isTouchingWall) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("Is touching wall", booleanText);

            if(collisions.wasTouchingWall) booleanText.normal.textColor = Color.white;
            if(!collisions.wasTouchingWall) booleanText.normal.textColor = Color.red;
            EditorGUILayout.LabelField("WAS touching wall", booleanText);

            //---------------------------------------------------------------------------

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        EditorGUI.indentLevel = 1;

        drawDefaultInspector = EditorGUILayout.Foldout(drawDefaultInspector, "Default Inspector", true, EditorStyles.toolbarDropDown);

        if (drawDefaultInspector)
        {
            EditorGUI.indentLevel = 2;
            base.OnInspectorGUI();
        }
    }

}
