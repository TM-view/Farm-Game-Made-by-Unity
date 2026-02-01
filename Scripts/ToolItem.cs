using System;
using UnityEngine;
using UnityEngine.UI; 

public class ToolItem : MonoBehaviour
{
    public ToolObject toolObject;
    public Image toolIcon;
    public Text toolNameText;
    public Text priceText;
    public Text ButtomRightText;
    public Text ButtomLeftText;
    public Text TopRightText;
    public float realPrice;
    private DataCenter dataCenter;
    void Start()
    {
        refresh();
    }

    public void selectTool()
    {
        if (dataCenter.gamePause)
        {
            return;
        }
        
        if (dataCenter.selectedTool != toolObject)
        {
            dataCenter.preveriousText = new Text[] { toolNameText, priceText, ButtomRightText, ButtomLeftText, TopRightText };
            dataCenter.currentMenu = "Tool";
            
            dataCenter.selectedTool = toolObject;

            toolNameText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            priceText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            ButtomRightText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            ButtomLeftText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
            TopRightText.color = new Color32(0x3F, 0xFF, 0x00, 0xFF);
        }
        else
        {
            setWhiteText();
            dataCenter.selectedTool = null;
        }
    }

    private void setWhiteText()
    {
        // ToolObject preveiousTool = dataCenter.selectedTool;
        toolNameText.color = Color.white;
        priceText.color = Color.white;
        ButtomRightText.color = Color.white;
        ButtomLeftText.color = Color.white;
        TopRightText.color = Color.white;
    }

    public void refresh()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();

        toolIcon.sprite = toolObject.toolIcon;
        string lv = toolObject.toolName != "Fertilizer" 
                    ? "Max" : (toolObject.level + dataCenter.fertilizerLvlInc).ToString();
        toolNameText.text = toolObject.toolName + " Lv" + lv;
        
        realPrice = lv == "Max" ? toolObject.price : toolObject.price * (float)Math.Pow(toolObject.growthOfPrice, dataCenter.fertilizerLvlInc);
        float m = realPrice;
        int v = 0;
        string vs = " abcdefghijklmnopqrstuvwxyz"; 
        while (m >= 1000)
        {
            m /= 1000;
            v++;
        }
        priceText.text = "Buy: $" + m.ToString("F0") + vs[v];

        if (toolObject.toolName == "Fertilizer")
        {
            TopRightText.text = "Eff: " + (toolObject.TopRightInt + (int)(dataCenter.fertilizerLvlInc / 4f)).ToString(); //Max 4
            ButtomRightText.text = "CD :" + (toolObject.ButtomRightInt - dataCenter.fertilizerLvlInc).ToString(); //Max 0
            ButtomLeftText.text = "Acc: " + (toolObject.ButtomLeftInt + (int)(dataCenter.fertilizerLvlInc / 5f)).ToString(); //Max 4
        }
        else
        {
            TopRightText.gameObject.SetActive(false);
            ButtomRightText.gameObject.SetActive(false);
            ButtomLeftText.gameObject.SetActive(false);
        }
    }
}
