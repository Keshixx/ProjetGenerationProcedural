using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateurDeForme : MonoBehaviour
{
    public float tailleTronc = 1f;
    public float epaisseurTronc = 0.5f;
    public float hauteurArbre = 5f;

    public float epaisseurArbre = 0.5f;

    [SerializeField]
    private float force = 1f;

    [SerializeField]
    private GameObject TroncParent;
    [SerializeField]
    private GameObject CouronneParent;
    [SerializeField]
    private GameObject branchePrefab;
    [SerializeField]
    private GameObject noeudPrefab;

    //slider from 0.1 to 1
    [Range(1f, 20000f)]
    public float nombreDePoint = 100f;
    [Range(0.1f, 0.90f)]
    public float precisionTronc = 0.1f;

    public bool autoUpdate;
    GameObject cylinder;
    
    void Start()
    {
        GenererArbre();
        //Empty game object for cylinder
        cylinder = new GameObject("cylinder");

        cylinder.tag = "Tree";
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
        GameObject[] objects2 = GameObject.FindGameObjectsWithTag("Branche");
        GameObject[] objects3 = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject obj in objects)
        {
            DestroyImmediate(obj);
        }
        foreach (GameObject obj in objects2)
        {
            DestroyImmediate(obj);
        }

        foreach (GameObject obj in objects3)
        {
            DestroyImmediate(obj);
        }

        GameObject tronc = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        tronc.tag = "Tree";
        tronc.transform.position = new Vector3(0, 0, 0);
        tronc.name = "tronc";
        tronc.transform.localScale = new Vector3(epaisseurTronc, tailleTronc, epaisseurTronc);
        GameObject couronne = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        couronne.tag = "Tree";
        couronne.transform.position = new Vector3(0, tailleTronc + hauteurArbre/2, 0);
        couronne.name = "couronne";
        couronne.transform.localScale = new Vector3(epaisseurArbre, hauteurArbre, epaisseurArbre);
        
        //TransformFormToPoint(couronne, epaisseurArbre, hauteurArbre, precisionCouronne);
        TransformTroncToPoint(tronc, tailleTronc, epaisseurTronc, precisionTronc);
        CreateRandomPointInEllipsoide(couronne.transform.position, new Vector3(epaisseurArbre, hauteurArbre, epaisseurArbre), CouronneParent, (int)nombreDePoint);
        //Empty game object for cylinder
        
    }

    bool RandomBool()
    {
        return Random.Range(0, 5) == 0;
    }
    List<Vector3> pointsArray = new List<Vector3>();
    List<Vector3> pointsArray2 = new List<Vector3>();
    void TransformTroncToPoint(GameObject forme, float tailleTronc, float epaisseurTronc,float precision)
    {
        Vector3 point1 = new Vector3(forme.transform.position.x, forme.transform.position.y-tailleTronc, forme.transform.position.z);
        //random point in the forme.transform.localScale for x z
        float x = Random.Range(forme.transform.position.x - epaisseurTronc / 2, forme.transform.position.x + epaisseurTronc / 2);
        float z = Random.Range(forme.transform.position.z - epaisseurTronc / 2, forme.transform.position.z + epaisseurTronc / 2);
        Vector3 point2 = new Vector3(x, forme.transform.position.y, z);
        pointsArray2.Add(point1);
        pointsArray2.Add(point2);
        CreateVisualizationPoint(point1, TroncParent, 0.3f);
        CreateVisualizationPoint(point2, TroncParent, 0.3f);
        Debug.DrawLine(point1, point2, Color.green, 100f);
        CreateCylinder(point1, point2, TroncParent, 0.3f);
    }

    void CreateRandomPointInEllipsoide(Vector3 center, Vector3 size, GameObject parent, int numberOfPoints)
    {
        // Calcul des carrés des tailles
        float sizeXSquare = size.x * size.x;
        float sizeYSquare = size.y * size.y;
        float sizeZSquare = size.z * size.z;
        int i = 0;
        while(i < numberOfPoints)
        {
            float x = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
            float y = Random.Range(center.y - size.y / 2, center.y + size.y / 2);
            float z = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

            Vector3 point = new Vector3(x, y, z);

            if (PointIsInEllipsoide(point, center, size))
            {
                pointsArray.Add(point);
                i++;
            }
        }
        Branches branches = this.gameObject.GetComponent<Branches>();
        Vector3 PointLePlusBas = new Vector3(center.x, center.y - size.y / 2, center.z);
        Debug.DrawLine(pointsArray2[1], PointLePlusBas, Color.green, 100f);
        CreateCylinder(pointsArray2[1], PointLePlusBas, TroncParent, 0.3f);
        Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), 1, Random.Range(-1, 1f));
        branches.CreateBranchFromPoint(pointsArray, PointLePlusBas, randomDir, center, CouronneParent,0.1f, force);
    }

    bool PointIsInEllipsoide(Vector3 point, Vector3 center, Vector3 size)
    {
        if(point.x > center.x + size.x / 2 || point.x < center.x - size.x / 2)
        {
            return false;
        }
        if (point.y > center.y + size.y / 2 || point.y < center.y - size.y / 2)
        {
            return false;
        }
        if (point.z > center.z + size.z / 2 || point.z < center.z - size.z / 2)
        {
            return false;
        }
        float xy = Mathf.Pow(point.x - center.x, 2) / Mathf.Pow(size.x / 2, 2) + Mathf.Pow(point.y - center.y, 2) / Mathf.Pow(size.y / 2, 2);
        float z = Mathf.Pow(point.z - center.z, 2) / Mathf.Pow(size.z / 2, 2);
        if (xy + z <= 1)
        {
            return true;
        }
        return false;
    }

    /*void TransformFormToPoint(GameObject forme, float epaisseurArbre, float hauteurArbre,float precision)
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
                    pointsArray.Add(point + forme.transform.position);
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
        Vector3 elem = pointsArray[0];
        foreach(Vector3 point in pointsArray)
        {
            if(elem.y >= point.y)
            {
                elem = point;
            }
        }
        pointsArray.Remove(elem);
        Branches branches = this.gameObject.GetComponent<Branches>();
        branches.CreateBranchFromPoint(pointsArray,elem,new Vector3(0,1,0), forme.transform.position, 1f);
    }*/

    public void CreateVisualizationPoint(Vector3 position, GameObject parent, float epaisseur = 0.1f)
    {
        GameObject visualizationPoint = Instantiate(noeudPrefab);
        visualizationPoint.transform.localScale = new Vector3(epaisseur, epaisseur, epaisseur);
        visualizationPoint.transform.position = position;
        visualizationPoint.transform.parent = parent.transform;
        //tag Point
        visualizationPoint.tag = "Point";
    }


    public void CreateCylinder(Vector3 point1, Vector3 point2, GameObject parent, float epaisseur = 0.1f)
    {
        //use branches prefab
        GameObject cylinder = Instantiate(branchePrefab);
        cylinder.transform.localScale = new Vector3(epaisseur, Vector3.Distance(point1, point2) / 2, epaisseur);
        cylinder.transform.position = new Vector3((point1.x + point2.x) / 2, (point1.y + point2.y) / 2, (point1.z + point2.z) / 2);
        cylinder.transform.up = point2 - point1;
        cylinder.transform.parent = parent.transform;
        cylinder.tag = "Branche";
    }
}
