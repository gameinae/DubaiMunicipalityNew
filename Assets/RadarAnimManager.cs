using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarAnimManager : MonoBehaviour
{
    [SerializeField] Sprite[] RadarSprites;
    int i=0;
    // Start is called before the first frame update
    void Start()
    {
        i = 0;
        InvokeRepeating("ApplySprite", 1f,1f);
        
    }

    // Update is called once per frame
    void Update()
    {
       // StartCoroutine(ApplySprite(i));
    }
    void ApplySprite()
    {
       
        if (i < RadarSprites.Length) this.GetComponent<Image>().sprite = RadarSprites[i];
        if (i <= RadarSprites.Length - 1) i++;
        else i = RadarSprites.Length;
        if (i == RadarSprites.Length)
        {
            i = 0;
        }
    }
}
