using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] DynamicJoystick joystick;

    void Update()
    {
#if UNITY_ANDROID

        print(joystick.Direction);

        transform.position = (Vector2)player.position - joystick.Direction * 10f;


#endif
#if UNITY_STANDALONE_WIN
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
#endif
    }
}
