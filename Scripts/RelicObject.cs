using UnityEngine;

[CreateAssetMenu(fileName = "RelicObject", menuName = "RelicObj")]
public class RelicObject : ScriptableObject
{
    public Sprite iconSprite;
    public string nameString;
    public int maxLevelInt;
    public float costFloat;
    public float costIncreaseFloat;
    public string descriptionString;
    public float baseStatFloat = 1;
    public float statGrowthFloat;
}
