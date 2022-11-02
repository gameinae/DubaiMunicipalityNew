using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OptionsManager : MonoBehaviour
{
    [SerializeField] Material ResedentialMat ,BuildingMat;
    [SerializeField] TMP_Dropdown TempOptionsDrpDown;
    [SerializeField]  float Terrain_R, Terrain_G, Terrain_B;
    [SerializeField]  float ResedentialMat_R, ResedentialMat_G, ResedentialMat_B;
    [SerializeField]  float BuildingMat_R, BuildingMat_G, BuildingMat_B;
    [SerializeField]  float Terrain_R2, Terrain_G2, Terrain_B2;
    [SerializeField]  float Terrain_R3, Terrain_G3, Terrain_B3;
    [SerializeField]  float ResedentialMat_R2, ResedentialMat_G2, ResedentialMat_B2;
    [SerializeField] float BuildingMat_R2, BuildingMat_G2, BuildingMat_B2;
    [SerializeField] GameObject TerrainObj, ResedentialMatObj,ResedentialMatObj2,ResedentialMatObj3,ResedentialMatObj4,ResedentialMatObj5, BuildingMatObj;
    void Awake()
    {
        //option1 values
        Terrain_R = (float)79/255; Terrain_G = (float)106 /255; Terrain_B = (float)26 /255;
        ResedentialMat_R = (float)143/255; ResedentialMat_G = (float)138 / 255; ResedentialMat_B = (float)130 / 255;
        BuildingMat_R = (float)200 /255; BuildingMat_G = (float)243 / 255; BuildingMat_B = (float)255 / 255;
        //option2 values
        Terrain_R2 = (float)72 / 255; Terrain_G2 = (float)119 / 255; Terrain_B2 = (float)67 / 255;
        ResedentialMat_R2 = (float)65/ 255; ResedentialMat_G2 = (float)159/ 255; ResedentialMat_B2 = (float)161 / 255;
        BuildingMat_R2 = (float)173 / 255; BuildingMat_G2 = (float)125 / 255; BuildingMat_B2 = (float)125 / 255;

        //option3 values
        Terrain_R3 = (float)217 / 255; Terrain_G3 = (float)215 / 255; Terrain_B3 = (float)215 / 255;
       

    }
    void start() {
        Option1();

    }
    public void _TempOptionsDrpDown() {
        if (TempOptionsDrpDown.value==0) {
            Option1();
        } else if (TempOptionsDrpDown.value == 1) {
            Option2();
        } else if (TempOptionsDrpDown.value == 2) {
            Option3();
        }
    }
    void Option1() {
        TerrainObj.GetComponent<Renderer>().material.color = new Vector4(Terrain_R, Terrain_G, Terrain_B,1);
        ResedentialMatObj.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R, ResedentialMat_G, ResedentialMat_B, 1);
        ResedentialMatObj2.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R, ResedentialMat_G, ResedentialMat_B, 1);
        ResedentialMatObj3.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R, ResedentialMat_G, ResedentialMat_B, 1);
        ResedentialMatObj4.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R, ResedentialMat_G, ResedentialMat_B, 1);
        ResedentialMatObj5.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R, ResedentialMat_G, ResedentialMat_B, 1);
        // BuildingMatObj.GetComponent<Renderer>().material.color = new Vector4(BuildingMat_R , BuildingMat_G , BuildingMat_B, 1);
        ResedentialMatObj.GetComponent<Renderer>().material.color =new Color32(255,230,240,255);

    }
    void Option2() {
        TerrainObj.GetComponent<Renderer>().material.color = new Vector4(Terrain_R2 , Terrain_G2, Terrain_B2, 1);
        ResedentialMatObj.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        ResedentialMatObj2.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        ResedentialMatObj3.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        ResedentialMatObj4.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        ResedentialMatObj5.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
      //  BuildingMatObj.GetComponent<Renderer>().material.color = new Vector4(BuildingMat_R2 , BuildingMat_G2 , BuildingMat_B2 , 1);

    }
    void Option3() {
        TerrainObj.GetComponent<Renderer>().material.color = new Vector4(Terrain_R3 , Terrain_G3, Terrain_B3, 1);
        //ResedentialMatObj.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        //ResedentialMatObj2.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        //ResedentialMatObj3.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        //ResedentialMatObj4.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
        //ResedentialMatObj5.GetComponent<Renderer>().material.color = new Vector4(ResedentialMat_R2 , ResedentialMat_G2 , ResedentialMat_B2 , 1);
      //  BuildingMatObj.GetComponent<Renderer>().material.color = new Vector4(BuildingMat_R2 , BuildingMat_G2 , BuildingMat_B2 , 1);

    }
   
}
