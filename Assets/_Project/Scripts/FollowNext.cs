using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowNext : MonoBehaviour
{
    [SerializeField] GameObject next;
    [SerializeField] float speed;

    void Update()
    {
        Vector3 followTarget = Vector3.zero;

        if (next == null)
            followTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        else
            followTarget = next.transform.position;

        Vector3 diff = followTarget - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        followTarget = new Vector3(followTarget.x, followTarget.y, 0);

        if(next == null)
            transform.position = Vector3.Lerp(transform.position, (followTarget), Time.deltaTime * (speed / Vector3.Distance(transform.position, followTarget)));
        else
            transform.position = Vector3.Lerp(transform.position, (followTarget - next.transform.up / 2f), Time.deltaTime * (speed / Vector3.Distance(transform.position, followTarget)));
    }
}
