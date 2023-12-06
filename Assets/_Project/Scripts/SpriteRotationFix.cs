using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRotationFix : MonoBehaviour
{
    void Update()
    {
        transform.up = Vector2.up;
    }
}
