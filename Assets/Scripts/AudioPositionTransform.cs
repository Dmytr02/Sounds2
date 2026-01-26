using System;
using FMODUnity;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioPositionTransform : MonoBehaviour
{
    [SerializeField] public Vector3 _size = Vector3.one;
    [SerializeField] public Vector3 _position = Vector3.one;
    [SerializeField] public Quaternion _rotaton = Quaternion.identity;
    [SerializeField] private StudioListener listener;
    [SerializeField] StudioEventEmitter emitter;
    [SerializeField] eObjType objType;
    void Start()
    {
        
    }
    
    void Update()
    {
        var pos = GetPosition();
        emitter.transform.position = pos.pos;
    }

    public (Vector3 pos, float dist) GetPosition()
    {
        (Vector3 pos, float dist) result = (Vector3.zero, float.PositiveInfinity);
        
        Matrix4x4 matrix = transform.localToWorldMatrix * Matrix4x4.TRS(_position, _rotaton, _size/2);
        
        Vector3 localPos = matrix.inverse.MultiplyPoint3x4(listener.transform.position);

        switch (objType)
        {
            case eObjType.sphere:
                float dist = localPos.magnitude;
                if (dist > 1) result.pos = matrix.MultiplyPoint3x4(localPos.normalized);
                else result.pos = listener.transform.position;
                break;
            case eObjType.cube:
                localPos = new Vector3(Mathf.Clamp(localPos.x, -1, 1), Mathf.Clamp(localPos.y, -1, 1), Mathf.Clamp(localPos.z, -1, 1));
                
                result.pos = matrix.MultiplyPoint3x4(localPos);
                break;
        }
        
        result.dist = (listener.transform.position - result.pos).magnitude;
        return result;
    }
    private void OnDrawGizmos()
    {
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale) * Matrix4x4.TRS(_position, _rotaton, _size);
        switch (objType)
        {
            case eObjType.sphere:
                Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
                break;
            case eObjType.cube:
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                break;
        }
    }
}
[CustomEditor(typeof(AudioPositionTransform))]
public class AudioPositionTransformEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        AudioPositionTransform t = (AudioPositionTransform)target;
        EditorGUI.BeginChangeCheck();
        
        Vector3 pos = t.transform.TransformPoint(t._position);
        Vector3 newPos = Handles.PositionHandle(pos, t._rotaton);
        
        Quaternion rot = t._rotaton;
        Quaternion newRot = Handles.RotationHandle(rot, pos);
        
        float size = HandleUtility.GetHandleSize(pos)*0.5f;
        Vector3 newScale = Handles.ScaleHandle(t._size, pos, rot, size);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Change Audio Position");
            t._position = t.transform.InverseTransformPoint(newPos);
            t._size = newScale;
            t._rotaton = newRot;
        }
    }
}

public enum eObjType
{
    sphere,
    cube
}