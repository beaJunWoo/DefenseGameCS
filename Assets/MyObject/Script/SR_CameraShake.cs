using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SR_CameraShake : MonoBehaviour
{
    public float defualtForce =50f;
    [SerializeField]
    public float force = 0f;
    public float speed = 500f;
    public Vector3 offset = Vector3.zero;

    Quaternion originRot;

    public bool isShakeing = false;

    private void Update()
    {
       // if(Input.GetMouseButtonDown(0))
       // {
       //     StartShake();
       // }
    }
    public void StartShake()
    {
        isShakeing = true;
        originRot = transform.rotation;
        force = defualtForce;
        StartCoroutine(ShakeCoroutine());
        Invoke("StopShake",0.5f);
    }
    void StopShake()
    {
        StopAllCoroutines();
        StartCoroutine(Reset());
        Invoke("ShakeFinsh",0.5f);
    }
    void ShakeFinsh()
    {
        StopAllCoroutines();
        isShakeing = false;
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 originEuler = transform.eulerAngles;
        while (true)
        {
            force -= Time.deltaTime*speed;
            float rotX = Random.Range(-offset.x, offset.x);
            float rotY = Random.Range(-offset.y, offset.y);
            float rotZ = Random.Range(-offset.z, offset.z);
            
            Vector3 RandomRot = originEuler + new Vector3(rotX, rotY, rotZ);
            Quaternion rot = Quaternion.Euler(RandomRot);

            while(Quaternion.Angle(transform.rotation, rot) > 0.5f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, force * Time.deltaTime);
                yield return null;
            }
            yield return null;
        }
    }

    private IEnumerator Reset()
    {
        while (true)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, originRot, force * Time.deltaTime);
            yield return null;
        }
    }
}
