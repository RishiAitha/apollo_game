using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{
    public float cooldownTime;

    public void Cooldown()
    {
        StartCoroutine("CooldownAnim");
    }

    public IEnumerator CooldownAnim()
    {
        GetComponent<Collider2D>().enabled = false;
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(cooldownTime / 10f);
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(cooldownTime / 10f);
            GetComponent<SpriteRenderer>().enabled = true;
        }
        GetComponent<Collider2D>().enabled = true;
    }
}
