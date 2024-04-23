using UnityEngine;

public class SR_Bullet : MonoBehaviour
{
    public enum OwnerType {Army,Enemy}
    OwnerType ownerType;
    public GameObject HitEffect;
    public Transform hitEfeectPos;
    float damage = 0.0f;
    float critical = 0f;
    public void Initialize(OwnerType ownerType, float damage,float critical)
    {
        this.ownerType = ownerType;
        this.damage = damage;
        this.critical = critical;
        Invoke("DestroyBullet", 1);
    }
    private void OnTriggerEnter(Collider other)
    {
        bool isCrash = false;

        if (ownerType == OwnerType.Army && (other.tag == "Enemy" || other.tag == "DestroyWall"))
        {
            isCrash = true;
            if (other.GetComponent<SR_Enemy>())
            {
                other.GetComponent<SR_Enemy>().Attacked(damage);
            }
            if (other.GetComponent<SR_PlayerObj>())
            {
                other.GetComponent<SR_PlayerObj>().Attacked(damage);
            }
        }
        if (ownerType == OwnerType.Enemy && (other.tag == "Army" || other.tag == "TargetSpawnObj" || other.tag == "NoTargetSpawnObj" || other.tag == "DestroyWall"))
        {   
            isCrash = true;
            other.GetComponent<SR_PlayerObj>().Attacked(damage);
        }
        if (other.tag == "Base") { isCrash = true; }
        if (isCrash)
        {           
            Instantiate(HitEffect, hitEfeectPos.position, hitEfeectPos.rotation);
            CancelInvoke("DestroyBullet");
            DestroyBullet();
        }
    }
    bool CheckCriticalHit()
    {
        return Random.Range(0.0f, 100.0f) < critical;
    }
    void DestroyBullet()
    {
        gameObject.SetActive(false);
    }
}
