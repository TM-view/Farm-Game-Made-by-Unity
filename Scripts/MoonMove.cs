using UnityEngine;

public class MoonMove : MonoBehaviour
{
    Vector3[] pos = {   
                        new Vector3(-25, 0, 10), // 0AM
                        new Vector3(-16, 0, 10), // 2AM 
                        new Vector3(-14.5f, 1.5f, 10), // 4AM
                        new Vector3(-13, 3, 10), // 6AM
                        new Vector3(-11.5f, 4.5f, 10), // 8AM
                        new Vector3(-10, 6, 10), // 10AM
                        new Vector3(-8.5f, 7, 10), // 12AM
                        new Vector3(-7, 8f, 10), // 2PM
                        new Vector3(-5.5f, 9.5f, 10), // 4PM
                        new Vector3(-2, 8, 10), // 6PM Night
                        new Vector3(0, 6.5f, 10), // 8 PM
                        new Vector3(2, 5, 10), //10PM
                        new Vector3(4, 3.5f, 10), //12PM
                    };
    
Color32[] sky = {
                    new Color32(0x22, 0x26, 0x26, 255),
                    new Color32(0x3B, 0x49, 0x4D, 255),
                    new Color32(0x4A, 0x70, 0x79, 255),
                    new Color32(0x4E, 0x96, 0xA5, 255),
                    new Color32(0x3F, 0xB3, 0xCC, 255),
                    new Color32(0x00, 0xD4, 0xFF, 255),
                    new Color32(0x43, 0xE0, 0xFF, 255), 
                    new Color32(0x55, 0xB2, 0xC6, 255), 
                    new Color32(0x49, 0x95, 0xA5, 255), 
                    new Color32(0x01, 0x3C, 0x49, 255), 
                    new Color32(0x06, 0x1D, 0x22, 255), 
                    new Color32(0x00, 0x11, 0x11, 255), 
                    new Color32(0x00, 0x00, 0x00, 255)
                };

    public void Move(int time)
    {
        transform.parent.GetComponent<Camera>().backgroundColor = sky[time];
        transform.position = pos[time];
    }

    void Start()
    {
        transform.parent.GetComponent<Camera>().backgroundColor = sky[0];
        transform.position = pos[0];
    }
}
