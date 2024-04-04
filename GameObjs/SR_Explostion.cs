
using UnityEngine;

public class SR_Explostion : MonoBehaviour
{
    [SerializeField]
    float maxLenght;
    float defaultDamage =30;
    private void OnTriggerEnter(Collider other)
    {
        float lenght = Vector3.Distance(other.gameObject.transform.position, transform.position);
        Debug.Log(lenght);
        float percentDamage = (maxLenght - lenght)/maxLenght;
        if (other.tag == "Army")
        {
            other.GetComponent<SR_PlayerObj>().Attacked(percentDamage*defaultDamage);
        }
        if(other.tag == "Base")
        {
            other.GetComponent<SR_MilitaryBase>().Attacked(15);
        }
    }
}
