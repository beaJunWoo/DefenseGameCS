using UnityEngine;
using UnityEngine.UI;

public class SR_Upgrade : MonoBehaviour
{
    public GameObject[] UpgradeArmys;
    SR_ArmyData armyData;

    [SerializeField] Text[] Txt_DamagePrice;
    [SerializeField] Text[] Txt_CriticalPrice;
    [SerializeField] Text[] Txt_ShieldPrice;

    [SerializeField] Slider[] S_Damage;
    [SerializeField] Slider[] S_Critical;
    [SerializeField] Slider[] S_Shield;

    Text[] ArmyNames;

    Text[] Txt_Damage;
    Text[] Txt_Critical;
    Text[] Txt_Shield;

    Image[] ArmyImages;

    public Button[] btn_DamageUpgrade;
    public Button[] btn_CriticalUpgrade;
    public Button[] btn_ShieldUpgrade;

    public void Awake()
    {
        armyData = SR_GameManager.instance.armyData;
    }
    public void Start()
    {
        ArmyNames = new Text[UpgradeArmys.Length];
        ArmyImages = new Image[UpgradeArmys.Length];
        Txt_Damage  = new Text[UpgradeArmys.Length];
        Txt_Critical = new Text[UpgradeArmys.Length];
        Txt_Shield = new Text[UpgradeArmys.Length];
        for (int i = 0; i < UpgradeArmys.Length; i++)
        {
            ArmyNames[i] = UpgradeArmys[i].transform.GetChild(0).transform.GetComponent<Text>();
            ArmyNames[i].text = armyData.armyData[i].name;
            ArmyImages[i] = UpgradeArmys[i].transform.GetChild(1).transform.GetChild(0).transform.GetComponent<Image>();
            ArmyImages[i].sprite = armyData.armyData[i].sprite;
            Txt_Damage[i] = UpgradeArmys[i].transform.GetChild(2).transform.GetChild(0).transform.GetComponent<Text>();
            Txt_Critical[i] = UpgradeArmys[i].transform.GetChild(2).transform.GetChild(1).transform.GetComponent<Text>();
            Txt_Shield[i] = UpgradeArmys[i].transform.GetChild(2).transform.GetChild(2).transform.GetComponent<Text>();
        }
        UpdateUI();

        for (int i=0; i< btn_DamageUpgrade.Length; i++)
        {
            int index = i;
            btn_DamageUpgrade[i].onClick.AddListener(() => DamageUpgrade((ArmyType)index));
        }
           
           
        for(int i=0;  i<btn_CriticalUpgrade.Length; i++)
        {
            int index = i;
            btn_CriticalUpgrade[i].onClick.AddListener(() => CriticalUpgrade((ArmyType)index));
        }
            

        for (int i = 0; i < btn_ShieldUpgrade.Length; i++)
        {
            int index = i;
            btn_ShieldUpgrade[i].onClick.AddListener(() => ShieldUpgrade((ArmyType)index));
        }
          
    }
    void DamageUpgrade(ArmyType armyType)
    {
        
        int price = armyData.armyData[(int)armyType].UpgradCost_addDamage;
        if (!SR_GameManager.instance.DecGameMoney(price)) { return; }
        Debug.Log("DamageUpgrade");
        armyData.armyData[(int)armyType].additionalDamage = Mathf.Lerp(armyData.armyData[(int)armyType].additionalDamage, armyData.MaxAdditionalDamage, 0.1f);
        armyData.armyData[(int)armyType].UpgradCost_addDamage =(int)(armyData.armyData[(int)armyType].UpgradCost_addDamage*1.5f);
        SR_GameManager.instance.DamageUpgradeCount[(int)armyType]++;
        UpdateUI();
    }
    void CriticalUpgrade(ArmyType armyType)
    {
        Debug.Log(armyType.ToString());
        int price = armyData.armyData[(int)armyType].UpgradCost_critical;
        if (!SR_GameManager.instance.DecGameMoney(price)) { return; }
        Debug.Log(Mathf.Lerp(armyData.armyData[(int)armyType].critical, armyData.MaxCritical, 0.1f));
        armyData.armyData[(int)armyType].critical = Mathf.Lerp(armyData.armyData[(int)armyType].critical, armyData.MaxCritical, 0.1f);
        armyData.armyData[(int)armyType].UpgradCost_critical = (int)(armyData.armyData[(int)armyType].UpgradCost_critical * 1.5f);
        SR_GameManager.instance.CriticalUpgradeCount[(int)armyType]++;
        UpdateUI();
    }
    void ShieldUpgrade(ArmyType armyType)
    {
       
        int price = armyData.armyData[(int)armyType].UpgradCost_shield;
        if (!SR_GameManager.instance.DecGameMoney(price)) { return; }
        Debug.Log("ShieldUpgrade");
        armyData.armyData[(int)armyType].shield = Mathf.Lerp(armyData.armyData[(int)armyType].shield, armyData.MaxShield, 0.1f);
        armyData.armyData[(int)armyType].UpgradCost_shield = (int)(armyData.armyData[(int)armyType].UpgradCost_shield * 1.5f);
        SR_GameManager.instance.ShieldUpgradeCount[(int)armyType]++;
        UpdateUI();
    }
    void UpdateUI()
    {
        for (int i = 0; i < Txt_Damage.Length; i++)
            Txt_Damage[i].text = $"데미지 : {(armyData.armyData[i].defaultDamage + armyData.armyData[i].additionalDamage):F2}";

        for (int i = 0; i < Txt_Critical.Length; i++)
            Txt_Critical[i].text = $"치명타 : {(armyData.armyData[i].critical):F2}%";

        for (int i = 0; i < Txt_Shield.Length; i++)
            Txt_Shield[i].text = $"방어력 : {(armyData.armyData[i].shield):F2}%";



        for (int i = 0; i < Txt_DamagePrice.Length; i++)
            Txt_DamagePrice[i].text = armyData.armyData[i].UpgradCost_addDamage.ToString();

        for (int i = 0; i < Txt_CriticalPrice.Length; i++)
            Txt_CriticalPrice[i].text = armyData.armyData[i].UpgradCost_critical.ToString();

        for (int i = 0; i < Txt_ShieldPrice.Length; i++)
            Txt_ShieldPrice[i].text = armyData.armyData[i].UpgradCost_shield.ToString();

        for (int i=0; i<S_Damage.Length; i++)
        {
            float newValue = armyData.armyData[i].additionalDamage / armyData.MaxAdditionalDamage;
            S_Damage[i].value = newValue;
        }
        for (int i = 0; i < S_Critical.Length; i++)
        {
            float newValue = armyData.armyData[i].critical / armyData.MaxCritical;
            S_Critical[i].value = newValue;
        }
        for (int i = 0; i < S_Shield.Length; i++)
        {
            float newValue = armyData.armyData[i].shield / armyData.MaxShield;
            S_Shield[i].value = newValue;
        }
    }  


}


    

