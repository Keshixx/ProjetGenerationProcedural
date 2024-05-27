using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafFalling : MonoBehaviour
{
    // Start is called before the first frame update
    bool isPressed = false;
    [SerializeField]
    float distance = 2f;
    float timeToFall;
    void Start()
    {
        timeToFall = UnityEngine.Random.Range(0f,10f);
    }

    // Update is called once per frame
    void Update()
    {
        //si j'appuie sur la touche espace
        if(timeToFall <= 0)
        {
            isPressed = true;
        }else
        {
            timeToFall -= Time.deltaTime;
        }
        if(isPressed)
        {
            Color couleur = GetComponent<SpriteRenderer>().color;
            if(couleur.r<5 && couleur.g>0 && couleur.b>0)
            {
                couleur.r += 0.0008f;
                couleur.g -= 0.0008f;
                couleur.b -= 0.0008f;
                this.GetComponent<SpriteRenderer>().color = couleur;
            }
            else{
                isPressed = false;
                //Add rigidbody2D
                this.gameObject.AddComponent<Rigidbody2D>();
                // Vector3 position = this.transform.position;
                // Vector3 centre = new Vector3(position.x + distance, position.y, position.z);
                // RotationInf(180,330,centre);
            }
        }
        if(EstEnDehorsDeLaMap())
        {
            Destroy(this.gameObject);
        }
    }

    // void RotationInf(int start,int fin, Vector3 centre)
    // {
    //     if(EstEnDehorsDeLaMap())
    //     {
    //         Destroy(this.gameObject);
    //     }
    //     for(int i = start; i<fin; i++)
    //     {

    //         Vector3 point = ReturnPoint(centre,i);
    //         //translate la feuille vers le point
    //         this.transform.Translate(point);
    //     }
    //     start = 360;
    //     Vector3 position = this.transform.position;
    //     centre = new Vector3(position.x - distance, position.y, position.z);
    //     fin = 210;
    //     RotationSup(start,fin,centre);
    // }

    // void RotationSup(int start,int fin, Vector3 centre)
    // {
    //     if(EstEnDehorsDeLaMap())
    //     {
    //         Destroy(this.gameObject);
    //     }
    //     for(int i = start; i>fin; i--)
    //     {

    //         Vector3 point = ReturnPoint(centre,i);
    //         //translate la feuille vers le point
    //         this.transform.Translate(point);
    //     }
    //     start = 180;
    //     Vector3 position = this.transform.position;
    //     centre = new Vector3(position.x + distance, position.y, position.z);
    //     fin = 330;
    //     RotationInf(start,fin,centre);
    // }

    Vector3 ReturnPoint(Vector3 center, int degre)
    {
        float x = center.x + 1 * Mathf.Cos(degre * Mathf.PI / 180);
        float y = center.y + 1 * Mathf.Sin(degre * Mathf.PI / 180);
        return new Vector3(x,y,0);
    }

    bool EstEnDehorsDeLaMap()
    {
        Vector3 parentPosition = this.transform.parent.position;
        Vector3 position = this.transform.position;
        Vector3 reelPos = new Vector3(parentPosition.x + position.x, parentPosition.y + position.y, parentPosition.z + position.z);
        if(reelPos.y < -10)
        {
            return true;
        }
        return false;
    }
}
