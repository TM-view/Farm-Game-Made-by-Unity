using UnityEngine;

public class BG_setting_control : MonoBehaviour
{
    public GameObject BG_setting;
    void Update()
    {
        if (BG_setting.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameObject se = BG_setting.transform.Find("setting_exit").gameObject;
                se.GetComponent<StartMenu>().SelectButton();
            }       
        }
    }
}
