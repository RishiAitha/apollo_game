using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{
    public float cooldownTime = 3f;

    public void Cooldown()
    {
        StartCoroutine("CooldownAnim");
    }

    public IEnumerator CooldownAnim()
    {
        GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(cooldownTime / 20f);
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(cooldownTime / 20f);
            GetComponent<SpriteRenderer>().enabled = true;
        }
        GetComponent<Collider2D>().enabled = true;
    }
}