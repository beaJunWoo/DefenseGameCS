using UnityEngine;
using UnityEngine.UI;

public class SR_PlayerObj : MonoBehaviour
{
    public enum BarrierType { Noting, Sandbags, Wall }

    protected SR_ArmyData armyData;
  
    [Header("ArmyTypeSetting")]
    public ArmyType armyType;
    public BarrierType barrierType;

    static protected int[] ArmyHp = {20, 30, 40,50, 80 };
    static protected int[] barrierHp = { 50, 200, 30, 40 };

    [SerializeField] protected float bulletDamage;
    [SerializeField] protected float hp;
    [SerializeField] int price;

    [Header("HpBar")]
    public GameObject HpBar;
    public Slider Slider_Hp;
    public float defaultHp;
    public float MaxHp;
    public float hpbarPosY =3.5f;

    public SR_DebuffDataBase debuffDataBase;

    void Start()
    {
        armyData = SR_GameManager.instance.armyData;
        SetDamage();
        Initialize();
    }
    void Update()
    {
        Slider_Hp.value = hp / defaultHp;
        if (HpBar.activeSelf)
        {
            HpBar.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, hpbarPosY, 0));
        }
    }
    public void Attacked(float damage)
    {
        float reductionPercent1 = debuffDataBase.debuffData[(int)DebuffData.DebuffType.ArmorBuff].NowDebuffAmount;
        float decDamage1 = Mathf.Min(damage * 0.5f, damage * reductionPercent1 * 0.01f);

        float reductionPercent2 = 0.0f;
        if (barrierType == BarrierType.Noting)
        {
            reductionPercent2 = armyData.armyData[(int)armyType].shield;
        }
        float decDamage2 = Mathf.Min(damage * 0.5f, reductionPercent2 + decDamage1);
        float newDamage = damage - decDamage2;

        if (hp - newDamage > 0.0f)
        {    
            hp -= newDamage;
            Debug.Log(hp);
            HpBar.SetActive(true);
        }
        else 
        { 
            hp = 0f;
            Die();
            HpBar.SetActive(false);
        }

    }
    public void SetPrice(int price)
    {

        this.price = price;
    }
    protected virtual void SetDamage()
    {
        if (barrierType != BarrierType.Noting) { return; }
        Debug.Log(armyType.ToString());
        bulletDamage = armyData.armyData[(int)armyType].defaultDamage+armyData.armyData[(int)armyType].additionalDamage;
    }
    public int GetPrice()
    {
        return price;
    }
    protected virtual void Die() 
    {
        gameObject.SetActive(false);
    }
    public virtual void Initialize()
    {
        if (barrierType == BarrierType.Noting)
        {
            hp = ArmyHp[(int)armyType];
        }
        else
        {
            hp = barrierHp[(int)barrierType-1];
        }
       
        defaultHp = hp;
        Slider_Hp.value = hp / defaultHp;
    }
}
