using System;
using UnityEngine;
using UnityEngine.UI;

public class PlantItem : MonoBehaviour
{
    public PlantObject plantObject;
    public Image plantIcon;
    public Text plantNameText;
    public Text plantHealthText;
    public Text plantBuyText;
    public Text plantSellText;
    public Text DaytoGrowText;
    public float realPrice;
    public float realIncome;
    public RelicObject coinIncrease;
    public RelicObject tomatoDiscount;
    public RelicObject sunflowerDiscount;
    public RelicObject cornDiscount;
    int whatUp = 0;
    float multiplePrice = 1;
    private DataCenter dataCenter;
    void Start()
    {
        refresh();
    }

    public void selectPlant()
    {
        if (dataCenter.gamePause)
        {
            return;
        }
        
        if (dataCenter.selectedPlant != plantObject)
        {
            dataCenter.preveriousText = new Text[] { plantNameText, plantHealthText, plantBuyText, plantSellText, DaytoGrowText };
            dataCenter.currentMenu = "Plant";
            
            dataCenter.selectedPlant = plantObject;

            plantNameText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            plantHealthText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            plantBuyText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            plantSellText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            DaytoGrowText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
        }
        else
        {
            dataCenter.selectedPlant = null;
            setWhiteText();
        }
    }

    void setWhiteText()
    {
        plantNameText.color = Color.white;
        plantHealthText.color = Color.white;
        plantBuyText.color = Color.white;
        plantSellText.color = Color.white;
        DaytoGrowText.color = Color.white;
    }

    public void refresh()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();

        switch (plantObject.plantName)
        {
            case "Corn": 
                            whatUp = dataCenter.cornLvlInc;
                            multiplePrice = cornDiscount.baseStatFloat + (cornDiscount.statGrowthFloat * dataCenter.cornDC);
                            break;
            case "Sunflower":
                            whatUp = dataCenter.sunflowerLvlInc;
                            multiplePrice = sunflowerDiscount.baseStatFloat + (sunflowerDiscount.statGrowthFloat * dataCenter.sunDC);
                            break;
            case "Tomato": 
                            whatUp = dataCenter.tomatoLvlInc;
                            multiplePrice = tomatoDiscount.baseStatFloat + (tomatoDiscount.statGrowthFloat * dataCenter.tomatoDC);
                            break;
        }
        realPrice = plantObject.buy_cost * (float)Math.Pow(plantObject.growthOfBuy, whatUp) * multiplePrice; 
        realIncome = plantObject.sell_cost * (float)Math.Pow(plantObject.growthOfSell, whatUp) * (coinIncrease.baseStatFloat + (coinIncrease.statGrowthFloat * dataCenter.incCoin));

        plantIcon.sprite = plantObject.plantIcon;
        plantNameText.text = plantObject.plantName + " Lv" + (plantObject.level + whatUp).ToString();
        plantHealthText.text = "Health: " + (plantObject.health * (plantObject.level + whatUp)).ToString();
        
        float m = realPrice;
        int v = 0;
        string vs = " abcdefghijklmnopqrstuvwxyz"; 
        while (m >= 1000)
        {
            m /= 1000;
            v++;
        }
        plantBuyText.text = "Buy: $" + m.ToString("F0") + vs[v];

        m = realIncome;
        v = 0;
        while (m >= 1000)
        {
            m /= 1000;
            v++;
        }
        plantSellText.text = "Sell: $" + m.ToString("F0") + vs[v];

        DaytoGrowText.text = "Grow: " + plantObject.timeToGrowPerStateHour.ToString("");
    }
}
