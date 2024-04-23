using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class SR_MultiPlayerInfo : MonoBehaviour
{
    public Button btn_Attack;

    public Text Txt_P1;
    public Text Txt_P2;

    public Slider Hpbar_P1;
    public Slider Hpbar_P2;

    public float P1Hp;
    public float P2Hp;

    bool EnemySpawnArmy = false;
    bool ArmySpawnArmy = false;
    public Transform p1Pos;
    public Transform p2Pos;

    public GameObject MinigunArmy;
    public GameObject MinigunEnemy;

    DatabaseReference roomRef;
    int PlayerNum =1;
    private void Awake()
    {
        if(GameObject.Find("Cvs_PVP"))
        {
            roomRef = GameObject.Find("Cvs_PVP").GetComponent<FB_PVP>().roomRef;
            roomRef.ValueChanged += HandleValueChanged;
            PlayerNum = GameObject.Find("Cvs_PVP").GetComponent<FB_PVP>().PlayerNum;
        }
        FetchPlayerInfo();
        UpdateHp();
    }
    private void Start()
    {
        btn_Attack.onClick.AddListener(() => SpawnArmy());
    }
    private void Update()
    {
        Hpbar_P1.value = P1Hp / 100.0f;
        Hpbar_P2.value = P2Hp / 100.0f;

        if(EnemySpawnArmy)
        {
            EnemySpawnArmy = false;
            if(PlayerNum == 1) //내가 p1이라면 
            {
                GameObject p1Minigun = Instantiate(MinigunEnemy);
                p1Minigun.transform.position = p2Pos.position;
                p1Minigun.GetComponent<SR_MoveArmy>().target = p1Pos;
            }
            else
            {
                GameObject p2Minigun = Instantiate(MinigunArmy);
                p2Minigun.transform.position = p1Pos.position;
                p2Minigun.GetComponent<SR_MoveArmy>().target = p2Pos;
            }
           
        }
        if(ArmySpawnArmy)
        {
            ArmySpawnArmy = false;
            if(PlayerNum == 1)
            {
                GameObject minigunArmy = Instantiate(MinigunArmy);
                minigunArmy.transform.position = p1Pos.position;
                minigunArmy.GetComponent<SR_MoveArmy>().target = p2Pos;
            }
            else
            {
                  GameObject minigunArmy = Instantiate(MinigunEnemy);
            minigunArmy.transform.position = p2Pos.position;
            minigunArmy.GetComponent<SR_MoveArmy>().target = p1Pos;
            }
        }
    }
    private void HandleValueChanged(object sender, ValueChangedEventArgs e)
    {
        if (e.DatabaseError != null)
        {
            Debug.LogError(e.DatabaseError.Message);
            return;
        }
        UpdateHp();
        CheckSpawnArmy();
    }
    void FetchPlayerInfo()
    {
        roomRef.Child("p1").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Txt_P1.text = snapshot.Value.ToString();
            }
        });

        roomRef.Child("p2").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Txt_P2.text = snapshot.Value.ToString();
            }
        });

       
    }
    void UpdateHp()
    {
        roomRef.Child("p1Hp").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                P1Hp = float.Parse(snapshot.Value.ToString());
            }
        });
        roomRef.Child("p2Hp").GetValueAsync().ContinueWith(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                P2Hp = float.Parse(snapshot.Value.ToString());
            }
        });
    }
    void CheckSpawnArmy()
    {
        if(PlayerNum==1)
        {
            roomRef.Child("p2_Army").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    EnemySpawnArmy = (bool)snapshot.Value;
                    if (EnemySpawnArmy)
                    {
                        roomRef.Child("p2_Army").SetValueAsync(false);
                    }
                }
            });
        }
        else
        {
            roomRef.Child("p1_Army").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    EnemySpawnArmy = (bool)snapshot.Value;
                    if(EnemySpawnArmy)
                    {
                        roomRef.Child("p1_Army").SetValueAsync(false);
                    }
                    
                }
            });
        }
       
    }
    void SpawnArmy()
    {
        Debug.Log("아군 생성");

        if (PlayerNum == 1)//내가 p1이라면 
        {
 
            roomRef.Child("p1_Army").SetValueAsync(true).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    ArmySpawnArmy = true;
                }
            });
        }
        else
        {
          
            roomRef.Child("p2_Army").SetValueAsync(true).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    ArmySpawnArmy = true;
                }
            });
        }
    }
    void Attack()
    {
        if(PlayerNum==1)
        {
            P2Hp -= 10;
            roomRef.Child("p2Hp").SetValueAsync(P2Hp);
        }else
        {
            P1Hp -= 10;
            roomRef.Child("p1Hp").SetValueAsync(P1Hp);
        }
        UpdateHp();
    }
   
}
