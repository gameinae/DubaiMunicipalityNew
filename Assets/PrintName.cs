using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintName : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string p = "";
        for(int i = 0; i < transform.childCount; i++)
            p+= transform.GetChild(i).name+"\n";
        print(p);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
