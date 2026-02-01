using UnityEngine;

[CreateAssetMenu(fileName = "PlantObject", menuName = "PlantObj")]
public class PlantObject : ScriptableObject
{
    public string plantName;
    public Sprite[] plantSprite;
    public Sprite plantIcon;
    public float timeToGrowPerStateHour;
    public int health;
    public int level = 1;
    public float buy_cost;
    public float growthOfBuy = 1;
    public float sell_cost;
    public float growthOfSell = 1;
    public int state_grow_loop = -1;
    public int num_loop = 0;
}
