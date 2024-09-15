using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;


[System.Serializable]
public class UpgradeData
{
    public string type;
    public float[] values;
}

[System.Serializable]
public class TurretUpgradesData : IJsonReadable
{
    public UpgradeData[] upgradesData;
    public int[] costs;
}
 