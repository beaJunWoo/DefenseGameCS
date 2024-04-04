using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

public class SR_Enemy : MonoBehaviour
{
    [Range(0f, 360f)][SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 1f;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;

    [SerializeField] LayerMask BlockedTargetMask;
    [SerializeField] LayerMask BlockedObstacleMask;

    LayerMask NowTargetMask;
    LayerMask NowObstacleMask;

    List<Collider> hitTargetList = new List<Collider>();
    Vector3 minPos;
    GameObject Target;
    bool FindPlayer = false;

    Animator ani;
    SR_ObjPooling SR_objPooling;
    Transform target;
    public Transform BulletSpawnPos;
    NavMeshAgent agent;
    Rigidbody rigid;
    SR_FinshGame SR_finshGame;
    SR_Coin SR_coion;
    public enum EnemyType { Gun, Rifle, Bomb, Car }
    public EnemyType enemyType;

    [Header("Releading")]
    float reloadPosY = 4.3f;
    public GameObject reload;
    public Transform reloadImg;
    [SerializeField] int max_MagazineCapacity;
    [SerializeField] int now_MagazineCapacity = 0;

    [Header("HpBar")]
    public GameObject HpBar;
    public Slider Slider_Hp;
    public float maxHp;
    protected float hpbarPosY = 3.5f;

    [Header("RotationBody")]
    public Transform Body;
    public float rotationSpeed;

    [Header("AttackedTxt")]
    public GameObject AttackedTxt;
    public float damgePosY;

    [SerializeField]
    float hp = 0;
    float bulletDamage;

    bool is_Active = true;
    public bool DebugMode = false;

    [Header("BombSetting")]
    public GameObject Bomb;
    public AudioSource audioClip;

    [Header("CarSetting")]
    public GameObject[] Tire;
    public GameObject DestoryEffect;
    public GameObject Car;
    public GameObject DestoryCar;
    GameObject NowDestoryEffect;
    public bool GetIs_Active() { return is_Active; }

    [Header("CoinTxt")]
    public GameObject DropCoin;

    int mincoin;
    int maxcoin;

    private void Awake()
    {
        SR_objPooling = GameObject.Find("ObjPooling").GetComponent<SR_ObjPooling>();
        SR_finshGame = GameObject.Find("Cvs_FinshGame").GetComponent<SR_FinshGame>();
        target = GameObject.Find("EndPos").GetComponent<Transform>();
        ani = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        Initialize(0);
    }
    void Start()
    {
        agent.SetDestination(target.position);
        now_MagazineCapacity = max_MagazineCapacity;
        agent.speed = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].Speed;
        mincoin = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].minRandomCoin;
        maxcoin = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].maxRandomCoin;
        SR_coion = GameObject.Find("Cvs_Coin").GetComponent<SR_Coin>();
    }

    private void FixedUpdate()
    {
        if (!is_Active) { return; }
        
        if (FindPlayer)
        {
           
            if (enemyType != EnemyType.Bomb)
            {
                agent.isStopped = true;
                ani.SetBool("Shoot", true);
              
            }
            else
            {
                agent.destination = Target.transform.position;
                float speedFactor = Mathf.InverseLerp(16.0f, 0.0f, agent.remainingDistance);
                
                agent.speed = Mathf.Lerp(10.0f, 5.0f, speedFactor);
            }
        }
        else
        {
           
            if (enemyType != EnemyType.Bomb)
            {
                
               
                ani.SetBool("Shoot", false);
            }else
            {
                agent.speed = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].Speed;
                agent.destination = new Vector3(-77.0f, 0.0f, 25.0f);
            }
            agent.isStopped = false;
        }
    }
    private void Update()
    {
        
        if(!is_Active) { return; }

        TurnBodyToPlayer();
        if (!agent.isStopped && enemyType == EnemyType.Car)
        {
            for (int i = 0; i < 4; i++)
            {
                Tire[i].transform.Rotate(Vector3.right * Time.deltaTime * 1000.0f); ;
            }
        }

        Slider_Hp.value = hp / maxHp;
        HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, hpbarPosY, 0));

        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            reload.SetActive(true);
            reload.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, reloadPosY, 0));
            reloadImg.transform.Rotate(0, 0, -250.0f * Time.deltaTime);
        }
        else { reload.SetActive(false); }
    }
    void TurnBodyToPlayer()
    {
        Vector3 myPos = BulletSpawnPos.position;
        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        if(DebugMode)
        {
            Debug.DrawRay(myPos, rightDir * ViewRadius, Color.blue);
            Debug.DrawRay(myPos, leftDir * ViewRadius, Color.blue);
            Debug.DrawRay(myPos, lookDir * ViewRadius, Color.cyan);
        }
       
        hitTargetList.Clear();
        Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, NowTargetMask);


        float minRange = 10000;
        string tagName = "";
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            float range = Vector3.Distance(myPos, targetPos);
            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, NowObstacleMask))
            {
                hitTargetList.Add(EnemyColli);
                if (range < minRange)
                {
                    tagName = EnemyColli.tag;
                    minRange = range;
                    minPos = targetPos;
                    minPos.y = 1.8f;
                    Target = EnemyColli.gameObject;
                }
            }
        }
        FindPlayer = hitTargetList.Count > 0 ? true : false;
        if (FindPlayer && is_Active)
        {
            Vector3 targetDirection = (minPos - BulletSpawnPos.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            //Vector3 vector = minPos - transform.position;
            //vector.y = 1;
            //if (enemyType == EnemyType.Car) { vector.y -= 3; }
            //Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, Quaternion.LookRotation(vector).normalized, Time.deltaTime * rotationSpeed);
        }
        else
        {
            if (agent.velocity.magnitude < 5f && !agent.isPathStale&& transform.position.x < 80.0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(Vector3.left);
                Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, transform.rotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
    void Fire()
    {
        GameObject bullet = SR_objPooling.GetObj(SR_ObjPooling.PoolingType.EnemyBullet);
        if (bullet != null)
        {
            SR_SoundManager.instance.PlaySfx(-0.1f, SR_SoundManager.Sfx.GunFire);
            float critical = 0f;
            bullet.gameObject.GetComponent<SR_Bullet>().Initialize(SR_Bullet.OwnerType.Enemy, bulletDamage, critical);
            bullet.transform.position = BulletSpawnPos.position;
            bullet.transform.rotation = BulletSpawnPos.rotation;

            bullet.GetComponent<Rigidbody>().velocity = BulletSpawnPos.forward * 100.0f;
            now_MagazineCapacity--;
            if (now_MagazineCapacity <= 0)
            {
                ani.SetTrigger("Reload");
                now_MagazineCapacity = max_MagazineCapacity;
            }
        }
    }
    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    public void Initialize(int priorityNum)
    {
        hp = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].Hp;
        bulletDamage = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].bulletDamage;

        max_MagazineCapacity = SR_GameManager.instance.SR_enemyData.enemyData[(int)enemyType].max_MagazineCapacity;
        maxHp = hp;
        Slider_Hp.value = hp / maxHp;
        gameObject.GetComponent<CapsuleCollider>().enabled = true;

        if (enemyType == EnemyType.Car)
        {
            Car.SetActive(true);
            DestoryCar.SetActive(false);
        }
        agent.SetDestination(target.position);
        agent.enabled = true;
        agent.avoidancePriority = priorityNum;
        is_Active = true;
        StartCoroutine(corutine());

    }
    public void Attacked(float damage)
    {
        if (hp <= 0.0f&& !is_Active) { return; }
        bool isDeath = hp - damage <= 0.0f;


        if (isDeath)
        {
            hp = 0.0f; 
            is_Active = false;

            Invoke("DestroyEnemy", 2);
            DroptheCoin();
            
            HpBar.SetActive(false); //rigid.isKinematic = true;
            reload.SetActive(false);
            ani.SetBool("Death", true);
            rigid.velocity = Vector3.zero;
            agent.isStopped = true;
           
            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            SR_finshGame.AddEnemyKill();

            if (enemyType == EnemyType.Car)
            {
                Car.SetActive(false);
                DestoryCar.SetActive(true);
                NowDestoryEffect =Instantiate(DestoryEffect, transform);
                SR_SoundManager.instance.PlaySfx(SR_SoundManager.Sfx.CarDestoy);
            }
            else
            {
                SR_SoundManager.instance.PlaySfx(SR_SoundManager.Sfx.Die);
            }
        }
        else
        {
            SR_SoundManager.Sfx sfx = (SR_SoundManager.Sfx)Random.Range(8, 10);

            SR_SoundManager.instance.PlaySfx(sfx, 0.3f);
            hp -= damage;
            HpBar.SetActive(true);
        }
        AppearDamageTxt(damage);
    }
    void DroptheCoin()
    {
        int randomCoin = Random.Range(mincoin, maxcoin);
        SR_coion.AddCoin(randomCoin);
        GameObject DropCoinTxt = Instantiate(DropCoin);
        Vector3 newCoinLocalScale = Vector3.one + Vector3.one * (randomCoin * 0.1f);
        DropCoinTxt.transform.GetChild(0).GetComponent<RectTransform>().transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 3.0f, 0));
        DropCoinTxt.transform.GetChild(0).GetComponent<Text>().text = randomCoin.ToString();
        DropCoinTxt.transform.GetChild(0).GetComponent<RectTransform>().localScale = newCoinLocalScale;
    }
    void AppearDamageTxt(float damage)
    {
        GameObject DamageTxt = Instantiate(AttackedTxt);
        DamageTxt.transform.GetChild(0).GetComponent<RectTransform>().transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), damgePosY, 0));
        DamageTxt.transform.GetChild(0).GetComponent<Text>().text = string.Format("-{0:F1}",damage);
        Vector3 newLocalScale;
        if (damage > 15.0f)
        {
            newLocalScale = Vector3.one + Vector3.one * (15 * 0.1f);
        }
        else
        {
            newLocalScale = Vector3.one + Vector3.one * (damage * 0.1f);
        }

        DamageTxt.transform.GetChild(0).GetComponent<RectTransform>().localScale = newLocalScale;
        if (damage > 4.0f)
        {
            DamageTxt.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    void DestroyEnemy()
    {
        gameObject.SetActive(false);
        this.transform.position = new Vector3(150, 0, 25);
        if(NowDestoryEffect != null) { Destroy(NowDestoryEffect); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (enemyType == EnemyType.Bomb && (collision.transform.tag == "Army" || collision.transform.tag == "Base"))
        {
            SR_SoundManager.instance.PlaySfx(0.1f, SR_SoundManager.Sfx.EnemyBomb);
            GameObject bomb = Instantiate(Bomb);
            bomb.transform.position = transform.position;
            Attacked(hp);
        }
    }
    IEnumerator corutine()
    {
        while(is_Active)
        {

            if (agent.pathStatus == NavMeshPathStatus.PathPartial&& agent.velocity.magnitude<2f)
            {
                NowTargetMask = BlockedTargetMask;
                NowObstacleMask = BlockedObstacleMask;
            }
            else
            {
                NowTargetMask = TargetMask;
                NowObstacleMask = ObstacleMask;
            }
            yield return new WaitForSeconds(1.0f);
        }
        yield return null;
    }
 
}
