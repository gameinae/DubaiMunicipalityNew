using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingsData", menuName = "ScriptableObjects/Building", order = 1)]

public class BuildingsSO : ScriptableObject
{
    public string name;
    public Material material;
    public float nCoordinate;
    public float eCoordinate;
    public enum DistrictType { d1,d2,d3,d4}
    public DistrictType district;
}
