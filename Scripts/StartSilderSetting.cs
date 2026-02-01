using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class StartSilderSetting : MonoBehaviour
{
    public SoundSetting soundSetting;
    public Slider targetSillder;
    public Text targetText;
    private DataCenter dataCenter;
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Start")
        {
            dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();
        }

        float value = transform.name == "volumeSlider" ? soundSetting.soundVolume : soundSetting.musicVolume;
        targetText.text = (value * 100).ToString("F0") + "%";
    }

    public void ChangeTigger()
    {
        float value = targetSillder.GetComponent<Slider>().value;
        targetText.text = (value * 100).ToString("F0") + "%";
        if (transform.name == "volumeSlider")
        {
            soundSetting.soundVolume = value;
        }
        else if (transform.name == "musicSlider")
        {
            soundSetting.musicVolume = value;
        }

        if (SceneManager.GetActiveScene().name != "Start")
        {
            if (dataCenter.isNight)
            {
                dataCenter.GetComponent<AudioSource>().volume = soundSetting.musicVolume * 0.8f;
            }
            else
            {
                dataCenter.GetComponent<AudioSource>().volume = soundSetting.musicVolume * 0.05f;
            }
        }
        else
        {
            GameObject panel = GameObject.FindWithTag("StartPanel");
            panel.transform.GetComponent<AudioSource>().volume = soundSetting
            .musicVolume * 0.05f;

            foreach (Transform child in panel.transform)
            {
                if (child.tag == "StartButton")
                {
                    child.GetComponent<AudioSource>().volume = soundSetting.soundVolume;
                }
            }
        }

    }
}
