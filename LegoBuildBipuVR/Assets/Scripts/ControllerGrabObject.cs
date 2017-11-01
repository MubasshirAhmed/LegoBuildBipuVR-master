using System;
using UnityEngine;

public class ControllerGrabObject : MonoBehaviour
{

    private static ControllerGrabObject _myInstance;

    public static ControllerGrabObject MyInstance
    {
        get
        {
            if (_myInstance == null)
                _myInstance = FindObjectOfType<ControllerGrabObject>();
            return _myInstance;
        }
    }

    private SteamVR_TrackedObject trackedObj;
    private GameObject collidingObject;
    private GameObject objectInHand;
    private bool _isMouseDragging;

    [SerializeField]
    private GameObject[] _childObj;
    private GameObject _mainChild;
    private Vector3 _childPosition;
    private Quaternion _childRotation;

    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    void Awake()
    {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }


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
        if (Controller.GetHairTriggerDown())
        {
            if (collidingObject)
            {
                IsMouseDragging = true;
                GrabObject();
                if (objectInHand)
                    ObjAdd();
            }
        }

        if (Controller.GetHairTriggerUp())
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
            objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
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
        print(objectInHand.tag);
        int index = 0;
        if (objectInHand.tag.Equals("Lego_L4"))
            index = 0;
        else if (objectInHand.tag.Equals("Lego_L8"))
            index = 1;

        if (objectInHand != null && _mainChild == null && IsMouseDragging)
        {
            _mainChild = Instantiate(_childObj[index]);
            _mainChild.transform.parent = objectInHand.transform;
            _mainChild.transform.localPosition = _childPosition;
            _mainChild.transform.localRotation = _childRotation;
            if (index == 0)
                _mainChild.transform.localScale = new Vector3(1, 4, 1);
            else if (index == 1)
                _mainChild.transform.localScale = new Vector3(0.5f, 4, 1);
        }
    }

    public void ChildObjDistroy()
    {
        Destroy(_mainChild);
    }
}
