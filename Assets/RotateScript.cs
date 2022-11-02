using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScript : MonoBehaviour
{

    // Start is called before the first frame update
    void OnEnable()
    {
        //this.transform.localRotation = new Vector3(0,10*Time.deltaTime,0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime*20f);
    }
}
