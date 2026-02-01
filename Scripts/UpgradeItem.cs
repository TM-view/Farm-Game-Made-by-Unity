using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeItem : MonoBehaviour
{
    DataCenter dataCenter;
    public RelicObject essenceIncrease;
    public UpgradeObject upgradeObject;
    public Image upgradeIcon;
    public Text upgradeNameText;
    public Text priceText;
    public Text whatUpgradeText;
    public float realPrice;
    private int whatUp = 0; 
    void Start()
    {
        refresh();
    }

    public void selectUpgrade()
    {
        if (dataCenter.gamePause)
        {
            return;
        }
        
        dataCenter.currentMenu = "Upgrade";
        float price = upgradeObject.Price * (float)Math.Pow(upgradeObject.GrowthRateMultiplePriceFloat, whatUp);
        float balance = dataCenter.money - price;

        int upgradeLv = 0;
        switch (upgradeObject.Name)
        {
            case "AutoHavest" : 
                                    upgradeLv = dataCenter.autoHavest;
                                    break;
            case "AutoWater" :
                                    upgradeLv = dataCenter.autoWater;
                                    break;
            case "CornUpgrade" :
                                    upgradeLv = dataCenter.cornLvlInc;
                                    break;
            case "SunflowerUpgrade" :
                                    upgradeLv = dataCenter.sunflowerLvlInc;
                                    break;
            case "TomatoUpgrade" :
                                    upgradeLv = dataCenter.tomatoLvlInc;
                                    break;
            case "FertilizerUpgrade" :
                                    upgradeLv = dataCenter.fertilizerLvlInc;
                                    break;
            case "Tap" :
                                    upgradeLv = dataCenter.tapLvlInc;
                                    break;         
        }

        if (balance >= 0 && upgradeLv != upgradeObject.MaxLevel)
        {
            switch (upgradeObject.Name)
            {
                case "AutoHavest" : 
                                        dataCenter.autoHavest++;
                                        whatUp = dataCenter.autoHavest;
                                        break;
                case "AutoWater" :
                                        dataCenter.autoWater++;
                                        whatUp = dataCenter.autoWater;
                                        break;
                case "CornUpgrade" :
                                        dataCenter.cornLvlInc++;
                                        whatUp = dataCenter.cornLvlInc;
                                        break;
                case "SunflowerUpgrade" :
                                        dataCenter.sunflowerLvlInc++;
                                        whatUp = dataCenter.sunflowerLvlInc;
                                        break;
                case "TomatoUpgrade" :
                                        dataCenter.tomatoLvlInc++;
                                        whatUp = dataCenter.tomatoLvlInc;
                                        break;
                case "FertilizerUpgrade" :
                                        dataCenter.fertilizerLvlInc++;
                                        whatUp = dataCenter.fertilizerLvlInc;
                                        break;
                case "Tap" :
                                        dataCenter.tapLvlInc++;
                                        whatUp = dataCenter.tapLvlInc;
                                        break;         
            }
            dataCenter.money = balance;
            dataCenter.essenceNextReset += (price / 100) * (essenceIncrease.baseStatFloat + (essenceIncrease.statGrowthFloat * dataCenter.incEss));
            refresh();
        }
    }

    public void refresh()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();

        realPrice = upgradeObject.Price * (float)Math.Pow(upgradeObject.GrowthRateMultiplePriceFloat, whatUp);
        
        upgradeIcon.sprite = upgradeObject.Icon;
        string lv = upgradeObject.MaxLevel == upgradeObject.Level + whatUp 
                    ? "Max" : (upgradeObject.Level + whatUp).ToString() ;
        upgradeNameText.text = upgradeObject.Name + " Lv" + lv; 
        
        if (lv != "Max")
        {
            float m = realPrice;
            int v = 0;
            string vs = " abcdefghijklmnopqrstuvwxyz"; 
            while (m >= 1000)
            {
                m /= 1000;
                v++;
            }
            priceText.text = "UpCost: $" + m.ToString("F0") + vs[v];

            string s =  upgradeObject.BaseStat == 0f && upgradeObject.GrowthRateStat == 0f 
                        ? "" : upgradeObject.BaseStat + whatUp + " -> " + (upgradeObject.BaseStat + upgradeObject.GrowthRateStat + whatUp);
            whatUpgradeText.text = upgradeObject.WhatUpgrade + s;
        }
        else
        {
            priceText.text = "";
            whatUpgradeText.text = "";
        }
        
        transform.parent.GetComponent<StoreSetting>().SortStore(transform.parent.name);

    }
}
