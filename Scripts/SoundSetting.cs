using UnityEngine;

[CreateAssetMenu(fileName = "SoundSetting", menuName = "soundSetting")]
public class SoundSetting : ScriptableObject
{
    //sound
    public float soundVolume = 1f;
    public float musicVolume = 1f;
    public bool music = true;
    public bool sound = true;

    //relic && essence
    public float essence = 0f;
    public int[] unlockRelicIndex;
    public int cornDC = 0;
    public int sunDC = 0;
    public int tomatoDC = 0;
    public int doubleTap = 0;
    public int incCoin = 0;
    public int incEss = 0;
}
