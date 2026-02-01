using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;


public class RelicItem : MonoBehaviour
{
    public SoundSetting soundSetting;
    public GameObject Fog;
    public RelicObject relicObject;
    public Image iconImage;
    public Text nameText;
    public Text priceText;
    public Text descriptionText;
    public float realCostFloat;

    DataCenter dataCenter;
    float relicLevel = 0;

    Transform rlsTransform;

    void Start()
    {
        iconImage.sprite = relicObject.iconSprite;
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();
        
        dataCenter.essence = soundSetting.essence;
        dataCenter.unlockRelicIndex = soundSetting.unlockRelicIndex;
        dataCenter.cornDC = soundSetting.cornDC;
        dataCenter.sunDC = soundSetting.sunDC;
        dataCenter.tomatoDC = soundSetting.tomatoDC;
        dataCenter.doubleTap = soundSetting.doubleTap;
        dataCenter.incCoin = soundSetting.incCoin;
        dataCenter.incEss = soundSetting.incEss;
        
        char c = transform.name[transform.name.Length - 2];
        if (c == 'i' || c == 'e')
        {
            gameObject.SetActive(true);
        }
        else if (c == 't')
        {
            gameObject.SetActive(!dataCenter.unlockRelicIndex.Contains(0));
        }
        else
        {
            gameObject.SetActive(!dataCenter.unlockRelicIndex.Contains(c - '0'));
        }

        rlsTransform = GameObject.FindWithTag("Viewport").transform.Find("RelicStore");
        switch (relicObject.nameString)
        {
            case "CornDC": 
                relicLevel = dataCenter.cornDC; break;
            case "DoubleTap": 
                relicLevel = dataCenter.doubleTap; break;
            case "CoinIncrease": 
                relicLevel = dataCenter.incCoin; break;
            case "EssenceIncrease": 
                relicLevel = dataCenter.incEss; break;
            case "SunflowerDC": 
                relicLevel = dataCenter.sunDC; break;
            case "TomatoDC": 
                relicLevel = dataCenter.tomatoDC; break;
            case "UnlockNewRelic":
                relicLevel = rlsTransform.childCount - 1 - dataCenter.unlockRelicIndex.Length; break;
            case "Reincarnation" : 
                relicLevel = 0; break;
        }
        refresh();
    }

    public void selectRelic()
    {
        if (dataCenter.gamePause)
        {
            return;
        }
        
        dataCenter.currentMenu = "Relic";

        // --- get relicLevel ---
        switch (relicObject.nameString)
        {
            case "CornDC": 
                relicLevel = dataCenter.cornDC; break;
            case "DoubleTap": 
                relicLevel = dataCenter.doubleTap; break;
            case "CoinIncrease": 
                relicLevel = dataCenter.incCoin; break;
            case "EssenceIncrease": 
                relicLevel = dataCenter.incEss; break;
            case "SunflowerDC": 
                relicLevel = dataCenter.sunDC; break;
            case "TomatoDC": 
                relicLevel = dataCenter.tomatoDC; break;
            case "UnlockNewRelic":
                relicLevel = rlsTransform.childCount - 1 - dataCenter.unlockRelicIndex.Length; break;
            case "Reincarnation" : 
                relicLevel = 0; break;
        }

        float price = relicObject.costFloat *
                      (float)Math.Pow(relicObject.costIncreaseFloat, relicLevel);

    
        float balance = dataCenter.essence - price;

        if (balance < 0 || (relicLevel >= relicObject.maxLevelInt && relicObject.nameString != "Reincarnation"))
            return;
        
        // --- apply upgrade ---
        switch (relicObject.nameString)
        {
            case "CornDC": 
                relicLevel = ++dataCenter.cornDC; soundSetting.cornDC = dataCenter.cornDC; break;
            case "DoubleTap": 
                relicLevel = ++dataCenter.doubleTap; soundSetting.doubleTap = dataCenter.doubleTap; break;
            case "CoinIncrease": 
                relicLevel = ++dataCenter.incCoin; soundSetting.incCoin = dataCenter.incCoin; break;
            case "EssenceIncrease": 
                relicLevel = ++dataCenter.incEss; soundSetting.incEss = dataCenter.incEss; break;
            case "SunflowerDC": 
                relicLevel = ++dataCenter.sunDC; soundSetting.sunDC = dataCenter.sunDC; break;
            case "TomatoDC": 
                relicLevel = ++dataCenter.tomatoDC; soundSetting.tomatoDC = dataCenter.tomatoDC; break;

            case "UnlockNewRelic":
                int randIndex = UnityEngine.Random.Range(0, dataCenter.unlockRelicIndex.Length);
                int get = dataCenter.unlockRelicIndex[randIndex]; 

                dataCenter.unlockRelicIndex =
                    dataCenter.unlockRelicIndex.Where((_, i) => i != randIndex).ToArray();

                Transform t = get == 0
                    ? rlsTransform.Find("RelicTemplate")
                    : rlsTransform.Find("RelicTemplate (" + get + ")");

                if (t) t.gameObject.SetActive(true);

                relicLevel = rlsTransform.childCount - 1 - dataCenter.unlockRelicIndex.Length;
                soundSetting.unlockRelicIndex = dataCenter.unlockRelicIndex;
                break;
            case "Reincarnation":
                balance = dataCenter.essenceNextReset + dataCenter.essence;
                StartCoroutine(reincarnation());
                break;
        }

        dataCenter.essence = balance;
        refresh();
    }

    public void refresh()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();

        realCostFloat = relicObject.costFloat *
                         (float)Math.Pow(relicObject.costIncreaseFloat, relicLevel);

        bool isMax = relicLevel >= relicObject.maxLevelInt;

        nameText.text = relicObject.nameString + " Lv" + (isMax ? "Max" : relicLevel.ToString());

        if (isMax && relicObject.nameString != "Reincarnation")
        {
            priceText.text = "";
            descriptionText.text = "";
        }
        else
        {
            float essence = realCostFloat;
            int index = 0;
            const string unit = " abcdefghijklmnopqrstuvwxyz";

            while (essence >= 1000f)
            {
                essence /= 1000f;
                index++;
            }

            priceText.text = "Cost: @" + essence.ToString("F0") + unit[index];
            if (relicObject.maxLevelInt == 0 && relicObject.costFloat == 0f && relicObject.costIncreaseFloat == 0f)
            {
                nameText.text = relicObject.nameString + " LvMax";

            }

            if (relicObject.nameString == "Reincarnation") //Reincarnation
            {
                float curEss = dataCenter.essence;
                int i = 0;
                while (curEss >= 1000f)
                {
                    curEss /= 1000f;
                    i++;
                }
                string curEssString = curEss.ToString("F2") + unit[i];

                float nextEss = dataCenter.essence + dataCenter.essenceNextReset;
                int j = 0;
                while (nextEss >= 1000f)
                {
                    nextEss /= 1000f;
                    j++;
                }
                string nextEssString = nextEss.ToString("F2") + unit[j];

                priceText.text = "curEssence: " + curEssString;
                descriptionText.text = "NextEssence: " + nextEssString;
            }
            else
            {
                float cur = relicObject.baseStatFloat + relicLevel * relicObject.statGrowthFloat;
                float next = relicObject.baseStatFloat + (relicLevel + 1) * relicObject.statGrowthFloat;
                descriptionText.text = relicObject.descriptionString + cur + " -> " + next;
            }
        }

        transform.parent.GetComponent<StoreSetting>()
            .SortStore(transform.parent.name);
    }

    IEnumerator reincarnation()
    {
        Fog.SetActive(true);
        Image fogImage = Fog.GetComponent<Image>();
        Color color = fogImage.color;

        Fog.GetComponent<AudioSource>().Play();

        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 0.5f; 
            fogImage.color = color;
            yield return null;
        }

        yield return null;

        SceneManager.LoadScene("Main");
    }
}
