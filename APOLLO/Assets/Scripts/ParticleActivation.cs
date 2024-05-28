using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivation : MonoBehaviour
{
    void Update()
    {
        if (transform.position.x < Camera.main.transform.position.x + (Camera.main.orthographicSize * (Screen.width / Screen.height))
            && transform.position.x > Camera.main.transform.position.x - (Camera.main.orthographicSize * (Screen.width / Screen.height))
            && transform.position.y < Camera.main.transform.position.y + Camera.main.orthographicSize
            && transform.position.y > Camera.main.transform.position.y - Camera.main.orthographicSize)
        {
            if (!GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            if (GetComponent<ParticleSystem>().isPlaying)
            {
                GetComponent<ParticleSystem>().Pause();
            }
        }
    }
}
