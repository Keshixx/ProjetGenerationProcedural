using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GenerateurDeForme))]
public class GenerateurDeFormEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GenerateurDeForme formGen = (GenerateurDeForme)target;
        
        if(DrawDefaultInspector())
        {
            if(formGen.autoUpdate)
            {
                formGen.GenererArbre();
            }
        }
        
        if(GUILayout.Button("Generate"))
        {
            formGen.GenererArbre();
        }
    }
}
