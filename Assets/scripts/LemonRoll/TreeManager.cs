using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    void Start()
    {
        GenerateurDeForme genForme = GetComponent<GenerateurDeForme>();
        genForme.tailleTronc=2f;
        genForme.epaisseurTronc = 1f;
        genForme.epaisseurArbre = 12f;
        genForme.hauteurArbre = 10f;
        genForme.nombreDePoint = 350;
        Vector3 position = this.transform.position;
        genForme.GenererArbre(position.x,position.y,position.z);
        genForme.ClearVariables();
    }
}