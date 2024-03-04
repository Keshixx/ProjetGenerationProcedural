using System.Collections;
using System.Collections.Generic;
using DigitalRuby.RainMaker;
using UnityEngine;

public class Pluie : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject pluie;
    [SerializeField] Material skyMaterial;
    [SerializeField] private GameObject flaquePrefab;

    private float TailleMaxFlaque = 22f;

    //Normal == 97 97 97 intensité 1
    //Pluie intensité 0.4 255 255 255
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(pluie.GetComponent<RainScript>().RainIntensity == 0f)
        {
            if(flaquePrefab.transform.position.y > 0f)
            {
                Vector3 pos = flaquePrefab.transform.position;
                pos.y -= 0.01f;
                flaquePrefab.transform.position = pos;
            }
        }else
        {
            if(flaquePrefab.transform.position.y < TailleMaxFlaque)
            {
                Vector3 pos = flaquePrefab.transform.position;
                pos.y += 0.01f;
                flaquePrefab.transform.position = pos;
            }
        }
        if(Input.GetKeyDown(KeyCode.P))
        if(pluie.GetComponent<RainScript>().RainIntensity == 0f)
        {
            pluie.GetComponent<RainScript>().RainIntensity = 1f;
            //Hexadecimal FFFFFF
            skyMaterial.SetColor("_Tint", new Color(1f, 1f, 1f));

            skyMaterial.SetFloat("_Exposure", 0.22f);
        }
        else
        {
            pluie.GetComponent<RainScript>().RainIntensity = 0f;
            
            skyMaterial.SetColor("_Tint", new Color(0.45f, 0.45f, 0.45f));
            skyMaterial.SetFloat("_Exposure", 1f);
        }
    }

}
