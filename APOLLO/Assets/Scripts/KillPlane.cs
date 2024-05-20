using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public List<GameObject> overlappingTransitions;

    void Update()
    {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f, LayerMask.GetMask("Trigger"));
        overlappingTransitions.Clear();
        foreach (Collider2D overlap in overlaps)
        {
            if (overlap.gameObject.tag == "Transition")
            {
                if (!overlappingTransitions.Contains(overlap.gameObject))
                {
                    overlappingTransitions.Add(overlap.gameObject);
                }   
            }
        }
    }
}
