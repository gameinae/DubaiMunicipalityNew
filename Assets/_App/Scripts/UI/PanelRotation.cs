using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelRotation : MonoBehaviour
{
    Camera cam;
    private void Start()
    {
        cam = Camera.main;
    }
    void Update()
    {
        transform.
            LookAt(new Vector3(cam.transform.position.x
            , 0,cam.transform.position.z));
    }
}
