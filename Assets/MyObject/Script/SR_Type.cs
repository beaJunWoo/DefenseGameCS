using DG.Tweening;
using UnityEngine;

public class SR_Type : MonoBehaviour
{
    string[] Btn_Names = { "Btn_ArmyType", "Btn_BarrierType" };
    int Onidx = -1;
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
            {
                if (Onidx >= 0)
                {
                    DOTween.Restart("Off" + Btn_Names[Onidx]);
                    Onidx = -1;
                }
            }
            else
            {

                string Btn_Name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
                if (Onidx >= 0)
                {
                    int idx = Onidx;
                    for (int i = 0; i < Btn_Names.Length; i++)
                    {
                        if (Btn_Name == Btn_Names[i])
                        {
                            Onidx = i;
                            break;
                        }

                    }
                    if (idx != Onidx)
                    {

                        DOTween.Restart("Off" + Btn_Names[idx]);
                        DOTween.Restart("On" + Btn_Names[Onidx]);
                    }
                }
                else
                {
                    for (int i = 0; i < Btn_Names.Length; i++)
                    {
                        if (Btn_Name == Btn_Names[i])
                        {
                            Onidx = i;
                            break;
                        }

                    }
                    if (Onidx > -1)
                    {
                        DOTween.Restart("On" + Btn_Names[Onidx]);
                    }

                }
            }
        }
    }
}
