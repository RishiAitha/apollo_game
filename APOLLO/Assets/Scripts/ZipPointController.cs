using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipPointController : MonoBehaviour
{
    public float cooldownTime;

    public bool cooldown;

    public void Cooldown()
    {
        StartCoroutine("CooldownAnim");
    }

    public IEnumerator CooldownAnim()
    {
        cooldown = true;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(cooldownTime / 10f);
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(cooldownTime / 10f);
            GetComponent<SpriteRenderer>().enabled = true;
        }
        cooldown = false;
    }
}
