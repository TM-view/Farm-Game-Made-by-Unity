using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Control : MonoBehaviour
{
    public int tutorial_page = 0;
    private string[] tutorial_text_page = {"1. Plants will grow only after the soil is tilled and watered.\n2. Plants will not grow at night.\n3. If you want to till the soil, use a hoe. And if you want to pull out plants, use a shovel.\n4. You can click on a zombie to damage it.", "5. Plants can be eaten by zombies. And if the plant's blood runs out, it will disappear.\n6. Zombies only come out at night.\n7. All zombies will die in the morning.\n8. Zombies get stronger as the days pass.", "9. Fertilizer can help plants grow faster by Eff, which means plants will grow faster to reach that state. CD is how many hours of cooldown. to use fertilizer again It starts counting after the plant is removed and Acc is how much time the plant takes to grow faster.", "10. Tomatoes can be harvested up to 3 times.\n11. If you're having a hard time going further, go to Relic and use Reincarnation."};
    public SoundSetting soundSetting;
    public GameObject description;
    public GameObject Next;
    public GameObject Back;
    AudioSource A;

    void Start()
    {
        changePage();
    }

    public void nextPage()
    {
        tutorial_page++;
        changePage();
    }

    public void backPage()
    {
        tutorial_page--;
        changePage();
    }
    void changePage()
    {
        description.GetComponent<Text>().text = tutorial_text_page[tutorial_page];
        if (tutorial_page == 0)
        {
            Back.SetActive(false);
            A = Next.GetComponent<AudioSource>();
        }
        else if (tutorial_page == tutorial_text_page.Length - 1)
        {
            Next.SetActive(false);
            A = Back.GetComponent<AudioSource>();
        }
        else
        {
            Next.SetActive(true);
            Back.SetActive(true);
            A = Next.GetComponent<AudioSource>();
        }
        
        A.volume = soundSetting.soundVolume;
        A.mute = !soundSetting.sound;
        A.Play();
    }
}
