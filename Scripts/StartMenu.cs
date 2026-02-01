using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;

public class StartMenu : MonoBehaviour
{
    public SoundSetting soundSetting;
    public GameObject Tutorial_UI;
    void Start()
    {
        if ((!soundSetting.music && transform.name == "Music") || (!soundSetting.sound && transform.name == "Sound"))
        {
            GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }
    }

    IEnumerator WaitPlayAndDoAction()
    {
        AudioSource audio = GetComponent<AudioSource>();
        audio.PlayOneShot(audio.clip);

        yield return new WaitForSeconds(0.28f);

        switch (transform.name)
        {
            case "NewGame":
                SceneManager.LoadScene("Main");
                break;

            case "Setting":
                GameObject.FindWithTag("StartPanel")
                    .transform.Find("BG_Setting")
                    .gameObject.SetActive(true);
                break;

            case "setting_exit":
                transform.parent.gameObject.SetActive(false);
                break;

            case "Exit":
                Application.Quit();
                break;

            case "Tutorial":
                GameObject.FindWithTag("StartPanel")
                    .transform.Find("Tutorial_UI")
                    .gameObject.SetActive(true);
                break;

            case "tutorial_exit":
                transform.parent.gameObject.SetActive(false);
                break;

            case "Sound":
                toggle(transform.name);
                switchColor();
                break;

            case "Music":
                toggle(transform.name);
                switchColor();
                break;

            case "Resume":
                DataCenter dataCenter =
                    GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();
                dataCenter.TogglePause();
                break;

            case "Quit":
                SceneManager.LoadScene("Start");
                break;
        }
    }
    public void SelectButton()
    {
        StartCoroutine(WaitPlayAndDoAction()); 
        
    }

    void switchColor()
    {
        if (GetComponent<UnityEngine.UI.Image>().color == Color.red)
        {
            // sound open
            GetComponent<UnityEngine.UI.Image>().color = Color.white;
        }
        else
        {
            // sound close
            GetComponent<UnityEngine.UI.Image>().color = Color.red;
        }
    }

    void toggle(string target)
    {
        GameObject panel = GameObject.FindWithTag("StartPanel");
        AudioSource audio = panel.transform.GetComponent<AudioSource>();
        if (target == "Sound")
        {
            soundSetting.sound = !soundSetting.sound;
            foreach (Transform child in panel.transform)
            {
                if (child.tag == "StartButton")
                {
                    child.GetComponent<AudioSource>().mute = !child.GetComponent<AudioSource>().mute;
                }
            }
        }
        else if (target == "Music")
        {
            soundSetting.music = !soundSetting.music;
            audio.mute = !audio.mute;
        }
    }
}
