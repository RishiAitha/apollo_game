using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightActivation : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x < Camera.main.transform.position.x + (Camera.main.orthographicSize * (Screen.width / Screen.height))
            && transform.position.x > Camera.main.transform.position.x - (Camera.main.orthographicSize * (Screen.width / Screen.height))
            && transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize
            && transform.position.y > Camera.main.transform.position.y - Camera.main.orthographicSize)
        {
            if (!GetComponent<Light2D>().enabled)
            {
                GetComponent<Light2D>().enabled = true;
            }
        }
        else
        {
            if (GetComponent<Light2D>().enabled)
            {
                GetComponent<Light2D>().enabled = false;
            }
        }
    }
}
