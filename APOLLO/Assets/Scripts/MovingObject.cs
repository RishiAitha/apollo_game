using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public GameObject movingObject1;
    public Transform pos1;
    public Transform pos2;
    public GameObject movingObject2;
    public Transform pos3;
    public Transform pos4;

    private Vector3 currentTargetPos1;
    private Vector3 currentTargetPos2;

    public bool moving;
    public float moveSpeed1;
    public float moveSpeed2;

    void Start()
    {
        moving = true;
        currentTargetPos1 = pos1.position;
        currentTargetPos2 = pos3.position;
    }

    private void Update()
    {
        if (GetComponent<PullController>() != null)
        {
            moving = !(GetComponent<PullController>().pulling || GetComponent<PullController>().aligning);
        }

        if (GetComponent<ZipController>() != null)
        {
            moving = !(GetComponent<ZipController>().pulling || GetComponent<ZipController>().aligning);
        }

        if (moving)
        {
            movingObject1.transform.position = Vector3.MoveTowards(movingObject1.transform.position, currentTargetPos1, moveSpeed1 * Time.deltaTime);
            if (Mathf.Abs(Vector3.Distance(movingObject1.transform.position, currentTargetPos1)) < 0.01f)
            {
                if (pos1.position == currentTargetPos1)
                {
                    currentTargetPos1 = pos2.position;
                }
                else
                {
                    currentTargetPos1 = pos1.position;
                }
            }

            if (movingObject2 != null)
            {
                movingObject2.transform.position = Vector3.MoveTowards(movingObject2.transform.position, currentTargetPos2, moveSpeed2 * Time.deltaTime);
                if (Mathf.Abs(Vector3.Distance(movingObject2.transform.position, currentTargetPos2)) < 0.01f)
                {
                    if (pos3.position == currentTargetPos2)
                    {
                        currentTargetPos2 = pos4.position;
                    }
                    else
                    {
                        currentTargetPos2 = pos3.position;
                    }
                }
            }
        }
    }
}
