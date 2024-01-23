using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurDeForme : MonoBehaviour
{
    public float tailleTronc = 1f;
    public float epaisseurTronc = 0.5f;
    public float hauteurArbre = 5f;

    public float epaisseurArbre = 0.5f;

    //slider from 0.1 to 1
    [Range(0.1f, 0.90f)]
    public float precisionCouronne = 0.1f;
    [Range(0.1f, 0.90f)]
    public float precisionTronc = 0.1f;

    public bool autoUpdate;

    void Start()
    {
        GenererArbre();
    }

    public void GenererArbre()
    {
        /*
            GameObject tronc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            tronc.transform.localScale = new Vector3(epaisseurTronc, tailleTronc, epaisseurTronc);
            tronc.transform.position = new Vector3(0, 0, 0);

            GameObject couronne = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            couronne.transform.localScale = new Vector3(epaisseurArbre, hauteurArbre, epaisseurArbre);
            couronne.transform.position = new Vector3(0, tailleTronc + hauteurArbre / 2, 0);
            couronne.transform.parent = tronc.transform;

            TransformFormToPoint(couronne);
        */
        //select object tronc and couronne do delete them before creating new ones
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Tree");
        foreach (GameObject obj in objects)
        {
            DestroyImmediate(obj);
        }

        GameObject tronc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tronc.tag = "Tree";
        tronc.transform.position = new Vector3(0, tailleTronc, 0);

        GameObject couronne = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        couronne.tag = "Tree";
        couronne.transform.position = new Vector3(0, tailleTronc*2 + (hauteurArbre/2)-0, 0);
        couronne.transform.parent = tronc.transform;
        TransformFormToPoint(couronne, epaisseurArbre, hauteurArbre, precisionCouronne);
        TransformTroncToPoint(tronc, tailleTronc, epaisseurTronc, precisionTronc);
    }

    bool RandomBool()
    {
        return Random.Range(0, 5) == 0;
    }
    List<Vector3> pointsArray = new List<Vector3>();
    List<Vector3> pointsArray2 = new List<Vector3>();
    void TransformTroncToPoint(GameObject forme, float tailleTronc, float epaisseurTronc,float precision)
    {
        pointsArray2.Clear();
        precision = 1 - precision;
        MeshFilter meshFilter = forme.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        GameObject parent = new GameObject("points");
        parent.tag = "Tree";
        Vector3[] vertices = mesh.vertices;
        float tailleTronc2 = this.tailleTronc;
        float epaisseurTronc2 = this.epaisseurTronc;
        Vector3[] vertices2 = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 point = vertices[i];
            point.x *= epaisseurTronc;
            point.y *= tailleTronc;
            point.z *= epaisseurTronc;
            vertices2[i] = point;
        }

        //creer les points de la forme pour la visualiser par rapport au mesh et non au bounds et par rapport à hauteurArbre et epaisseurArbre
        while(tailleTronc > 0)
        {
            epaisseurTronc = epaisseurTronc2;
            while (epaisseurTronc > 0)
            {
                for (int i = 0; i < vertices2.Length; i++)
                {
                    Vector3 point = vertices[i];
                    point.x *= epaisseurTronc;
                    point.y *= tailleTronc;
                    point.z *= epaisseurTronc;
                    if (RandomBool() && RandomBool())
                    {
                        CreateVisualizationPoint(point, parent);
                        pointsArray2.Add(point);
                    }
                }
                epaisseurTronc -= precision;
            }
            tailleTronc -= precision;
        }
        
        mesh.vertices = vertices2;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        parent.transform.position = forme.transform.position;
        Bounds bounds = mesh.bounds;
        bounds.center = forme.transform.position;

        forme.GetComponent<MeshFilter>().mesh = mesh;
        mesh.bounds = bounds;
        GameObject branch = new GameObject("branch");
        branch.tag = "Tree";
        //CreateBranchFromPoint(pointsArray2, pointsArray2[0], precision, branch);
        branch.transform.position = forme.transform.position;
    }

    void TransformFormToPoint(GameObject forme, float epaisseurArbre, float hauteurArbre,float precision)
    {
        precision = 1 - precision;
        MeshFilter meshFilter = forme.GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        GameObject parent = new GameObject("points");
        parent.tag = "Tree";
        Vector3[] vertices = mesh.vertices;
        float epaisseurArbre2 = this.epaisseurArbre;
        float hauteurArbre2 = this.hauteurArbre;
        Vector3[] vertices2 = new Vector3[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 point = vertices[i];
            point.x *= epaisseurArbre;
            point.y *= hauteurArbre;
            point.z *= epaisseurArbre;
            vertices2[i] = point;
        }
        
        //creer les points de la forme pour la visualiser par rapport au mesh et non au bounds et par rapport à hauteurArbre et epaisseurArbre
        while(epaisseurArbre > 0.2 && hauteurArbre > 0.2)
        { 
            for (int i = 0; i < vertices2.Length; i++)
            {
                Vector3 point = vertices[i];
                point.x *= epaisseurArbre;
                point.y *= hauteurArbre;
                point.z *= epaisseurArbre;
                if(RandomBool() && RandomBool())
                {
                    CreateVisualizationPoint(point, parent);
                    pointsArray.Add(point);
                }
            }
            epaisseurArbre -= precision;
            hauteurArbre -= precision;
        }
        mesh.vertices = vertices2;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        parent.transform.position = forme.transform.position;
        Bounds bounds = mesh.bounds;
        bounds.center = forme.transform.position;
        
        forme.GetComponent<MeshFilter>().mesh = mesh;
        mesh.bounds = bounds;
        //CreateBranchFromPoint(pointsArray, pointsArray[0], precision, parent);
    }

    void CreateVisualizationPoint(Vector3 position, GameObject parent)
    {
        GameObject visualizationPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visualizationPoint.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        visualizationPoint.transform.position = position;
        visualizationPoint.transform.parent = parent.transform;
    }

    //Retourne le vecteur qui va de p1 à p2
    Vector3 vectFrom2Points(Vector3 point1, Vector3 point2)
    {
        return new Vector3(point2.x - point1.x, point2.y - point1.y, point2.z - point1.z);
    }


    Vector3 orthogonal(Vector3 vect)
    {
        return new Vector3(vect.z, vect.y, -vect.x);
    }


    List<Vector3> champsDeVision(Vector3 direction,Vector3 p1)
    {
        Vector3 meridianne=p1+direction;
        Vector3 orthogo = orthogonal(direction);
        Vector3 p2 = meridianne + orthogo;
        Vector3 p3 = meridianne - orthogo;
        return new List<Vector3> { p1, p2, p3 };
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

       return ((a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x)>0 && 
       (a.z - p.z) * (b.y - p.y) - (a.y - p.y) * (b.z - p.z)>0 && 
       (a.x - p.x) * (b.z - p.z) - (a.z - p.z) * (b.x - p.x)>0 ) ||
         ((a.x - p.x) * (b.y - p.y) - (a.y - p.y) * (b.x - p.x) < 0 &&
         (a.z - p.z) * (b.y - p.y) - (a.y - p.y) * (b.z - p.z) < 0 &&
            (a.x - p.x) * (b.z - p.z) - (a.z - p.z) * (b.x - p.x) < 0);
    }

    bool estEnVu(List<Vector3> points, Vector3 p)
    {
        return triangleDesEnfers(points[0], points[1], p) && triangleDesEnfers(points[0], points[2], p) && triangleDesEnfers(points[1], points[2], p);
    }

    void CreateBranchFromPoint(List<Vector3> allPoints, Vector3 p,Vector3 direction)
    {
        List<Vector3> pointsVu = new List<Vector3>();
        List<Vector3> visu = champsDeVision(direction, p);
        foreach (Vector3 point in allPoints)
        {
            if (estEnVu(visu, point))
            {
                pointsVu.Add(point);
            }
        }
        Debug.Log(pointsVu.Count);
    }


}
