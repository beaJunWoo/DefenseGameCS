using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SR_AutoMove : MonoBehaviour
{
    public Transform TF_MainCamera;
    public Toggle Tg_AutoMove;
    public float cameraSpeed=10.0f;
    public float maxRange = 100.0f;
    public float focusPos = 5.0f;
    
    [SerializeField]
    float leftMax;

    GameObject Target;

    private void Start()
    {
        leftMax = GameObject.Find("MilitaryBase").transform.position.x + 20;
        StartCoroutine(CheckCoroutine());
    }
    void Update()
    {
        if (Tg_AutoMove.isOn && leftMax< TF_MainCamera.position.x)
        {
            float newX = Mathf.Lerp(TF_MainCamera.position.x, GetPosX(), cameraSpeed * Time.deltaTime);
            Vector3 newPos = new Vector3(newX, TF_MainCamera.position.y, TF_MainCamera.position.z);
            TF_MainCamera.position = newPos;
        }

    }
    void FindEnemyPos()
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");

        float MinX =float.MaxValue;
        for(int i=0; i<Enemys.Length; i++)
        {
            if (Enemys[i].transform.position.x < MinX && Enemys[i].transform.position.x < maxRange&& Enemys[i].GetComponent<SR_Enemy>().GetIs_Active() )
            {
                MinX = Enemys[i].transform.position.x;
                Target = Enemys[i];
            }
        }
       
    }
    float GetPosX()
    {
        if(Target == null) { return TF_MainCamera.position.x; }
        float PosX =Target.transform.position.x;
        if (PosX > maxRange) { return TF_MainCamera.position.x; }
        else { return PosX - focusPos; }
    }

    IEnumerator CheckCoroutine()
    {
        while (true)
        {
            FindEnemyPos();
            yield return new WaitForSeconds(1);
        }
    }
}
