using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObjectGrab : MonoBehaviour
{
    private static PhysicsObjectGrab _myInstance;

    public static PhysicsObjectGrab MyInstance
    {
        get
        {
            if (_myInstance == null)
                _myInstance = FindObjectOfType<PhysicsObjectGrab>();
            return _myInstance;
        }
    }

    private GameObject collidingObject;
    private GameObject objectInHand;
    private bool _isMouseDragging;

    [SerializeField]
    private GameObject _childObj;
    private GameObject _mainChild;
    private Vector3 _childPosition;
    private Quaternion _childRotation;

    public void OnTriggerEnter(Collider other)
    {
        SetCollidingObject(other);
    }

    public void OnTriggerStay(Collider other)
    {

        SetCollidingObject(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!collidingObject)
        {
            return;
        }

        collidingObject = null;
    }

    private void SetCollidingObject(Collider col)
    {
        if (collidingObject || !col.GetComponent<Rigidbody>())
        {
            return;
        }

        collidingObject = col.gameObject;
    }

    private void Start()
    {
        _childPosition = Vector3.zero;
        _childRotation = Quaternion.identity;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (collidingObject)
            {
                IsMouseDragging = true;
                GrabObject();
                if (objectInHand)
                    ObjAdd();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (objectInHand)
            {
                IsMouseDragging = false;
                ReleaseObject();
            }
        }
    }

    private void GrabObject()
    {
        objectInHand = collidingObject;
        collidingObject = null;

        var joint = AddFixedJoint();
        joint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }


    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject()
    {
        if (GetComponent<FixedJoint>())
        {
            GetComponent<FixedJoint>().connectedBody = null;
            Destroy(GetComponent<FixedJoint>());

            objectInHand.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        }
        objectInHand = null;
    }

    public bool IsMouseDragging
    {
        get { return _isMouseDragging; }
        set { _isMouseDragging = value; }
    }

    private void ObjAdd()
    {
        if (objectInHand != null && _mainChild == null && IsMouseDragging)
        {
            _mainChild = Instantiate(_childObj);
            _mainChild.transform.parent = objectInHand.transform;
            _mainChild.transform.localPosition = _childPosition;
            _mainChild.transform.localRotation = _childRotation;
        }
    }

    public void ChildObjDistroy()
    {
        Destroy(_mainChild);
    }
}
