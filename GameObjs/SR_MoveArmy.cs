using UnityEngine;
using UnityEngine.AI;

public class SR_MoveArmy : SR_Army
{
    Transform target;
    NavMeshAgent agent;
    void Start()
    {
        armyData = SR_GameManager.instance.armyData;
        SR_objPooling = GameObject.Find("ObjPooling").GetComponent<SR_ObjPooling>();
        target = GameObject.Find("StartPos").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
        ani = GetComponent<Animator>();
        reload.SetActive(false);
        now_MagazineCapacity = max_MagazineCapacity;
        Initialize();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (is_Active)
        {
            ani.SetFloat("FireSpeed", 1.0f + debuffDataBase.debuffData[(int)DebuffData.DebuffType.RPMBuff].NowDebuffAmount * 0.01f);
            if (FindPlayer) { agent.isStopped = true; }
            else { agent.isStopped = false; }

            ani.SetBool("Shoot", FindPlayer);
            TargettingEnemy();
        }
        SetImgAttackRange();
    }

    void Update()
    {
        Slider_Hp.value = hp / defaultHp;
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

}
