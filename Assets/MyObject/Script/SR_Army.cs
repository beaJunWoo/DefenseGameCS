using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class SR_Army : SR_PlayerObj
{
    [Header("AttackRangeSetteing")]
    public bool DebugMode = false;
    [Range(0f, 360f)] [SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 1f;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;
    Vector3 minPos;
    protected bool FindPlayer = false;
    List<Collider> hitTargetList = new List<Collider>();

    [Header("AttackStyle")]
    public float DefaultRotationSpeed = 10.0f;
    public Transform BulletSpawnPos;
    public Transform BulletEffectPos;
    public SpriteRenderer SPR_AttackRange;

    [Header("Releading")]
    protected float reloadPosY = 4.3f;
    public GameObject reload;
    public Transform reloadImg;
    public int max_MagazineCapacity;
    [SerializeField] protected int now_MagazineCapacity = 0;
    
    [Header("RotationBody")]
    public Transform Body;

    protected bool is_Active;
    protected SR_ObjPooling SR_objPooling;
    protected Animator ani;

    public GameObject flashFX;
    SR_EnemySpawnManager SR_enemySpawnManager;
    private void Start()
    {
        armyData = SR_GameManager.instance.armyData;
        ani = GetComponent<Animator>();
        SR_objPooling = GameObject.Find("ObjPooling").GetComponent<SR_ObjPooling>();
        now_MagazineCapacity = max_MagazineCapacity;
        hitTargetList.Clear();
        FindPlayer = false;
        Initialize();
        StartCoroutine(HealCoroutin());
        SetDamage();
    }
    private void FixedUpdate()
    {
        if (is_Active)
        {
          
            ani.SetFloat("FireSpeed", 1.0f + debuffDataBase.debuffData[(int)DebuffData.DebuffType.RPMBuff].NowDebuffAmount * 0.01f);
            TargettingEnemy();
            MaxHp = defaultHp + debuffDataBase.debuffData[(int)DebuffData.DebuffType.MaxHpBuff].NowDebuffAmount;
            SR_enemySpawnManager = GameObject.Find("EnemySpawnManager").GetComponent<SR_EnemySpawnManager>();
        }
        SetImgAttackRange();
    }

    void Update()
    {
        Slider_Hp.value = hp / MaxHp;
        if (HpBar.activeSelf)
        {
            HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, hpbarPosY, 0));
        }
        if (ani.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            reload.SetActive(true);
            reload.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, reloadPosY, 0));
            reloadImg.transform.Rotate(0, 0, -250.0f * Time.deltaTime);
        }
        else { reload.SetActive(false); }
    }

    protected void TargettingEnemy()
    {
        hitTargetList.Clear();
        Vector3 myPos = transform.position + Vector3.up * 0.5f;

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        
        Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);

        float minRange = 10000;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            float range = Vector3.Distance(myPos, targetPos);
            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
            {
                hitTargetList.Add(EnemyColli);
                if (range < minRange)
                {
                    minRange = range;
                    minPos = targetPos;
                    minPos.y = 1.5f;
                }
            }
        }
      
        if((hitTargetList.Count != 0)!=FindPlayer)
        {
            FindPlayer = hitTargetList.Count != 0;
            ani.SetBool("Shoot", FindPlayer);
        }
      
        if (FindPlayer && is_Active)
        {
            Vector3 targetDirection = (minPos - BulletSpawnPos.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            float RotationSeed = DefaultRotationSpeed + debuffDataBase.debuffData[(int)DebuffData.DebuffType.AimAccuracy].NowDebuffAmount * 0.01f * DefaultRotationSpeed;
            Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, targetRotation, Time.deltaTime * RotationSeed);

            //Vector3 vector = minPos - transform.position;
            //vector.y = 0f;
            //Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, Quaternion.LookRotation(vector).normalized, Time.deltaTime * RotationSeed);
        }
        else
        {
            Body.transform.rotation = Quaternion.Lerp(Body.transform.rotation, transform.rotation, Time.deltaTime * DefaultRotationSpeed);
        }
    }

    void OnDrawGizmos()
    {
        if(DebugMode)
        {
            Vector3 myPos = transform.position + Vector3.up * 0.5f;

            float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
            Vector3 lookDir = AngleToDir(lookingAngle);

            Debug.DrawRay(myPos, rightDir * ViewRadius, Color.blue);
            Debug.DrawRay(myPos, leftDir * ViewRadius, Color.blue);
            Debug.DrawRay(myPos, lookDir * ViewRadius, Color.cyan);

            Gizmos.DrawWireSphere(myPos, ViewRadius);
        }
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }
    protected override void SetDamage()
    {

        if (barrierType != BarrierType.Noting) { return; }
        Debug.Log(armyType.ToString());
        bulletDamage = armyData.armyData[(int)armyType].defaultDamage + armyData.armyData[(int)armyType].additionalDamage;
    }
    protected override void Die()
    {
        is_Active = false;
        ani.SetBool("Death", true);
        Invoke("Destory", 10);
        gameObject.GetComponent<BoxCollider>().enabled = false;
       
    }
    public override void Initialize()
    {
        is_Active = true;
        hp = ArmyHp[(int)armyType];
        gameObject.GetComponent<BoxCollider>().enabled = true;
        transform.rotation = Quaternion.LookRotation(Vector3.right);
        defaultHp = hp;
        Slider_Hp.value = hp / MaxHp;
    }
    void Destory()
    {
        gameObject.SetActive(false);
    }
    void Destory(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void Fire()
    {
        SR_SoundManager.instance.PlaySfx((SR_SoundManager.Sfx)armyType);
        GameObject bullet = null;
        if (gameObject.tag == "Army")
             bullet = SR_objPooling.GetObj(SR_ObjPooling.PoolingType.ArmyBullet);
        if (gameObject.tag == "Enemy")
             bullet = SR_objPooling.GetObj(SR_ObjPooling.PoolingType.EnemyBullet);

        if (bullet != null)
        {
            float BulletDamage =  bulletDamage+ debuffDataBase.debuffData[(int)DebuffData.DebuffType.DamageBuff].NowDebuffAmount*bulletDamage*0.01f;
            float critical = armyData.armyData[(int)armyType].critical;
            if (gameObject.tag == "Army")
                bullet.gameObject.GetComponent<SR_Bullet>().Initialize(SR_Bullet.OwnerType.Army, BulletDamage, critical);
            if (gameObject.tag == "Enemy")
                bullet.gameObject.GetComponent<SR_Bullet>().Initialize(SR_Bullet.OwnerType.Enemy, BulletDamage, critical);
            bullet.transform.position = BulletSpawnPos.position;
            bullet.transform.rotation = BulletSpawnPos.rotation;

            Instantiate(flashFX, BulletEffectPos);

            bullet.GetComponent<Rigidbody>().velocity = BulletSpawnPos.forward * 100.0f;
            now_MagazineCapacity--;
            if (now_MagazineCapacity <= 0)
            {
                ani.SetTrigger("Reload");
                ani.SetFloat("ReloadSpeed", 1.0f+debuffDataBase.debuffData[(int)DebuffData.DebuffType.Reloadshorten].NowDebuffAmount*0.01f);
                now_MagazineCapacity = max_MagazineCapacity;
            }
        }
    }
    
   
    public void SetImgAttackRange()
    {
        if (SPR_AttackRange == null) { return; }
        if (!SR_enemySpawnManager.startGame) { return; }
        float colorA;
        if (is_Active)
        {
            colorA = FindPlayer ? 0.4f : 0.2f;
        }
        else
        {
            colorA = 0.0f;
        }
        UpdateAttackRangeColor(colorA);
    }
    public void UpdateAttackRangeColor(float colorA)
    {
        Color newColor = SPR_AttackRange.color;
        newColor.a = colorA;
        SPR_AttackRange.color = newColor;
    }
   
    public void Heal(float healAmount)
    {
       if (!is_Active) { return; }
       MaxHp = defaultHp + debuffDataBase.debuffData[(int)DebuffData.DebuffType.MaxHpBuff].NowDebuffAmount;
       if (hp + healAmount > MaxHp)
       {
            hp = MaxHp;
        }
        else
        {
            hp += healAmount;
        }
    }

    IEnumerator HealCoroutin()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            
            if (!is_Active)
            {
                continue;
            }

            long HealAmount = (long)(debuffDataBase.debuffData[(int)DebuffData.DebuffType.ContinuousHealing].NowDebuffAmount * 0.01f);
            if (hp + HealAmount > MaxHp)
            {
                hp = MaxHp;
            }
            else
            {
                hp += HealAmount;
            }
        }
    } 
}
