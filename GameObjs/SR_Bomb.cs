using Unity.VisualScripting;
using UnityEngine;

public class SR_Bomb : MonoBehaviour
{
    public GameObject bombEffect;
    public Transform EffectPos;
    bool isBomb = false;

    [SerializeField]
    float maxLenght;

    SR_CameraShake cameraShake;
    private void Start()
    {
        cameraShake = Camera.main.GetComponent<SR_CameraShake>();
        Invoke("Delete", 2);
    }
    void Attack()
    {
        SR_SoundManager.instance.PlaySfx(0.3f,SR_SoundManager.Sfx.Bomb);
        cameraShake.StartShake();
        isBomb = true;
        GameObject bombeffect = Instantiate(bombEffect,EffectPos.position,Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        float lenght = Vector3.Distance(other.gameObject.transform.position, transform.position);
        Debug.Log(lenght);
        float percentDamage = (maxLenght - lenght) / maxLenght;
        if (other.tag == "Enemy")
        {
            other.transform.GetComponent<SR_Enemy>().Attacked(100* percentDamage);
        }
        if (other.tag == "Tile")
        {  
           if (!isBomb)
           {
               Attack();
           }
        }
    }
    void Delete()
    {
        Destroy(gameObject);
    }
}
