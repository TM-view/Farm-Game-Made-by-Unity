using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour
{
    private DataCenter dataCenter;
    public Image buttonImage;
    public Sprite[] buttonSprite;
    public SoundSetting soundSetting;
    public void Start()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();
        dataCenter.currentMenu = "Plant";
        if (transform.name == dataCenter.currentMenu)
        {
            buttonImage.sprite = buttonSprite[1];
        }
        else
        {
            buttonImage.sprite = buttonSprite[0];
        }
    }
    public void selected()
    {
        if (dataCenter.gamePause)
        {
            return;
        }
        
        AudioSource audio = GetComponent<AudioSource>();
        audio.volume = soundSetting.soundVolume;
        audio.mute = !soundSetting.sound;
        audio.Play();
        
        if (dataCenter.currentMenu != transform.name)
        {
            GameObject scrollView = GameObject.FindWithTag("Scroll");
            GameObject store = GameObject.FindWithTag("Viewport");
            foreach (Transform child in store.transform)
            {
                if (child.name == transform.name + "Store")
                {
                    dataCenter.currentMenu = transform.name;
                    child.gameObject.SetActive(true);
                    scrollView.GetComponent<ScrollRect>().content = child.GetComponent<RectTransform>();
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
}
