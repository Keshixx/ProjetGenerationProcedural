using System;
using System.Collections;
using System.Collections.Generic;
//using for sprite 2D
using UnityEngine;
public class Branches : MonoBehaviour
{
    [SerializeField]
    private GameObject p1;
    [SerializeField]
    private GameObject p2;
    [SerializeField]
    private GameObject p3;
    [SerializeField]
    private GameObject p4;
    [SerializeField]
    private float forceGlobal = 1f;
    //sprite de la feuille
    [SerializeField]
    private GameObject feuillePrefab;
    void Start()
    {
        // Vector3 direction = new Vector3(0,1,0);
        // List<Vector3> points = new List<Vector3>();
        // points.Add(p2.transform.position);
        // points.Add(p3.transform.position);
        // points.Add(p4.transform.position);
        // CreateBranchFromPoint(points, p1.transform.position, direction, p1.transform.position, forceGlobal);
        //CreateRandomPoint(500);
    }

    void CreateRandomPoint(int nbPoint)
    {
        System.Random random = new System.Random();
        float x;
        float y;
        float z;
        List<Vector3> maListe = new List<Vector3>();
        List<GameObject> mesPoints = new List<GameObject>();
        GameObject point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        point.transform.localScale  = new Vector3(0.1f,0.1f,0.1f);
        point.transform.position = new Vector3(0,0,0);
        Vector3 p1 = point.transform.position;
        mesPoints.Add(point);
        for(int i = 0; i<nbPoint; i++)
        {
            x = ((float)random.NextDouble() * 2f - 1f) * 2;
            y = ((float)random.NextDouble()) * 5;
            z = ((float)random.NextDouble() * 2f - 1f) * 2;
            point = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            point.transform.localScale  = new Vector3(0.1f,0.1f,0.1f);
            point.transform.position = new Vector3(x,y,z);
            maListe.Add(point.transform.position);
            mesPoints.Add(point);
        }
        //CreateBranchFromPoint(maListe,p1,new Vector3(0,10,0), p1);
        List<Vector3> maListe2 = new List<Vector3>(maListe);
        foreach(Vector3 elem in maListe)
        {
            if(elem.y < p1.y)
            {
                maListe2.Remove(elem);
            }
        }
        maListe = maListe2;
        foreach(GameObject elem in mesPoints)
        {
            Destroy(elem);
        }
    }

    Vector3 vectFrom2Points(Vector3 point1, Vector3 point2)
    {
        return new Vector3(point2.x - point1.x, point2.y - point1.y, point2.z - point1.z);
    }

    //Orthogonal vertical triangle rouge.
    Vector3 orthogonal1(Vector3 vect)
    {
        return new Vector3(0,-vect.z, vect.y);
    }
    //Orthogonal vertical triangle bleu.
    Vector3 orthogonal2(Vector3 vect)
    {
        return new Vector3(-vect.y, vect.x, 0);
    }
    //Orthogonal horizontal triangle rouge.
    Vector3 orthogonal3(Vector3 vect)
    {
        return new Vector3(0,-vect.z, vect.x);
    }
    //Orthogonal horizontal triangle bleu.
    Vector3 orthogonal4(Vector3 vect)
    {
        return new Vector3(-vect.y, vect.x, 0);
    }

    //Orthogonal profondeur triangle rouge.
    Vector3 orthogonal5(Vector3 vect)
    {
        return new Vector3(vect.z,0, 0);
    }
    //Orthogonal profondeur triangle bleu.
    Vector3 orthogonal6(Vector3 vect)
    {
        return new Vector3(-vect.y, vect.z, 0);
    }

    int cpt = 0;
    List<Vector3> champsDeVision(Vector3 direction,Vector3 p1,int choix)
    {
        Vector3 meridianne=(p1+direction);
        Vector3 orthogo;
        Vector3 othogo2;
        switch (choix){
            case 0://Y
                orthogo = orthogonal1(direction);
                othogo2 = orthogonal2(direction);
                break;
            case 1://X
                orthogo = orthogonal3(direction);
                othogo2 = orthogonal4(direction);
                break;
            case 2://Z
                orthogo = orthogonal5(direction);
                othogo2 = orthogonal6(direction);
                break;
            default:
                orthogo = orthogonal1(direction);
                othogo2 = orthogonal2(direction);
                break;
        }
        Vector3 p2 = meridianne + orthogo;
        Vector3 p3 = meridianne - orthogo;
        Vector3 p4 = meridianne + othogo2;
        Vector3 p5 = meridianne - othogo2;
        //dessiner un trait de p1 à p2 et de p1 à p3 et de p2 à p3
        //Debug.Log("p1 : "+p1);
        //Debug.Log("p2 : "+p2);
        //Debug.Log("p3 : "+p3);
        //Debug.Log("p4 : "+p4);
        //Debug.Log("p5 : "+p5);
        // Debug.DrawLine(p1, p2, Color.red, 1000f);
        // Debug.DrawLine(p1, p3, Color.red, 1000f);
        // Debug.DrawLine(p2, p3, Color.red, 1000f);
        // Debug.DrawLine(p1, p4, Color.blue, 1000f);
        // Debug.DrawLine(p1, p5, Color.blue, 1000f);
        // Debug.DrawLine(p4, p5, Color.blue, 1000f);
        

        return new List<Vector3> { p1, p2, p3, p4, p5 };
    }


    bool triangleDesEnfers(Vector3 a,Vector3 b, Vector3 p)
    //https://www.youtube.com/watch?v=kkucCUlyIUE&ab_channel=Quantale
    /*
    On va appliqué E a toute les couples de dimensions possible et pas seulement x,y car nous somme en 3D
    E(a,b) = (a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x)
    E(a,b) > 0 si b est à gauche de a
    E(a,b) < 0 si b est à droite de a
    E(a,b) = 0 si b est sur a
    */
    {

       return (a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x)>=0  ||
         (a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x) <= 0;
    }

    bool estEnVu(List<Vector3> points, Vector3 p)
    {
        return triangleDesEnfers(points[0], points[1], p) && triangleDesEnfers(points[2], points[1], p) && triangleDesEnfers(points[1], points[2], p);
    }

    bool DansPyramideY(List<Vector3> pyramide, Vector3 p)
    {
        float coeffDirecteur = (pyramide[3].y - pyramide[0].y) / (pyramide[3].x - pyramide[0].x);
        coeffDirecteur = Mathf.Abs(coeffDirecteur);
        //square of p.x^2 + p.z^2
        float xz = Mathf.Sqrt(Mathf.Pow(p.x, 2) + Mathf.Pow(p.z, 2));
        //Debug.Log(coeffDirecteur);
        //Debug.Log(xz);
        float ypoint = coeffDirecteur * xz;
        //Debug.Log(ypoint);
        //Debug.Log(pyramide[1].y);
        return p.y >= ypoint && ypoint <= pyramide[1].y && p.y <= pyramide[1].y;
    }

    bool DansPyramideX(List<Vector3> pyramide, Vector3 p)
    {
        float coeffDirecteur = (pyramide[3].x - pyramide[0].x) / (pyramide[3].y - pyramide[0].y);
        coeffDirecteur = Mathf.Abs(coeffDirecteur);
        //square of p.x^2 + p.z^2
        float yz = Mathf.Sqrt(Mathf.Pow(p.y, 2) + Mathf.Pow(p.z, 2));
        float xpoint = coeffDirecteur * yz;
        if(p.x < 0)
        return Mathf.Abs(p.x) >= Mathf.Abs(xpoint) && xpoint >= pyramide[1].x && p.x >= pyramide[1].x;
        //Debug.Log(coeffDirecteur);
        //Debug.Log(yz);
        
        //Debug.Log(xpoint);
        //Debug.Log(pyramide[1].x);
        return Mathf.Abs(p.x) >= Mathf.Abs(xpoint) && xpoint <= pyramide[1].x && p.x <= Mathf.Abs(pyramide[1].x);
    }

    bool DansPyramideZ(List<Vector3> pyramide, Vector3 p)
    {
        float coeffDirecteur = (pyramide[3].y - pyramide[0].y) / (pyramide[3].z - pyramide[0].z);
        coeffDirecteur = Mathf.Abs(coeffDirecteur);
        //square of p.x^2 + p.z^2
        float xy = Mathf.Sqrt(Mathf.Pow(p.x, 2) + Mathf.Pow(p.y, 2));
        float zpoint = coeffDirecteur * xy;
        if(p.z < 0)
        return Mathf.Abs(p.z) >= Mathf.Abs(zpoint) && zpoint >= pyramide[1].z && p.z >= pyramide[1].z;
        //Debug.Log(coeffDirecteur);
        //Debug.Log(xy);
        
        //Debug.Log(zpoint);
        //Debug.Log(pyramide[1].z);
        return p.z >= zpoint && zpoint <= pyramide[1].z && p.z <= pyramide[1].z;
    }


    float distanceDe(Vector3 v1 ,Vector3 v2)
    {
       return Mathf.Abs(v1.x-v2.x)+Mathf.Abs(v1.y-v2.y)+Mathf.Abs(v1.z-v2.z);
    }

    int MatchingVector(Vector3 direction)
    {
        List <Vector3> maListe = new List<Vector3>();
        maListe.Add(new Vector3(0,0,1)); //0 Devant
        maListe.Add(new Vector3(0,0,-1)); //1 Derriere
        maListe.Add(new Vector3(0,1,0));//2 Haut
        maListe.Add(new Vector3(1,0,0));//3 Droite
        maListe.Add(new Vector3(-1,0,0));//4 Gauche
        maListe.Add(new Vector3(-1,0,-1));//5 Gauche-Derriere
        maListe.Add(new Vector3(-1,0,1));//6 Gauche-Devant
        maListe.Add(new Vector3(1,0,-1));//7 Droite-Derriere
        maListe.Add(new Vector3(1,0,1));//8 Droite-Devant
        maListe.Add(new Vector3(1,1,0));//9 Haut-Droite
        maListe.Add(new Vector3(-1,1,0));//10 Haut-Gauche
        maListe.Add(new Vector3(0,1,1));//11 Haut-Devant
        maListe.Add(new Vector3(0,1,-1));//12 Haut-Derriere
        maListe.Add(new Vector3(-1,1,-1));//13 Haut-Gauche-Derriere
        maListe.Add(new Vector3(-1,1,1));//14 Haut-Gauche-Devant
        maListe.Add(new Vector3(1,1,-1));//15 Haut-Droite-Derriere
        maListe.Add(new Vector3(1,1,1));//16 Haut-Droite-Devant

        int index = 0;
        float minimum = distanceDe(maListe[0],direction);
        for(int i = 1; i<maListe.Count; i++)
        {
            float distance = distanceDe(maListe[i],direction);
            if(distance <= minimum)
            {
                minimum = distance;
                index = i;
            }
        }
        return index;
    }

    Vector3 MatchingVector2(Vector3 direction)
    {
        List <Vector3> maListe = new List<Vector3>();
        maListe.Add(new Vector3(0,0,1)); //0 Devant
        maListe.Add(new Vector3(0,0,-1)); //1 Derriere
        maListe.Add(new Vector3(0,1,0));//2 Haut
        maListe.Add(new Vector3(1,0,0));//3 Droite
        maListe.Add(new Vector3(-1,0,0));//4 Gauche
        maListe.Add(new Vector3(-1,0,-1));//5 Gauche-Derriere
        maListe.Add(new Vector3(-1,0,1));//6 Gauche-Devant
        maListe.Add(new Vector3(1,0,-1));//7 Droite-Derriere
        maListe.Add(new Vector3(1,0,1));//8 Droite-Devant
        maListe.Add(new Vector3(1,1,0));//9 Haut-Droite
        maListe.Add(new Vector3(-1,1,0));//10 Haut-Gauche
        maListe.Add(new Vector3(0,1,1));//11 Haut-Devant
        maListe.Add(new Vector3(0,1,-1));//12 Haut-Derriere
        maListe.Add(new Vector3(-1,1,-1));//13 Haut-Gauche-Derriere
        maListe.Add(new Vector3(-1,1,1));//14 Haut-Gauche-Devant
        maListe.Add(new Vector3(1,1,-1));//15 Haut-Droite-Derriere
        maListe.Add(new Vector3(1,1,1));//16 Haut-Droite-Devant

        int index = 0;
        float minimum = distanceDe(maListe[0],direction);
        for(int i = 1; i<maListe.Count; i++)
        {
            float distance = distanceDe(maListe[i],direction);
            if(distance <= minimum)
            {
                minimum = distance;
                index = i;
            }
        }
        return maListe[index];
    }

    public void CreateBranchFromPoint(List<Vector3> allPoints, Vector3 p, Vector3 direction,Vector3 origin, GameObject parent,float epaisseurBranche,float force = 1.5f)
    {//Y X Z
        this.GetComponent<GenerateurDeForme>().CreateVisualizationPoint(p, parent, epaisseurBranche);
        List<Vector3> pointsVu = new List<Vector3>();
        List<Vector3> visu1 = new List<Vector3>();
        List<Vector3> visu2 = new List<Vector3>();
        Vector3 vecteurNormal;
        Vector3 vecteurNormal2;
        bool isVertical = false;
        bool isHorizontal = false;
        bool isProfondeur = false;
        // switch (MatchingVector(direction))
        // {
        //     case 0:
        //         vecteurNormal = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,2);
        //         isProfondeur = true;
        //         break;
        //     case 1:
        //         vecteurNormal = new Vector3(0,0,-force);
        //         visu1 = champsDeVision(vecteurNormal, p,2);
        //         isProfondeur = true;
        //         break;
        //     case 2:
        //         vecteurNormal = new Vector3(0,force,0);
        //         visu1 = champsDeVision(vecteurNormal, p,0);
        //         isVertical = true;
        //         break;
        //     case 3:
        //         vecteurNormal = new Vector3(force,0,0);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         isHorizontal = true;
        //         break;
        //     case 4:
        //         vecteurNormal = new Vector3(-force,0,0);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         isHorizontal = true;
        //         break;
        //     case 5:
        //         vecteurNormal = new Vector3(-force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,-force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 6:
        //         vecteurNormal = new Vector3(-force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 7:
        //         vecteurNormal = new Vector3(force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,-force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 8:
        //         vecteurNormal = new Vector3(force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 9:
        //         vecteurNormal = new Vector3(force,0,0);
        //         vecteurNormal2 = new Vector3(0,force,0);
        //         visu1 = champsDeVision(vecteurNormal2,p,0);
        //         visu2 = champsDeVision(vecteurNormal, p,1);
        //         isVertical = true;
        //         isHorizontal = true;
        //         break;
        //     case 10:
        //         vecteurNormal = new Vector3(0,force,0);
        //         vecteurNormal2 = new Vector3(-force,0,0);
        //         visu1 = champsDeVision(vecteurNormal, p,0);
        //         visu2 = champsDeVision(vecteurNormal2,p,1);
        //         isVertical = true;
        //         isHorizontal = true;
        //         break;
        //     case 11:
        //         vecteurNormal = new Vector3(0,force,0);
        //         vecteurNormal2 = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,0);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isVertical = true;
        //         break;
        //     case 12:
        //         vecteurNormal = new Vector3(0,force,0);
        //         vecteurNormal2 = new Vector3(0,0,-force);
        //         visu1 = champsDeVision(vecteurNormal, p,0);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isVertical = true;
        //         break;
        //     case 13:
        //         vecteurNormal = new Vector3(-force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,-force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 14:
        //         vecteurNormal = new Vector3(-force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 15:
        //         vecteurNormal = new Vector3(force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,-force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     case 16:
        //         vecteurNormal = new Vector3(force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        //     default:
        //         vecteurNormal = new Vector3(force,0,0);
        //         vecteurNormal2 = new Vector3(0,0,force);
        //         visu1 = champsDeVision(vecteurNormal, p,1);
        //         visu2 = champsDeVision(vecteurNormal2,p,2);
        //         isProfondeur = true;
        //         isHorizontal = true;
        //         break;
        // }
        Debug.Log("isVertical: "+isVertical);
        Debug.Log("isHorizontal : "+isHorizontal);
        Debug.Log("isProfondeur : "+isProfondeur);
        foreach (Vector3 point in allPoints)
        {
            // if (DansPyramideY(visu1, point) && !pointsVu.Contains(point))
            // {
            //     if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
            //     if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
            //     if((p.y < point.y && p.y >= 0))
            //     pointsVu.Add(point);
            // }
            if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 30)
            {
                //if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
                //if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
                if((p.y < point.y && p.y >= 0))
                pointsVu.Add(point);
            }
        }
        // if(isVertical)
        // {
        //     foreach (Vector3 point in allPoints)
        //     {
        //         // if (DansPyramideY(visu1, point) && !pointsVu.Contains(point))
        //         // {
        //         //     if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //         //     if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //         //     if((p.y < point.y && p.y >= 0))
        //         //     pointsVu.Add(point);
        //         // }
        //         if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //         {
        //             // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //             // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //             if((p.y < point.y && p.y >= 0))
        //             pointsVu.Add(point);
        //         }
        //     }
        //     if(isHorizontal)
        //     {
        //         List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
        //         foreach(Vector3 point in pointsVu)
        //         {
        //             // if(DansPyramideX(visu2,elem) && !pointsVu.Contains(elem))
        //             // {
        //             //     if((p.z > elem.z && p.z <= 0) || (p.z < elem.z && p.z >= 0))
        //             //     if((p.x > elem.x && p.x <= 0) || (p.x < elem.x && p.x >= 0))
        //             //     if((p.y < elem.y && p.y >= 0))
        //             //     pointsVuCpy.Add(elem);
        //             // }
        //             if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //             {
        //                 // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //                 // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //                 if((p.y < point.y && p.y >= 0))
        //                 pointsVu.Add(point);
        //             }
        //         }
        //         pointsVu = pointsVuCpy;
        //     }else if(isProfondeur)
        //     {
        //         List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
        //         foreach(Vector3 point in pointsVu)
        //         {
        //             // if(DansPyramideZ(visu2,elem) && !pointsVu.Contains(elem))
        //             // {
        //             //     if((p.z > elem.z && p.z <= 0) || (p.z < elem.z && p.z >= 0))
        //             //     if((p.x > elem.x && p.x <= 0) || (p.x < elem.x && p.x >= 0))
        //             //     if((p.y < elem.y && p.y >= 0))
        //             //     pointsVuCpy.Add(elem);
        //             // }
        //             if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //             {
        //                 // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //                 // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //                 if((p.y < point.y && p.y >= 0))
        //                 pointsVu.Add(point);
        //             }
        //         }
        //         pointsVu = pointsVuCpy;
        //     }
        // }else if(isHorizontal)
        // {
        //     foreach (Vector3 point in allPoints)
        //     {
        //         // if (DansPyramideX(visu1, point) && !pointsVu.Contains(point))
        //         // {
        //         //     if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //         //     if((p.x > point.x && p.x <= 0) || (p.x < point.x && p.x >= 0))
        //         //     if((p.y < point.y && p.y >= 0))
        //         //     pointsVu.Add(point);
        //         // }
        //         if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //         {
        //             // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //             // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //             if((p.y < point.y && p.y >= 0))
        //             pointsVu.Add(point);
        //         }
        //     }
        //     if(isVertical)
        //     {
        //         List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
        //         foreach(Vector3 point in pointsVu)
        //         {
        //             // if(DansPyramideY(visu2,elem) && !pointsVu.Contains(elem))
        //             // {
        //             //     if((p.z > elem.z && p.z <= 0) || (p.z < elem.z && p.z >= 0))
        //             //     if((p.x > elem.x && p.x <= 0) || (p.x < elem.x && p.x >= 0))
        //             //     if((p.y < elem.y && p.y >= 0))
        //             //     pointsVuCpy.Add(elem);
        //             // }
        //             if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //             {
        //                 // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //                 // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //                 if((p.y < point.y && p.y >= 0))
        //                 pointsVu.Add(point);
        //             }
        //         }
        //         pointsVu = pointsVuCpy;
        //     }else if(isProfondeur)
        //     {
        //         List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
        //         foreach(Vector3 point in pointsVu)
        //         {
        //             // if(DansPyramideZ(visu2,elem) && !pointsVu.Contains(elem))
        //             // {
        //             //     if((p.z > elem.z && p.z <= 0) || (p.z < elem.z && p.z >= 0))
        //             //     if((p.x > elem.x && p.x <= 0) || (p.x < elem.x && p.x >= 0))
        //             //     if((p.y < elem.y && p.y >= 0))
        //             //     pointsVuCpy.Add(elem);
        //             // }
        //             if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //             {
        //                 // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //                 // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //                 if((p.y < point.y && p.y >= 0))
        //                 pointsVu.Add(point);
        //             }
        //         }
        //         pointsVu = pointsVuCpy;
        //     }
        // }else if(isProfondeur)
        // {
        //     foreach (Vector3 point in allPoints)
        //     {
        //         // if (DansPyramideZ(visu1, point) && !pointsVu.Contains(point))
        //         // {
        //         //     if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //         //     if((p.x > point.x && p.x <= 0) || (p.x < point.x && p.x >= 0))
        //         //     if((p.y < point.y && p.y >= 0))
        //         //     pointsVu.Add(point);
        //         // }
        //         if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //         {
        //             // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //             // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //             if((p.y < point.y && p.y >= 0))
        //             pointsVu.Add(point);
        //         }
        //     }

        //     if(isVertical)
        //     {
        //         List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
        //         foreach(Vector3 point in pointsVu)
        //         {
        //             // if(DansPyramideY(visu2,elem) && !pointsVu.Contains(elem))
        //             // {
        //             //     if((p.z > elem.z && p.z <= 0) || (p.z < elem.z && p.z >= 0))
        //             //     if((p.x > elem.x && p.x <= 0) || (p.x < elem.x && p.x >= 0))
        //             //     if((p.y < elem.y && p.y >= 0))
        //             //     pointsVuCpy.Add(elem);
        //             // }
        //             if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //             {
        //                 // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //                 // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //                 if((p.y < point.y && p.y >= 0))
        //                 pointsVu.Add(point);
        //             }
        //         }
        //         pointsVu = pointsVuCpy;
        //     }else if(isHorizontal)
        //     {
        //         List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
        //         foreach(Vector3 point in pointsVu)
        //         {
        //             // if(DansPyramideX(visu2,elem) && !pointsVu.Contains(elem))
        //             // {
        //             //     if((p.z > elem.z && p.z <= 0) || (p.z < elem.z && p.z >= 0))
        //             //     if((p.x > elem.x && p.x <= 0) || (p.x < elem.x && p.x >= 0))
        //                 // if((p.y < elem.y && p.y >= 0))
        //             //     pointsVuCpy.Add(elem);
        //             // }
        //             if(Vector3.Distance(point,p) <= force && !pointsVu.Contains(point) && Vector3.Angle(direction,point-p) <= 45)
        //             {
        //                 // if((p.z > point.z && p.z <= 0) || (p.z < point.z && p.z >= 0))
        //                 // if((p.x >= point.x && p.x <= 0) || (p.x <= point.x && p.x >= 0))
        //                 if((p.y < point.y && p.y >= 0))
        //                 pointsVu.Add(point);
        //             }
        //         }
        //         pointsVu = pointsVuCpy;
        //     }
        // }
    
        Debug.Log(pointsVu.Count);
        if(pointsVu.Count >=1)
        {
            cpt++;
            // List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
            // for(int i = 2; i < pointsVu.Count; i++)
            // {
            //     pointsVuCpy.Remove(pointsVu[i]);
            // }
            // pointsVu = pointsVuCpy;
            // Vector3 element = pointsVu[0];
            // if(pointsVu.Count == 2) element = moyenneDe2Vex(pointsVu[0],pointsVu[1]);
            // foreach(Vector3 elem in pointsVu)
            // {
            //     allPoints.Remove(elem);
            // }
            // if(element.y < p.y)
            // {
            //     element.y = p.y;
            // }
            // element.y=element.y*1.03f;
            // Debug.DrawLine(p, element, Color.green, 1000f);
            // Vector3 newDirection = element - p;
            // CreateBranchFromPoint(allPoints,element,newDirection,origin,force*0.90f);
            // System.Random random = new System.Random();
            // CreateBranchFromPoint(allPoints,element,NewRandomDirection(newDirection),origin,force*0.90f);
            //Random index 2 times
            Vector3 newDirection;
            Vector3 newDirection2;
            System.Random random = new System.Random();
            if(pointsVu.Count > 1)
            {
                int index1 = UnityEngine.Random.Range(0,pointsVu.Count);
                int index2 = UnityEngine.Random.Range(0,pointsVu.Count);
                
            
                while(index1 == index2)
                {
                    index2 = UnityEngine.Random.Range(0,pointsVu.Count);
                }
                List<Vector3> pointsVuCpy = new List<Vector3>(pointsVu);
                for(int i = 0; i<pointsVu.Count; i++)
                {
                    if(i != index1 && i != index2)
                    {
                        pointsVuCpy.Remove(pointsVu[i]);
                    }
                }
                pointsVu = pointsVuCpy;
                Debug.Log("PointsVu : "+pointsVu.Count);
                Vector3 element = moyenneDe2Vex(pointsVu[0],pointsVu[1]);
                foreach(Vector3 elem in pointsVu)
                {
                    allPoints.Remove(elem);
                }
                // Debug.DrawLine(p, element, Color.green, 1000f);
                // GenerateurDeForme gen = this.GetComponent<GenerateurDeForme>();
                // gen.CreateCylinder(p,element, parent, epaisseurBranche);
                // newDirection = element - p;
                // newDirection.y = Mathf.Abs(newDirection.y);
                // CreateBranchFromPoint(allPoints,element,newDirection,origin,parent,0.1f,force*1f);
                foreach(Vector3 elem in pointsVu)
                {
                    GenerateurDeForme gen = this.GetComponent<GenerateurDeForme>();
                    gen.CreateCylinder(p,elem, parent, epaisseurBranche);
                    newDirection = elem - p;
                    newDirection.y = Mathf.Abs(newDirection.y);
                    CreateBranchFromPoint(allPoints,elem,newDirection,origin, parent,0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent,0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent,0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);

                    // CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,force*1f);
                }

            }else{
                foreach(Vector3 elem in pointsVu)
                {
                    allPoints.Remove(elem);
                }
                foreach(Vector3 elem in pointsVu)
                {
                    GenerateurDeForme gen = this.GetComponent<GenerateurDeForme>();
                    gen.CreateCylinder(p,elem, parent, epaisseurBranche);
                    newDirection = elem - p;
                    newDirection.y = Mathf.Abs(newDirection.y);
                    CreateBranchFromPoint(allPoints,elem,newDirection,origin,parent,0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,parent, 0.1f,force*1f);
                    // CreateBranchFromPoint(allPoints,elem,NewRandomDirection(newDirection),origin,force*1f);
                }
                // Debug.DrawLine(p, direction, Color.green, 1000f);
                // newDirection2 = direction - p;
                // newDirection2.y = Mathf.Abs(newDirection2.y);
                // CreateBranchFromPoint(allPoints,direction,newDirection2,origin,force*1f);
                // if(UnityEngine.Random.Range(0,3) == 1)
                // CreateBranchFromPoint(allPoints,direction,NewRandomDirection(newDirection2),origin,force*1f);
            }
        }else{
            GameObject feuille = Instantiate(feuillePrefab,p,Quaternion.identity);
            feuille.transform.parent = parent.transform;
            float xLocalScale = (Vector3.Distance(p,p+direction.normalized)/4)/2;
            float yLocalScale = Vector3.Distance(p,p+direction.normalized)/4;
            float zLocalScale = (Vector3.Distance(p,p+direction.normalized)/4)/2;
            feuille.transform.localScale = new Vector3(xLocalScale,yLocalScale,zLocalScale);
            feuille.transform.position = p + direction.normalized;
            feuille.transform.up = direction.normalized;

            //Creer une sphere rouge pour voir ou pointe la direction
           

        }
    }

    Vector3 moyenneDe2Vex(Vector3 v1, Vector3 v2)
    {
        return new Vector3((v1.x+v2.x)/2,(v1.y+v2.y)/2,(v1.z+v2.z)/2);
    }

    void DeletePoint(Vector3 point,Vector3 origin)
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("Point");
        foreach(GameObject elem in points)
        {
            if(elem.transform.position+origin == point)
            {
                Destroy(elem);
            }
        }
    }

    Vector3 VectorMoyenEntreXVector(List<Vector3> points)
    {
        Vector3 vecteurMoyen = new Vector3(0,0,0);
        foreach(Vector3 point in points)
        {
            vecteurMoyen += point;
        }
        vecteurMoyen /= points.Count;
        return vecteurMoyen;
    }

    Vector3 NewRandomDirection(Vector3 direction)
    {
        System.Random random = new System.Random();
        
        float x = ((float)random.NextDouble() * 2f - 1f);
        float y = ((float)random.NextDouble());
        float z = ((float)random.NextDouble() * 2f - 1f);
        if((direction.x < 0 && x > 0) || (direction.x > 0 && x < 0))
        {
            x = -x;
        }
        return new Vector3(x,y,z);
    }
}