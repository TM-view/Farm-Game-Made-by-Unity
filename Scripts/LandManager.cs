using System;
using UnityEngine;
using System.Collections;

public class LandManager : MonoBehaviour
{
    public Sprite[] landSprite = new Sprite[3];
    public Sprite[] ImgPlant = new Sprite[3];
    public GameObject ImagePlant;
    public Sprite currentLandSprite;
    public bool isPlanted = false;
    public int plantState = 0;
    public int growTimeCounter = 0;
    public int loop = 0;
    public int health = 1;
    public bool hasFertilizer = false;
    public int[] fertilizerStat = new int[3];
    int whatUp = 0;
    float multiplePrice = 1;
    public PlantObject plantOnLand;
    private DataCenter dataCenter;
    //Relic Zone
    public RelicObject coinIncrease;
    public RelicObject essenceIncrease;
    public RelicObject tomatoDiscount;
    public RelicObject sunflowerDiscount;
    public RelicObject cornDiscount;
    public SoundSetting soundSetting;
    AudioSource Audio;
    public AudioClip hoeing;
    public AudioClip watering;
    public AudioClip fertilizing;
    public AudioClip shoveling;
    public AudioClip planting;
    public AudioClip growup;
    public AudioClip havesting;
    
    void Start()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();
        currentLandSprite = transform.GetComponent<SpriteRenderer>().sprite = landSprite[0];
        Audio = GetComponent<AudioSource>();
    }

    void OnMouseDown()
    {
        if (isPlanted && plantState == plantOnLand.plantSprite.Length - 1)
        {
            Havest();
        }

        //in Plant Menu
        else if (dataCenter.currentMenu == "Plant" && dataCenter.selectedPlant != null)
        {
            implant();
        }

        //in Tool Menu
        else if (dataCenter.currentMenu == "Tool" && dataCenter.selectedTool != null)
        {
            useTool();
        }
    }

    void refresh(string name)
    {
        switch (name)
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
        
    }
    
    void PlayEffect(AudioClip wantPlay)
    {
        Audio.clip = wantPlay;
        Audio.volume = soundSetting.soundVolume;
        Audio.mute = !soundSetting.sound;
        Audio.Play();
    }
    public void Havest()
    {
        PlayEffect(havesting);

        transform.GetChild(1).gameObject.SetActive(true); //Coin
        if (loop < 1)
        {
            Pluck();
        }
        else
        {
            plantState = plantOnLand.state_grow_loop;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = plantOnLand.plantSprite[plantState];
            loop--;
        }
        refresh(plantOnLand.name);
        dataCenter.money += plantOnLand.sell_cost * (float)Math.Pow(plantOnLand.growthOfSell, whatUp) * (coinIncrease.baseStatFloat + (coinIncrease.statGrowthFloat * dataCenter.incCoin));
        dataCenter.essenceNextReset += whatUp * (essenceIncrease.baseStatFloat + (essenceIncrease.statGrowthFloat * dataCenter.incEss));
    }

    void implant()
    {
        // Planting
        if (!isPlanted && currentLandSprite != landSprite[0]) 
        {
            if (CanBuy())
            {
                PlayEffect(planting);

                if (currentLandSprite == landSprite[1])
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = dataCenter.selectedPlant.plantSprite[0]; 
                    plantState = 0;
                }
                else if (currentLandSprite == landSprite[2])
                {
                    transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = dataCenter.selectedPlant.plantSprite[1]; 
                    plantState = 1;
                }
                plantOnLand = dataCenter.selectedPlant;

                transform.GetChild(0).gameObject.SetActive(true);
                isPlanted = true;
                loop = plantOnLand.num_loop;
                
                refresh(plantOnLand.name);
                health = plantOnLand.health * (whatUp + 1);

                string[] names = { "Corn", "Sunflower", "Tomato" };
                int index = Array.IndexOf(names, plantOnLand.name);
                ImagePlant.GetComponent<SpriteRenderer>().sprite = ImgPlant[index];
                ImagePlant.SetActive(true);
            }
        }
    }

    void useTool()
    {
        string toolName = dataCenter.selectedTool.toolName;
        if (toolName == "Water")
        {
            if (currentLandSprite == landSprite[1] && plantState == 0) // Land with soil
            {
                if (CanBuy())
                {
                    PlayEffect(watering);

                    currentLandSprite = transform.GetComponent<SpriteRenderer>().sprite = landSprite[2]; // Land with water
                    if (isPlanted)
                    {
                        plantState = 1;
                        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = plantOnLand.plantSprite[1]; // Update plant sprite
                    }
                }
            }
        }
        else if (toolName == "Hoe")
        {
            if (currentLandSprite == landSprite[0]) // Land with grass
            {
                if (CanBuy())
                {
                    PlayEffect(hoeing);

                    currentLandSprite = transform.GetComponent<SpriteRenderer>().sprite = landSprite[1 + dataCenter.autoWater]; // Land with soil
                }
            }
        }
        else if (toolName == "Shovel")
        {
            if (plantOnLand != null)
            {
                if (transform.GetChild(0).GetComponent<SpriteRenderer>().sprite != plantOnLand.plantSprite[0])
                {
                    if (CanBuy())
                    {
                        PlayEffect(shoveling);

                        Pluck();
                    }
                }
            }
        }
        else if (toolName == "Fertilizer")
        {
            if (!hasFertilizer && fertilizerStat[1] == 0 && transform.GetComponent<SpriteRenderer>().sprite != landSprite[0])
            {
                if (CanBuy())
                {
                    PlayEffect(fertilizing);

                    hasFertilizer = true;
                    fertilizerStat[0] = dataCenter.selectedTool.TopRightInt + (int)(dataCenter.fertilizerLvlInc / 4f); //Eff
                    fertilizerStat[1] = dataCenter.selectedTool.ButtomRightInt - dataCenter.fertilizerLvlInc; //CD
                    fertilizerStat[2] = dataCenter.selectedTool.ButtomLeftInt + (int)(dataCenter.fertilizerLvlInc / 5f); //Acc
                }
            }
        }
        // Add other tools here
    }

    void Pluck()
    {
        ImagePlant.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = plantOnLand.plantSprite[dataCenter.autoWater];
        currentLandSprite = transform.GetComponent<SpriteRenderer>().sprite = landSprite[1 + dataCenter.autoWater];
        plantState = 0;
        isPlanted = false;
        hasFertilizer = false;
        fertilizerStat[0] = 0;
        fertilizerStat[2] = 0;
    }

    bool CanBuy()
    {
        float price = 0f;

        if (dataCenter.currentMenu == "Plant")
        {
            refresh(dataCenter.selectedPlant.name);
            price = dataCenter.selectedPlant.buy_cost * (float)Math.Pow(dataCenter.selectedPlant.growthOfBuy, whatUp) * multiplePrice; 
        }
        else if (dataCenter.currentMenu == "Tool")
        {
            price = dataCenter.selectedTool.name != "Fertilizer" 
            ? dataCenter.selectedTool.price 
            : dataCenter.selectedTool.price * (float)Math.Pow(dataCenter.selectedTool.growthOfPrice, dataCenter.fertilizerLvlInc);
        }

        float balance = dataCenter.money - price;
        
        if (balance >= 0f)
        {
            dataCenter.money = balance;
            dataCenter.essenceNextReset += (price / 100) * (essenceIncrease.baseStatFloat + (essenceIncrease.statGrowthFloat * dataCenter.incEss));
            GameObject.FindWithTag("Store").GetComponent<StoreSetting>().SortStore(dataCenter.currentMenu + "Store");
            return true;
        }

        return false;
    }

    public void Growth()
    {
        growTimeCounter++;
        Debug.Log("growTimeCounter: " + growTimeCounter);
        float realGrowTime = plantState <= fertilizerStat[0] && hasFertilizer ? plantOnLand.timeToGrowPerStateHour - fertilizerStat[2] : plantOnLand.timeToGrowPerStateHour ;    

        if (growTimeCounter == realGrowTime)
        {
            StartCoroutine(ShowGrow());
            growTimeCounter = 0;
            plantState++;
            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = plantOnLand.plantSprite[plantState];
            
            if (plantState == 2)
            {
                ImagePlant.SetActive(false);
            }

            if (plantState == plantOnLand.plantSprite.Length - 1)
            {
                string farmName = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite.name;
                Transform farmTrans = transform.GetChild(0).gameObject.transform;
                farmTrans.Find(farmName + "_Collider").gameObject.SetActive(true);

                if (dataCenter.autoHavest > 0)
                {
                    Havest();
                }
            }
        }
    }

    IEnumerator ShowGrow()
    {
        Audio.clip = growup;
        Audio.volume = soundSetting.soundVolume * 0.05f;
        Audio.mute = !soundSetting.sound;
        Audio.Play();
        transform.GetChild(2).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f); 
        transform.GetChild(2).gameObject.SetActive(false);
    }

    void OnMouseEnter() // non-sense condition 
    {
        if (
            (dataCenter.currentMenu == "Plant"
                && dataCenter.selectedPlant != null
                && transform.GetChild(0).gameObject.activeSelf)
            || (dataCenter.currentMenu == "Tool"
                && dataCenter.selectedTool != null
                && (
                    (dataCenter.selectedTool.toolName == "Fertilizer"
                        && (hasFertilizer
                            || fertilizerStat[1] != 0
                            || transform.GetComponent<SpriteRenderer>().sprite == landSprite[0]))
                            || dataCenter.money - (dataCenter.selectedTool.price * (float)Math.Pow(dataCenter.selectedTool.growthOfPrice, dataCenter.fertilizerLvlInc)) < 0

                    || (dataCenter.selectedTool.toolName == "Water"
                        && transform.GetComponent<SpriteRenderer>().sprite == landSprite[2])
                    || (dataCenter.selectedTool.toolName == "Shovel" 
                        && (transform.GetComponent<SpriteRenderer>().sprite == landSprite[0]
                        || plantOnLand == null
                        || dataCenter.money - dataCenter.selectedTool.price < 0))
                    || (dataCenter.selectedTool.toolName == "Hoe")
                        && transform.GetComponent<SpriteRenderer>().sprite != landSprite[0]
                        || dataCenter.money - dataCenter.selectedTool.price < 0
                )
            )
        )
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0x85, 0x85, 0xFF);
            transform.GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0x85, 0x85, 0xFF);
        }
        else
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0x6C, 0xFF, 0x8E, 0xFF);
            transform.GetComponent<SpriteRenderer>().color = new Color32(0x6C, 0xFF, 0x8E, 0xFF);
        }
    }

    void OnMouseExit()
    {
        if (!(transform.GetComponent<SpriteRenderer>().color == Color.white))
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
            transform.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void ZombieAttack()
    {
        Pluck();
    }

    public IEnumerator PlantEating()
    {
        transform.GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0x85, 0x85, 0xFF);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0x85, 0x85, 0xFF);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
