using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SR_Skills : MonoBehaviour
{

    public Button missileButton; // 인스펙터에서 할당
    public Button minigunButton; // 인스펙터에서 할당

    public SR_SkillsData SR_skillsSettingData;
    public SR_Fastforward SR_fastforward; 
    SkillsData.SkillType[] skillType;
    public GameObject[] skillTimeText;
    public Image[] skill_Image;
    public Image[] skillOffImage;
    public Text[] Txt_skillsPrice;
    Text[] Txt_skillTime;

    bool[] skillOn = { };
    float[] nowSkillTime = { };

    [Header("#DronSettings")]
    public GameObject MissileControlUI;
    public GameObject Dron;
    public Transform DronePos;

    [Header("#minigunSettings")]
    public GameObject MinigunArmy;
    public Transform minigunPos;

    Camera Maincamera;
    Vector3 pos;
    SR_Coin SR_coin;

    void Start()
    {
        Maincamera = Camera.main;
        SR_coin = GameObject.Find("Cvs_Coin").GetComponent<SR_Coin>();

        Txt_skillTime = new Text[skillTimeText.Length];
        nowSkillTime = new float[skillTimeText.Length];
        for (int i = 0; i < skillTimeText.Length; i++)
        {
            Txt_skillTime[i] = skillTimeText[i].GetComponent<Text>();
            skillTimeText[i].SetActive(false);
            nowSkillTime[i] = 0f;
            SR_skillsSettingData.skillsData[i].SkillCost = SR_skillsSettingData.skillsData[i].defaultSkillCost;
            Txt_skillsPrice[i].text = SR_skillsSettingData.skillsData[i].SkillCost.ToString();
            skill_Image[i].sprite = SR_skillsSettingData.skillsData[i].skill_img;
        }
        missileButton.onClick.AddListener(() => SKillBtnOn(SkillsData.SkillType.MISSILE));
        minigunButton.onClick.AddListener(() => SKillBtnOn(SkillsData.SkillType.MINIGUN));
    }

    public void SKillBtnOn(SkillsData.SkillType skillType)
    {
        int Index = (int)skillType;

        int skillCost = SR_skillsSettingData.skillsData[Index].SkillCost;

        if (!SR_coin.DeductionCoin(skillCost)) { return; }

        switch (skillType)
        {
            case SkillsData.SkillType.MISSILE:
                StartCoroutine(MissileOn());
                break;
            case SkillsData.SkillType.MINIGUN:
                MinigunOn();
                break;

        }
        SR_skillsSettingData.skillsData[Index].SkillCost = (int)(SR_skillsSettingData.skillsData[Index].SkillCost * 1.25f);
        Txt_skillsPrice[Index].text = SR_skillsSettingData.skillsData[Index].SkillCost.ToString();
        StartCoroutine(SetBtnTimeCoroutine(Index));
    }

    void MinigunOn()
    {
        GameObject minigunArmy =Instantiate(MinigunArmy);
        minigunArmy.transform.position = minigunPos.position;
    }
    IEnumerator MissileOn()
    { 
        MissileControlUI.SetActive(true);
        Time.timeScale = 0.1f;
        while (true)
        {
            if (MissileControlUI.activeSelf && Input.GetMouseButton(0))
            {
                Ray ray = Maincamera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Tile")))
                {
                    pos = hit.point;
                }
                Debug.Log(pos.x);
                MissileControlUI.SetActive(false);
                SR_fastforward.UpdateSpeed();
                GameObject dron = Instantiate(Dron);
                Vector3 newPos = DronePos.position;
                newPos.z = pos.z;
                dron.transform.position = newPos;
                dron.GetComponent<SK_SkillofType>().SetBombPos(pos);
                yield break;
            }
            yield return null;
        }
    }
    IEnumerator SetBtnTimeCoroutine(int Index)
    {
       skillTimeText[Index].SetActive(true);
       nowSkillTime[Index] = SR_skillsSettingData.skillsData[Index].SkillCooltime;
       
       while (nowSkillTime[Index] > 0)
       {
           nowSkillTime[Index] -= Time.deltaTime;
           Txt_skillTime[Index].text = ((int)nowSkillTime[Index]).ToString();
       
           float time = nowSkillTime[Index] / SR_skillsSettingData.skillsData[Index].SkillCooltime;
           skillOffImage[Index].fillAmount = time;
       
           if (nowSkillTime[Index] <= 0.0f)
           {
               nowSkillTime[Index] = 0.0f;
               skillTimeText[Index].SetActive(false);
           }
       
           yield return null; 
       }
    }
}   
