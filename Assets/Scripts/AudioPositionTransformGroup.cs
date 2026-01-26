using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class AudioPositionTransformGroup : MonoBehaviour
{
    [SerializeField] List<AudioPositionTransform> audioPositionTransforms = new List<AudioPositionTransform>();
    [SerializeField] StudioEventEmitter emitter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var audioPositionTransform in audioPositionTransforms)
        {
            audioPositionTransform.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        (Vector3 pos, float dist) pos = (Vector3.zero, float.PositiveInfinity);
        foreach (var audioPositionTransform in audioPositionTransforms)
        {
            (Vector3 pos, float dist) pos0 = audioPositionTransform.GetPosition();
            if (pos0.dist < pos.dist)
            {
                pos = pos0;
            }
        }
        emitter.transform.position = pos.pos;
    }
}
