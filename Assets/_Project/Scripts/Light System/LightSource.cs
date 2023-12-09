using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] public float range;

    private void OnDisable()
    {
        PixelLighting.Instance.lightSources.Remove(this);
    }
}
