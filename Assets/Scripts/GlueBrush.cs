using UnityEngine;
using System.Collections;

public class GlueBrush : MonoBehaviour
{
    [SerializeField]
    private CollisionDelegator _paintBrush;

    [SerializeField]
    private GlueDab _glueDabPrefab;

    public bool madeGlue = false;

    // Use this for initialization
    void Awake()
    {
        _paintBrush.OnCollisionEnterDel += OnBrushDab;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnBrushDab(object other)
    {
        Collision collision = (Collision)other;
        if(collision != null)
        {
            GlueDab otherDab = collision.gameObject.GetComponent<GlueDab>();
            if(otherDab == null && !madeGlue)
            {
                madeGlue = true;

                foreach(ContactPoint contactPoint in collision.contacts)
                {
                    GlueDab newDab = Instantiate(_glueDabPrefab, contactPoint.point, Quaternion.identity);

                    newDab.FirstGlueJoint.connectedBody = collision.rigidbody;
                }
            }
        }
    }
}
