using UnityEngine;

[CreateAssetMenu(fileName = "ToolObject", menuName = "ToolObj")]
public class ToolObject : ScriptableObject
{
    public Sprite toolIcon;
    public string toolName;
    public int price;
    public float growthOfPrice = 1;
    public int level = 1;
    public int TopRightInt;
    public int ButtomRightInt;
    public int ButtomLeftInt;
}
