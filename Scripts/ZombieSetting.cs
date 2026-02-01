using UnityEngine;
using System.Collections;


public class ZombieSetting : MonoBehaviour
{
    public Animator anim;
    private AudioSource Audio;
    public Vector3 currentPos;
    public Vector3 targetPos;
    public GameObject forCollider;
    LandManager farm;
    bool attacking = false;
    bool alive = true;
    public float speed = 0.55f;
    public int health = 3;
    public float aspd = 5f; // 1 atack / x sec
    private float _aspd;
    public int atk = 1;
    DataCenter dataCenter;
    public RelicObject doubleTap;
    public UpgradeObject tapTap;
    public RelicObject coinIncrease;
    public RelicObject essenceIncrease;
    public AudioClip zombieDie;
    public AudioClip zombieTaken;
    public AudioClip zombieEat;
    public SoundSetting soundSetting;
    public GameObject coin;
    public GameObject punch;
    void Start()
    {
        dataCenter = GameObject.FindWithTag("DataCenter").GetComponent<DataCenter>();
        anim = GetComponent<Animator>();
        Audio = GetComponent<AudioSource>();

        _aspd = aspd;
        _aspd = 1;
    }

    public void zombieCollider(Collider2D collision)
    {
        if (collision.tag == "Farm")
        {
            attacking = true;
            anim.SetBool("attacking", attacking);
            farm = collision.transform.parent.GetComponent<LandManager>();
        }
    }

    public void zombieClick()
    {
        bool doubleTap = Random.Range(1, 100) <= (int)dataCenter.doubleTap;
        StartCoroutine(Taken());
        int damage = doubleTap ? 2 * (dataCenter.tapLvlInc + 1) : dataCenter.tapLvlInc + 1;
        health -= damage;  
        if (alive && health <= 0)
        {
            StartCoroutine(DropCoin());
            dataCenter.money += 1 + dataCenter.day * dataCenter.day * (coinIncrease.baseStatFloat + (coinIncrease.statGrowthFloat * dataCenter.incCoin));
            dataCenter.essenceNextReset += (dataCenter.day / 10) * (essenceIncrease.baseStatFloat + (essenceIncrease.statGrowthFloat * dataCenter.incEss));
            StartCoroutine(DieAndDestroy());
        }
    }

    IEnumerator DropCoin()
    {
        GameObject C = Instantiate(coin, transform.position + new Vector3(0, 2, 0), Quaternion.identity);
        yield return new WaitForSeconds(2f); 
        Destroy(C);
    }

    IEnumerator DieAndDestroy()
    {
        Audio.clip = zombieDie;
        Audio.volume = soundSetting.soundVolume * 0.1f;
        Audio.mute = !soundSetting.sound;
        Audio.Play();
        alive = false;
        attacking = false;
        anim.SetBool("alive", false);
        yield return new WaitForSeconds(2f); // เวลา anim

        Destroy(gameObject);
    }

    IEnumerator Taken()
    {
        Audio.volume = soundSetting.soundVolume * 0.3f;
        Audio.clip = zombieTaken;
        Audio.mute = !soundSetting.sound;
        Audio.Play();

        Collider2D col = GetComponent<Collider2D>();
        Bounds b = col.bounds;

        Vector3 randomPos = new Vector3(
            Random.Range(b.min.x, b.max.x),
            Random.Range(b.min.y, b.max.y),
            transform.position.z
        );

        GameObject P = Instantiate(punch, randomPos, Quaternion.identity);

        GetComponent<SpriteRenderer>().color = new Color32(0xFF, 0x85, 0x85, 0xFF);
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return new WaitForSeconds(0.1f);
        
        Destroy(P);
    }

    void Update()
    {
        //dead
        if (alive && (transform.position == targetPos || !dataCenter.isNight))
        {
            StartCoroutine(DieAndDestroy());
        }

        //attack
        if (!attacking && alive)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );
        }
        else if (attacking && alive)
        {
            if (_aspd > 0)
            {
                _aspd -= Time.deltaTime;
            }
            else
            {
                AudioSource fc = forCollider.GetComponent<AudioSource>();
                fc.volume = soundSetting.soundVolume * 0.3f;
                fc.clip = zombieEat;
                fc.mute = !soundSetting.sound;
                fc.Play();
                _aspd = aspd;
                farm.health -= atk;
                farm.StartCoroutine(farm.PlantEating());

                if (farm.health <= 0 && attacking)
                {
                    attacking = false;
                    farm.ZombieAttack();
                    anim.SetBool("attacking", attacking);
                }
            }
        }
    }
}
