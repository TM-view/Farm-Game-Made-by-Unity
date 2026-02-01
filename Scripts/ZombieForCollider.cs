using UnityEngine;

public class ZombieForCollider : MonoBehaviour
{
    public GameObject zombieObj;
    void OnTriggerEnter2D(Collider2D collision)
    {
        zombieObj.GetComponent<ZombieSetting>().zombieCollider(collision);
    }
}
