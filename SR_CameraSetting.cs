using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SR_CameraSetting : MonoBehaviour
{
    Transform MainCameraPos;
    SR_CameraShake SR_cameraShake;
    public Toggle Tg_CameraSetting;

    public Transform NearCamera;
    public Transform FarCamera;

    float positionThreshold =0.1f;
    float rotationThreshold = 0.1f;
    [SerializeField]
    float speed;
    void Start()
    {
        MainCameraPos = Camera.main.transform;
        SR_cameraShake = Camera.main.gameObject.GetComponent<SR_CameraShake>();
        StartCoroutine(CameraSetting());
    }

    public void ChangeCameraSetting()
    {
        StopCoroutine(CameraSetting());
        StartCoroutine(CameraSetting());
    }
    IEnumerator CameraSetting()
    {
     
        while(true)
        {
            if (SR_cameraShake.isShakeing) { yield return null; }
            Vector3 newPos;
            Quaternion rotation;
            if (Tg_CameraSetting.isOn)
            {
                newPos = FarCamera.position;
                rotation = FarCamera.rotation;
            }
            else
            {
                newPos = NearCamera.position;
                rotation = NearCamera.rotation;
            }
            newPos.x = MainCameraPos.position.x;

            float positionMargin = Vector3.Distance(MainCameraPos.position, newPos);
            float rotationMargin = Quaternion.Angle(MainCameraPos.rotation, rotation);
            if (positionMargin > positionThreshold)
            {
                MainCameraPos.position = Vector3.Lerp(MainCameraPos.position, newPos, speed * Time.deltaTime);
            }
            if (rotationMargin > rotationThreshold)
            {
                MainCameraPos.rotation = Quaternion.Lerp(MainCameraPos.rotation, rotation, speed * Time.deltaTime);
            }
            if(positionMargin <= positionThreshold &&
               rotationMargin <= rotationThreshold) 
            { 
                yield break; 
            }
            yield return null;
        }
    }
}
