using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nail : MonoBehaviour
{
    [SerializeField]
    private CollisionDelegator _nailTip = null;

    [SerializeField]
    private CollisionDelegator _nailHead = null;

    [SerializeField]
    private Collider _nailBodyCollider = null;

    private Rigidbody _rigidBody = null;

    private const float _penetrationDepth = 0.05f;

    private List<Collider> _currentConnectedColliders = new List<Collider>();

	// Use this for initialization
	void Start ()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _nailTip.OnTriggerEnterDel += OnNailTipTrigger;

        _nailHead.OnTriggerEnterDel += OnNailHeadTrigger;
    }

    void OnNailTipTrigger(object other)
    {
        Collider otherCollider = (Collider)other;
        if(otherCollider != null && !_currentConnectedColliders.Contains(otherCollider))
        {
            Rigidbody otherRigidBody = otherCollider.GetComponent<Rigidbody>();
            if(otherRigidBody)
            {
                FixedJoint fixJoint = AddFixedJoint(gameObject);
                fixJoint.connectedBody = otherRigidBody;

                _currentConnectedColliders.Add(otherCollider);

                _nailBodyCollider.enabled = false;
            }
        }
    }

    void OnNailHeadTrigger(object other)
    {
        Collider otherCollider = (Collider)other;
        if (otherCollider != null)
        {
            Rigidbody otherRigidBody = otherCollider.GetComponent<Rigidbody>();
            if (otherRigidBody)
            {
                _rigidBody.isKinematic = true;

                transform.position += (-_nailHead.transform.up) * _penetrationDepth;
            }
        }
    }

    private FixedJoint AddFixedJoint(GameObject toAddTo)
    {
        FixedJoint fx = toAddTo.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
