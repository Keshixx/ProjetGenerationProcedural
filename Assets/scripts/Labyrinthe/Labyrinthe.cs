using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Labyrinthe : MonoBehaviour
{

    public float coordX = 0;
    public float coordY = 0;
    private List<Cellules> Cellules = new List<Cellules>();
    private List<Cellules> Chemin = new List<Cellules>();
    public int nbCellules = 100;
    int nbCellulesX;
    int nbCellulesY;
    [SerializeField]
    GameObject citron;
    [SerializeField]
    GameObject feuilles;
    [SerializeField]
    Material citronMaterial;
    [SerializeField]
    GameObject noeudPrefab;
    [SerializeField]
    GameObject branchePrefab;
    void Start()
    {
        int carre = (int)Mathf.Sqrt(nbCellules);
        if(carre*carre == nbCellules)
        {
            nbCellulesX = carre;
            nbCellulesY = carre;
        }
        else
        {
            //redefine the number of cells to be a square
            nbCellules = carre * carre;
            nbCellulesX = carre;
            nbCellulesY = carre;
        }
        GenerateCellules();
        AfficherLabyrinthe2D();
    }

    void Update()
    {
        
    }

    private bool HaveVoisinCellule(Cellules cellule)
    {
        if(cellule.coordX == 0 && cellule.coordY !=0)
        {
            if(Cellules[cellule.index+1].isVisited)
            {
                return true;
            }
            return false;
        }
        if(cellule.coordX == nbCellulesX-1 && cellule.coordY !=0)
        {
            if(Cellules[cellule.index-1].isVisited)
            {
                return true;
            }
            return false;
        }
        if(cellule.coordY == 0 && cellule.coordX !=0)
        {
            if(Cellules[cellule.index+nbCellulesX].isVisited)
            {
                return true;
            }
            return false;
        }
        if(cellule.coordY == nbCellulesY-1 && cellule.coordX !=0)
        {
            if(Cellules[cellule.index-nbCellulesX].isVisited)
            {
                return true;
            }
            return false;
        }
        return false;
    }

    private void GenerateCellules()
    {
        for (int i = 0; i < nbCellules; i++)
        {
            Cellules.Add(new Cellules(coordX, coordY, null, null, null, null, i));
            Cellules[i].isWall = true;
            //plane in black
            
            coordX++;
            if (coordX == nbCellulesX)
            {
                coordX = 0;
                coordY++;
            }
        }
        for(int i = 1; i<nbCellulesY-1; i+=2)
        {
            for(int j = 1; j<nbCellulesX-1; j+=2)
            {
                if(Cellules[i*nbCellulesX+j].coordX > 1)
                {
                    Cellules[i * nbCellulesX + j].voisinGauche = Cellules[i * nbCellulesX + j - 2];
                }
                if(Cellules[i * nbCellulesX + j].coordX < nbCellulesX-2)
                {
                    Cellules[i * nbCellulesX + j].voisinDroite = Cellules[i * nbCellulesX + j + 2];
                }
                if(Cellules[i * nbCellulesX + j].coordY > 1)
                {
                    Cellules[i * nbCellulesX + j].voisinBas = Cellules[Cellules[i  * nbCellulesX + j].index - 2*nbCellulesX];
                }
                if(Cellules[i * nbCellulesX + j].coordY < nbCellulesY-2)
                {

                    Cellules[i * nbCellulesX + j].voisinHaut = Cellules[Cellules[i * nbCellulesX + j].index + 2*nbCellulesX];
                }
                Chemin.Add(Cellules[i * nbCellulesX + j]);
            }
        }

        int index = Random.Range(0, Chemin.Count);
        GenerateLabyrinthe(Chemin[index]);
    }

    private bool GenerateLabyrinthe(Cellules cellule)
    {
        cellule.isVisited = true;
        cellule.isWall = false;
        List<Cellules> voisins = FindVoisin(cellule);
        Debug.Log(voisins.Count);
        if(voisins.Count == 0)
        {
            return false;
        }
        //shuffle the list of neighbors
        for (int i = 0; i < voisins.Count; i++)
        {
            Cellules temp = voisins[i];
            int randomIndex = Random.Range(i, voisins.Count);
            voisins[i] = voisins[randomIndex];
            voisins[randomIndex] = temp;
        }
        foreach(Cellules voisin in voisins)
        {
            //mur entre la cellule et le voisin
            if(voisin.isVisited)
            {
                continue;
            }
            if(voisin == cellule.voisinHaut)
            {
                Cellules[cellule.index + nbCellulesX].isWall = false;
            }else if(voisin == cellule.voisinBas)
            {
                Cellules[cellule.index - nbCellulesX].isWall = false;
            }else if(voisin == cellule.voisinGauche)
            {
                Cellules[cellule.index - 1].isWall = false;
            }else
            {
                Cellules[cellule.index + 1].isWall = false;
            }
            voisin.isVisited = true;
            voisin.isWall = false;
            GenerateLabyrinthe(voisin);
        }
        return true;
    }

    private List<Cellules> AllCellulesWithoutLeft()
    {
        List<Cellules> cellules = new List<Cellules>();
        for (int i = 0; i < nbCellules; i++)
        {
            if (Cellules[i].voisinGauche == null)
            {
                cellules.Add(Cellules[i]);
            }
        }
        return cellules;
    }

    private List<Cellules> FindVoisin(Cellules cellule)
    {
        List<Cellules> voisins = new List<Cellules>();
        if (cellule.voisinHaut != null && !cellule.voisinHaut.isVisited)
        {
            voisins.Add(cellule.voisinHaut);
        }
        if (cellule.voisinBas != null && !cellule.voisinBas.isVisited)
        {
            voisins.Add(cellule.voisinBas);
        }
        if (cellule.voisinGauche != null && !cellule.voisinGauche.isVisited)
        {
            voisins.Add(cellule.voisinGauche);
        }
        if (cellule.voisinDroite != null && !cellule.voisinDroite.isVisited)
        {
            voisins.Add(cellule.voisinDroite);
        }
        return voisins;
    }

    private void AfficherLabyrinthe2D()
    {
        for (int i = 0; i < nbCellules; i++)
        {
            GameObject cellule = GameObject.CreatePrimitive(PrimitiveType.Plane);
            cellule.transform.position = new Vector3(Cellules[i].coordX, 0, Cellules[i].coordY);
            cellule.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            if(Cellules[i].isWall)
            {
                cellule.GetComponent<Renderer>().material.color = Color.black;
                cellule.name = "Mur " + i;
                cellule.tag = "Mur";
            }
            else
            {
                cellule.GetComponent<Renderer>().material.color = Color.white;
                cellule.name = "Cellule " + i;
                cellule.tag = "Cellule";
            }
            
        }
        List<Cellules> bordures = new List<Cellules>();
        for(int i = 0; i<nbCellules; i++)
        {
            if(Cellules[i].coordX == 0 || Cellules[i].coordX == nbCellulesX-1 || Cellules[i].coordY == 0 || Cellules[i].coordY == nbCellulesY-1)
            {
                bordures.Add(Cellules[i]);
            }
        }
        int random1 = Random.Range(0, bordures.Count);
        while(!HaveVoisinCellule(bordures[random1]))
        {
            //supprimer la cellule de la liste des bordures
            bordures.RemoveAt(random1);
            random1 = Random.Range(0, bordures.Count);
        }
        int random2 = Random.Range(0, bordures.Count);
        while(random2 == random1 || !HaveVoisinCellule(bordures[random2]))
        {
            //supprimer la cellule de la liste des bordures
            bordures.RemoveAt(random2);
            random2 = Random.Range(0, bordures.Count);
        }
        GameObject mur1 = GameObject.Find("Mur " + bordures[random1].index);
        GameObject mur2 = GameObject.Find("Mur " + bordures[random2].index);
        mur1.GetComponent<Renderer>().material.color = Color.white;
        mur2.GetComponent<Renderer>().material.color = Color.white;
    }

    private void AfficherLabyrinthe3D()
    {
        for (int i = 0; i < nbCellules; i++)
        {
            
            if(Cellules[i].isWall)
            {
                GameObject cellule = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cellule.transform.position = new Vector3(Cellules[i].coordX, 0, Cellules[i].coordY);
                cellule.transform.localScale = new Vector3(1f, 1f, 1f);
                cellule.GetComponent<Renderer>().material.color = Color.black;
                cellule.name = "Mur " + i;
                cellule.tag = "Mur";
            }
            else
            {
                GameObject cellule = GameObject.CreatePrimitive(PrimitiveType.Plane);
                cellule.transform.position = new Vector3(Cellules[i].coordX, 0, Cellules[i].coordY);
                cellule.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                cellule.GetComponent<Renderer>().material.color = Color.white;
                cellule.name = "Cellule " + i;
                cellule.tag = "Cellule";
            }
            
        }
        List<Cellules> bordures = new List<Cellules>();
        for(int i = 0; i<nbCellules; i++)
        {
            if(Cellules[i].coordX == 0 || Cellules[i].coordX == nbCellulesX-1 || Cellules[i].coordY == 0 || Cellules[i].coordY == nbCellulesY-1)
            {
                bordures.Add(Cellules[i]);
            }
        }
        int random1 = Random.Range(0, bordures.Count);
        while(!HaveVoisinCellule(bordures[random1]))
        {
            //supprimer la cellule de la liste des bordures
            bordures.RemoveAt(random1);
            random1 = Random.Range(0, bordures.Count);
        }
        int random2 = Random.Range(0, bordures.Count);
        while(random2 == random1 || !HaveVoisinCellule(bordures[random2]))
        {
            //supprimer la cellule de la liste des bordures
            bordures.RemoveAt(random2);
            random2 = Random.Range(0, bordures.Count);
        }
        GameObject entree = GameObject.Find("Mur " + bordures[random1].index);
        GameObject sortie = GameObject.Find("Mur " + bordures[random2].index);
        //Destroy them to replace by plane
        Destroy(entree);
        Destroy(sortie);
        GameObject entreePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        entreePlane.transform.position = new Vector3(bordures[random1].coordX, 0, bordures[random1].coordY);
        entreePlane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        entreePlane.GetComponent<Renderer>().material.color = Color.white;
        entreePlane.name = "Cellule "+bordures[random1].index;
        entreePlane.tag = "Cellule";
        GameObject sortiePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        sortiePlane.transform.position = new Vector3(bordures[random2].coordX, 0, bordures[random2].coordY);
        sortiePlane.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        sortiePlane.GetComponent<Renderer>().material.color = Color.white;
        sortiePlane.name = "Cellule "+bordures[random2].index;
        sortiePlane.tag = "Cellule";
    }

    private void AfficherLabyrintheArbre()
    {
        
        foreach(Cellules cell in Cellules)
        {
            if(cell.isWall)
            {
                GameObject mur = new GameObject();
                mur.name = "Mur " + cell.index;
                mur.tag = "Mur";
                mur.transform.position = new Vector3(cell.coordX, 0, cell.coordY);
                mur.AddComponent<GenerateurDeForme>();
                mur.AddComponent<Branches>();
                Branches branches = mur.GetComponent<Branches>();
                branches.citronPrefab = citron;
                branches.feuillePrefab = feuilles;
                branches.citronMaterial = citronMaterial;
                GenerateurDeForme genForm = mur.GetComponent<GenerateurDeForme>();
                genForm.branchePrefab = branchePrefab;
                genForm.noeudPrefab = noeudPrefab;
                genForm.tailleTronc = 0.001f;
                genForm.epaisseurTronc = 0.1f;
                genForm.hauteurArbre = 1f;
                genForm.epaisseurArbre = 1f;
                genForm.force = 0.2f;
                genForm.nombreDePoint = 600f;
                genForm.precisionTronc = 0.01f;
                genForm.GenererArbre(cell.coordX,0, cell.coordY);
            }else{
                //creer un plane de 0.1f en blanc
                GameObject cellule = GameObject.CreatePrimitive(PrimitiveType.Plane);
                cellule.transform.position = new Vector3(cell.coordX, 0, cell.coordY);
                cellule.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                cellule.GetComponent<Renderer>().material.color = Color.white;
                cellule.name = "Cellule " + cell.index;
                cellule.tag = "Cellule";
            }
        }
    }
}
