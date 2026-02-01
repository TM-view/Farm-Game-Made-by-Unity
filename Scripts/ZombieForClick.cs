using UnityEngine;

public class ZombieForClick : MonoBehaviour
{
    void OnMouseDown()
    {
        GetComponent<ZombieSetting>().zombieClick();
    }
}
