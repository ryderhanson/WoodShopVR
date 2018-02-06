using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTool : MonoBehaviour
{
    public int SliceID { get; private set; }
    Vector3 _prevPos;
    Vector3 _pos;

    [SerializeField]
    private Vector3 _origin = -Vector3.forward;

    [SerializeField]
    private Vector3 _direction = Vector3.forward;

    private void Update()
    {
        _prevPos = _pos;
        _pos = transform.position;

        if(IsSwinging && MoveVelocity < 0.00001f)
        {
            IsSwinging = false;
        }
    }

    public Vector3 Origin
    {
        get
        {
            Vector3 localShifted = transform.InverseTransformPoint(transform.position) + _origin;
            return transform.TransformPoint(localShifted);
        }
    }

    public Vector3 BladeDirection { get { return transform.rotation * _direction.normalized; } }
    public Vector3 MoveDirection { get { return (_pos - _prevPos).normalized; } }
    public float MoveVelocity { get { return (_pos - _prevPos).magnitude; } }
    public bool IsSwinging { get; private set; }

    public void BeginNewSlice()
    {
        IsSwinging = true;

        SliceID = System.Guid.NewGuid().GetHashCode();
    }
}