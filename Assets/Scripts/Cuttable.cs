using BzKovSoft.ObjectSlicer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
    IBzSliceableAsync _sliceableAsync;

    void Start()
    {
        _sliceableAsync = GetComponentInParent<IBzSliceableAsync>();
    }

    private void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        var knife = other.gameObject.GetComponent<CuttingTool>();
        if (knife == null)
            return;

        if(knife.IsSwinging)
        {
            return;
        }

        StartCoroutine(Slice(knife));
    }

    private IEnumerator Slice(CuttingTool knife)
    {
        // The call from OnTriggerEnter, so some object positions are wrong.
        // We have to wait for next frame to work with correct values
        yield return null;

        Vector3 point = GetCollisionPoint(knife);
        Vector3 normal = Vector3.Cross(knife.MoveDirection, knife.BladeDirection);
        Plane plane = new Plane(normal, point);

        if (_sliceableAsync != null)
        {
            knife.BeginNewSlice();

            _sliceableAsync.Slice(plane, knife.SliceID, null);
        }
    }

    private Vector3 GetCollisionPoint(CuttingTool knife)
    {
        Vector3 distToObject = transform.position - knife.Origin;
        Vector3 proj = Vector3.Project(distToObject, knife.BladeDirection);

        Vector3 collisionPoint = knife.Origin + proj;
        return collisionPoint;
    }
}
