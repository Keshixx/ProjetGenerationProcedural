using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mockito3 : MonoBehaviour
{
    float timeToFall;
    float timeToLive = 10f;
    void Start()
    {
        timeToFall = UnityEngine.Random.Range(5,15);
    }
    void Update()
    {
        if(this.gameObject.transform.position.y < -10)
        {
            Destroy(this.gameObject);
        }
        if(timeToFall <= 0)
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
        else
        {
            timeToFall -= Time.deltaTime;
        }
        if(GetComponent<Rigidbody>().useGravity && EstArreter() && IsGrounded())
        {
            Color couleur = GetComponent<Renderer>().material.color;
            Vector3 scale = this.transform.localScale;
            if(couleur.r>0 && couleur.g>0 && couleur.b>0)
            {
                couleur.r -= 0.01f;
                couleur.g -= 0.01f;
                couleur.b -= 0.01f;
                scale.x -= 0.005f;
                scale.y -= 0.005f;
                scale.z -= 0.005f;
                this.GetComponent<Renderer>().material.color = couleur;
                this.transform.localScale = scale;
            }
            else{
                GetComponent<Rigidbody>().useGravity = false;
                if(UnityEngine.Random.Range(0,2) == 0)
                {
                    Vector3 position = this.transform.position; 
                    GenerateurDeForme genForme = GetComponent<GenerateurDeForme>();
                    genForme.tailleTronc=2f;
                    genForme.epaisseurTronc = 1f;
                    genForme.epaisseurArbre = 50f;
                    genForme.hauteurArbre = 10f;
                    genForme.nombreDePoint = 600;
                    genForme.GenererArbre(position.x,position.y,position.z);
                    genForme.ClearVariables();
                    Debug.Log("Position: " + position);
                    Debug.Log("Couleur: " + couleur);
                }
                Destroy(this.gameObject);
            }
        }
    }



    bool EstArreter()
    {
        return GetComponent<Rigidbody>().velocity.y == 0 && GetComponent<Rigidbody>().velocity.x == 0 && GetComponent<Rigidbody>().velocity.z == 0;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }


}
