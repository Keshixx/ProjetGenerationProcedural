using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cellules
{
    public float coordX = 0;
    public float coordY = 0;
    public bool isVisited = false;
    public bool isWall = false;
    public Cellules voisinHaut;
    public Cellules voisinBas;
    public Cellules voisinGauche;
    public Cellules voisinDroite;
    public int index;
    public Cellules(float x, float y, Cellules haut, Cellules bas, Cellules gauche, Cellules droite, int i)
    {
        coordX = x;
        coordY = y;
        voisinBas = bas;
        voisinHaut = haut;
        voisinGauche = gauche;
        voisinDroite = droite;
        index = i;
    }
}