using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "DistrictssData", menuName = "ScriptableObjects/Districts", order = 2)]

public class DistrictSO : ScriptableObject
{
    public string name;
    public enum DistrictType { d1, d2, d3, d4 }
    public DistrictType district;
}
