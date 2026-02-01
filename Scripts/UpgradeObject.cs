using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeObject", menuName = "UpgradeObj")]
public class UpgradeObject : ScriptableObject
{
    public Sprite Icon;
    public string Name;
    public int Level = 1;
    public int MaxLevel;
    public float Price;
    public float GrowthRateMultiplePriceFloat;
    public string WhatUpgrade;
    public float BaseStat;
    public float GrowthRateStat;
}
