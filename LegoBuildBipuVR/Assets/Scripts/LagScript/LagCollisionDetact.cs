using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LagCollisionDetact : MonoBehaviour
{

    private static LagCollisionDetact _myInstance;

    public static LagCollisionDetact MyInstance
    {
        get
        {
            if (_myInstance == null)
                _myInstance = FindObjectOfType<LagCollisionDetact>();
            return _myInstance;
        }
    }

    private Vector3 _changePosition;
    private Transform _tr;
    private bool _activity;
    private Vector3 _stablePosition;
    private Vector3 _stableRotation;
    private bool _objectStabilityCheck;
    //[SerializeField]
    public GameObject FixedJoinMainObj;

    FixedJoint joint;

    public bool ObjectStabilityCheck
    {
        get { return _objectStabilityCheck; }
        set { _objectStabilityCheck = value; }
    }

    // Use this for initialization
    void Start()
    {
        FixedJoinMainObj = GameObject.Find("LegoSixMain");
        _stablePosition = new Vector3(-1, 100, -1);
    }


    private void OnTriggerStay(Collider other)
    {
        PositionCalculation(other);
        if (ControllerGrabObject.MyInstance.IsMouseDragging == false && other.gameObject.tag != "LL")
        {
            ControllerGrabObject.MyInstance.ChildObjDistroy();
        }
        if (ControllerGrabObject.MyInstance.IsMouseDragging)
        {
            ReleaseObject();
        }
    }

    private void PositionCalculation(Collider other)
    {
        //Transform tm = other.gameObject.transform.root.gameObject.transform;
        //if (tm.eulerAngles.y == 0 || tm.eulerAngles.y == 90 || tm.eulerAngles.y == 180 || tm.eulerAngles.y == 270)
        if (!ObjectStabilityCheck)
        {
            if (other.gameObject.name == "L1" || other.gameObject.name == "L2" || other.gameObject.name == "L3" || other.gameObject.name == "L4" || other.gameObject.name == "L5" || other.gameObject.name == "L6" || other.gameObject.name == "L7" || other.gameObject.name == "L8" /*|| other.gameObject.name == "M1" || other.gameObject.name == "M2" || other.gameObject.name == "M3" || other.gameObject.name == "M4"*/)
            {
                _tr = gameObject.transform.root.transform;
                _changePosition = gameObject.transform.position - other.gameObject.transform.position;
                _stablePosition = new Vector3(_tr.position.x - _changePosition.x, 0.25f + other.gameObject.transform.root.transform.position.y, _tr.position.z - _changePosition.z);
                //_tr.position = _stablePosition;
                float ang = DegreeCalculation(_tr);
                //_tr.eulerAngles = new Vector3(0, ang, 0);
                if (ControllerGrabObject.MyInstance.IsMouseDragging == false)
                {
                    _tr.eulerAngles = new Vector3(0, ang, 0);
                    _tr.position = _stablePosition;
                    ObjectStabilityCheck = true;
                    StartCoroutine(FixedObjectPosition(other));
                }
            }
        }
    }

    private float DegreeCalculation(Transform _tr)
    {
        float ang = 0;
        if ((_tr.eulerAngles.y >= 315 && _tr.eulerAngles.y <= 360) || (_tr.eulerAngles.y >= 0 && _tr.eulerAngles.y < 45))
            ang = 0;
        else if (_tr.eulerAngles.y >= 45 && _tr.eulerAngles.y < 135)
            ang = 90;
        else if (_tr.eulerAngles.y >= 135 && _tr.eulerAngles.y < 225)
            ang = 180;
        else if (_tr.eulerAngles.y >= 225 && _tr.eulerAngles.y < 315)
            ang = 270;
        return (ang);
    }

    private void ObjectFixedJoind()
    {

        if (!gameObject.transform.root.gameObject.GetComponent<FixedJoint>())
        {
            joint = AddFixedJoint();
            joint.connectedBody = FixedJoinMainObj.GetComponent<Rigidbody>();
        }
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.transform.root.gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;//Mathf.Infinity;
        fx.breakTorque = 20000; //Mathf.Infinity;
        return fx;
    }

    private void ReleaseObject()
    {
        if (gameObject.transform.root.gameObject.GetComponent<FixedJoint>())
        {
            gameObject.transform.root.gameObject.GetComponent<FixedJoint>().connectedBody = null;
            Destroy(gameObject.transform.root.gameObject.GetComponent<FixedJoint>());
            //objectInHand.GetComponent<Rigidbody>().velocity = Controller.velocity;
            //objectInHand.GetComponent<Rigidbody>().angularVelocity = Controller.angularVelocity;
        }
        //objectInHand = null;
    }

    IEnumerator FixedObjectPosition(Collider other)
    {
        int i = 0;
        while (i < 20)
        {
            if (other.gameObject.name == "L1" || other.gameObject.name == "L2" || other.gameObject.name == "L3" || other.gameObject.name == "L4" || other.gameObject.name == "L5" || other.gameObject.name == "L6" || other.gameObject.name == "L7" || other.gameObject.name == "L8" /*|| other.gameObject.name == "M1" || other.gameObject.name == "M2" || other.gameObject.name == "M3" || other.gameObject.name == "M4"*/)
            {
                _tr = gameObject.transform.root.transform;
                _changePosition = gameObject.transform.position - other.gameObject.transform.position;
                _stablePosition = new Vector3(_tr.position.x - _changePosition.x, 0.25f + other.gameObject.transform.root.transform.position.y, _tr.position.z - _changePosition.z);
                _tr.position = _stablePosition;
                float ang = DegreeCalculation(_tr);
                _tr.eulerAngles = new Vector3(0, ang, 0);
            }
            i++;
            ObjectFixedJoind();
            ControllerGrabObject.MyInstance.ChildObjDistroy();
        }
        yield return null;
    }
}
