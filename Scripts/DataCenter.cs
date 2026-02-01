using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataCenter : MonoBehaviour
{
    public GameObject[] zombiePrefab;
    public SoundSetting soundSetting;
    public AudioClip daySound;
    public AudioClip nightSound;
    public Text timeText;
    public GameObject BG_Store;
    public GameObject BG_Setting;
    public GameObject openStore;
    //Upgrade Zone 
    public int autoWater = 0;
    public int autoHavest = 0;
    public int cornLvlInc = 0;
    public int sunflowerLvlInc = 0;
    public int tomatoLvlInc = 0;
    public int fertilizerLvlInc = 0;
    public int tapLvlInc = 0;
    //Relic Zone
    public float essenceNextReset = 0f;
    public int[] unlockRelicIndex;
    public int cornDC = 0;
    public int sunDC = 0;
    public int tomatoDC = 0;
    public int doubleTap = 0;
    public int incCoin = 0;
    public int incEss = 0;

    public bool Store = false;
    public int day;
    public int currentTime = 0;
    public int HourPerDay = 12;
    public float timePerHour = 3f; // xx seconds per in-game hour
    public float _timePerHour;
    public int timeToNight = 8;
    public bool isNight = false;
    public int numZom;
    public bool gamePause = false;

    public float _money = 200.0f;
    public float money
    {
        get
        {
            return _money;
        }
        set
        {
            _money = value;
            Text mn = BG_Store.transform.Find("Money").GetComponent<Text>();
            float m = _money;
            int v = 0;
            string vs = " abcdefghijklmnopqrstuvwxyz"; 
            while (m >= 1000)
            {
                m /= 1000;
                v++;
            }
            mn.text = "$" + m.ToString("F2") + vs[v];
        }
    }
    public float _essence = 0f;
    public float essence
    {
        get
        {
            return _essence;
        }
        set
        {
            _essence = value;
            soundSetting.essence = value;

            Text mn = BG_Store.transform.Find("Essence").gameObject.GetComponent<Text>();
            float m = _essence;
            int v = 0;
            string vs = " abcdefghijklmnopqrstuvwxyz"; 
            while (m >= 1000)
            {
                m /= 1000;
                v++;
            }
            mn.text = "@" + m.ToString("F2") + vs[v];
        }
    }

    public Sprite[] buttonSprite;
    public string _currentMenu = "Plant";
    public string currentMenu
    {
        get
        {
            return _currentMenu;
        }
        set
        {
            _currentMenu = value;
            if (_currentMenu != null)
            {             
                GameObject menu = BG_Store.transform.Find("Menu").gameObject;
                foreach (Transform child in menu.transform)
                {
                    if (_currentMenu == child.name)
                    {
                        child.GetComponent<Image>().sprite = buttonSprite[1];
                    }
                    else
                    {
                        child.GetComponent<Image>().sprite = buttonSprite[0];
                    }
                }
            }

            if (_currentMenu == "Relic")
            {
                BG_Store.transform.Find("Essence").gameObject.SetActive(true);

                Text mn = BG_Store.transform.Find("Essence").gameObject.GetComponent<Text>();
                float m = essence;
                int v = 0;
                string vs = " abcdefghijklmnopqrstuvwxyz"; 
                while (m >= 1000)
                {
                    m /= 1000;
                    v++;
                }
                mn.text = "@" + m.ToString("F2") + vs[v];

                BG_Store.transform.Find("Money")
                .gameObject.SetActive(false);
            }
            else
            {
                BG_Store.transform.Find("Essence")
                .gameObject.SetActive(false);    

                BG_Store.transform.Find("Money")
                .gameObject.SetActive(true);
            }

            GameObject store = GameObject.FindWithTag("Viewport").transform.Find(_currentMenu + "Store").gameObject;
            foreach (Transform child in store.transform)
            {
                switch (_currentMenu)
                {
                    case "Plant": 
                                    child.GetComponent<PlantItem>().refresh();
                                    break;
                    case "Tool": 
                                    child.GetComponent<ToolItem>().refresh();
                                    break;
                    case "Upgrade": 
                                    child.GetComponent<UpgradeItem>().refresh();
                                    break;
                    case "Relic": 
                                    child.GetComponent<RelicItem>().refresh();
                                    break;
                }
            }
            store.GetComponent<StoreSetting>().SortStore(_currentMenu + "Store");
        }
    }
    private Text[] _preveriousText;
    public Text[] preveriousText
    {
        get
        {
            return _preveriousText;
        }
        set
        {
            if (_preveriousText != null)
            {
                foreach (Text txt in _preveriousText)
                {
                    txt.color = Color.white;
                }
            }
            _preveriousText = value;
        }
    }
    public PlantObject _selectedPlant;
    public PlantObject selectedPlant
    {
        get
        {
            return _selectedPlant;
        }
        set
        {
            _selectedPlant = value;
        
            //add other menu
            if (value != null)
            {
                selectedTool = null;
            }
        }
    }
    public ToolObject _selectedTool;
    public ToolObject selectedTool
    {
        get
        {
            return _selectedTool;
        }
        set
        {
            _selectedTool = value;

            //add other menu
            if (value != null)
            {
                selectedPlant = null;
            }
        }
    }
    AudioSource Audio;

    void Start()
    {
        gamePause = false;

        Audio = GetComponent<AudioSource>();
        Audio.clip = !isNight ? daySound : nightSound;
        Audio.volume = daySound ? soundSetting.musicVolume * 0.05f : soundSetting.musicVolume * 0.8f;
        Audio.mute = !soundSetting.music;
        Audio.Play();

        GameObject rls = GameObject.FindWithTag("Viewport").transform.Find("RelicStore")?.gameObject;
        unlockRelicIndex = Enumerable.Range(0, rls.transform.childCount -1).ToArray();

        _timePerHour = timePerHour;
        string sunset = !isNight ? "Day : " : "Night : " ;
        timeText = GameObject.FindWithTag("Time").GetComponent<Text>();
        timeText.text = "DAY " + day + "\n" + sunset + (currentTime * 2) + "AM";
        
        Text mn = GameObject.FindWithTag("Money").GetComponent<Text>();
        float m = money;
        int v = 0;
        string vs = " abcdefghijklmnopqrstuvwxyz"; 
        while (m >= 1000)
        {
            m /= 1000;
            v++;
        }
        mn.text = "$" + m.ToString("F2") + vs[v];
        
        BG_Store.gameObject.SetActive(Store);
    }
    public void TogglePause()
    {
        gamePause = !gamePause;

        if (Audio.isPlaying)
        {
            Audio.Pause();  
        }
        else
        {
            Audio.Play();
        }

        GameObject pauseMenu = GameObject.FindWithTag("Canvas").transform.Find("PauseMenu").gameObject;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    public void ToggleStore()
    {
        if (gamePause)
        {
            return;
        }

        AudioSource A = openStore.GetComponent<AudioSource>();
        A.volume = soundSetting.soundVolume * 0.05f;
        A.mute = !soundSetting.sound;
        A.Play();

        bool active = BG_Store.gameObject.activeSelf;
        BG_Store.gameObject.SetActive(!active);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (BG_Setting.activeSelf)
            {
                BG_Setting.transform.Find("setting_exit").GetComponent<StartMenu>().SelectButton();
            }
            else
            {
                TogglePause();
            }
        }

        Audio.volume = isNight ? soundSetting.musicVolume * 0.2f : soundSetting.musicVolume * 0.03f;
        Audio.mute = !soundSetting.music;

        if (gamePause)
        {
            return;    
        }

        if (_timePerHour > 0)
        {
            _timePerHour -= Time.deltaTime;
        }
        else
        {
            //DayCycle
            _timePerHour = timePerHour;
            currentTime++;
            
            if (currentTime > HourPerDay)
            {
                currentTime = 0;
                numZom = 0;
                isNight = false;
                day++;

                Audio.volume = soundSetting.musicVolume * 0.03f;
                Audio.clip = daySound;
                Audio.Play();
            }
            else if (currentTime == timeToNight + 1)
            {
                isNight = true;
                GameObject[] farms = GameObject.FindGameObjectsWithTag("Farm");
                int countWaterfarm = farms.Count(f =>
                {
                    var sr = f.GetComponent<SpriteRenderer>();
                    return sr != null && sr.sprite != null && sr.sprite.name == "Land_2";
                });
                numZom = 1 + (day / 3) + (countWaterfarm / 2);
                Audio.volume = soundSetting.musicVolume * 0.2f;
                Audio.clip = nightSound;
                Audio.Play();
            }

            GameObject.FindWithTag("Moon").GetComponent<MoonMove>().Move(currentTime);

            //Manager Time
            string sunset = !isNight ? "Day : " : "Night : " ;
            if (currentTime > 6)
            {
                timeText.text = "DAY " + day + "\n" + (sunset + (currentTime - 6) * 2).ToString() + "PM";    
            }
            else
            {
                timeText.text = "DAY " + day + "\n" + (sunset + currentTime * 2).ToString() + "AM";
            }

            //Manager PlantGrowth
            GameObject map = GameObject.FindWithTag("Tilemap");
            foreach (Transform child in map.transform)
            {
                if (child.tag == "FarmLand")
                {
                    LandManager land = child.GetComponent<LandManager>();
                    if (land.currentLandSprite == land.landSprite[2] && land.isPlanted 
                        && land.plantState != land.plantOnLand.plantSprite.Length - 1 && !isNight) // Land with water
                        {
                            land.Growth();
                        }
                    
                    // LandRestore
                    if (!land.hasFertilizer && land.fertilizerStat[1] > 0)
                    {
                        land.fertilizerStat[1]--;
                    }
                }
            }

            //zombie attack
            int numZomInWave = (numZom / 4) > 0 ? (int)(numZom / 4) : 1 ;
            if (isNight && numZom > 0)
            {
                //summon Zombie
                for (int i = 0; i < numZomInWave; i++)
                {
                    if (numZomInWave <= 0)
                    {
                        break;
                    }
                    
                    GameObject[] zomLand = GameObject.FindGameObjectsWithTag("LandOut");
                    GameObject spawnZom = zomLand[Random.Range(0, zomLand.Length)];
                    GameObject targetZom = zomLand.SingleOrDefault(o =>
                        o.name.EndsWith(spawnZom.name[^2..]) &&
                        o.name != spawnZom.name
                    );    

                    Quaternion rot = Quaternion.identity;

                    if (spawnZom.name.Contains("right"))
                    {
                        rot = Quaternion.Euler(0, 180, 0);
                    }

                    GameObject zombie = Instantiate(zombiePrefab[Random.Range(0,3)], spawnZom.transform.position, rot);
                    zombie.GetComponent<ZombieSetting>().currentPos = spawnZom.transform.position;
                    zombie.GetComponent<ZombieSetting>().targetPos = targetZom.transform.position;

                    zombie.GetComponent<ZombieSetting>().aspd = 5f - (0.02f * day);
                    zombie.GetComponent<ZombieSetting>().atk = 1 + (int)(day / 5);
                    zombie.GetComponent<ZombieSetting>().health = 3 + (int)(day / 5);
                    zombie.GetComponent<ZombieSetting>().speed = 0.55f + (0.05f * (day / 10));
                    numZom--;
                }
            }
        
        }
    }
}
