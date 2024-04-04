using UnityEngine;

public class SK_SkillofType : MonoBehaviour
{
    public GameObject Bomb;
    public float speed = 5.0f;
    public float descentSpeed = 2000;

    Vector3 pos  = new Vector3(9999,0,0);
    bool isFire = false;

    void Start()
    {
        SR_SoundManager.instance.PlaySfx(0.3f,SR_SoundManager.Sfx.Dron,0.2f);
        Invoke("Destroy", 5);
    }
    void FixedUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.x += Time.deltaTime * speed;
        gameObject.transform.position = newPos;
    }
    private void Update()
    {
        if (pos.x <= transform.position.x&&!isFire)
        {
            Debug.Log(pos.x);
            GameObject bomb = Instantiate(Bomb);
            Vector3 newPos = transform.position;
            newPos.x = pos.x;
            bomb.transform.position = newPos;
            bomb.GetComponent<Rigidbody>().AddForce(Vector3.down * descentSpeed);
            isFire = true;
        }
    }
    void Destroy()
    {
        Destroy(gameObject);
    }
    public void SetBombPos(Vector3 pos)
    {
        this.pos = pos;
    }
}
