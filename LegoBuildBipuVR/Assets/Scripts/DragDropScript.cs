using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDropScript : MonoBehaviour
{

    private static DragDropScript _myInstance;

    public static DragDropScript MyInstance
    {
        get
        {
            if (_myInstance == null)
                _myInstance = FindObjectOfType<DragDropScript>();
            return _myInstance;
        }
    }

    //Initialize Variables
    GameObject getTarget;
    bool isMouseDragging;
    Vector3 offsetValue;
    Vector3 positionOfScreen;

    [SerializeField]
    private GameObject _childObj;
    private GameObject _mainChild;
    private Vector3 _childPosition;
    private Quaternion _childRotation;

    public bool IsMouseDragging
    {
        get { return isMouseDragging; }
        set { isMouseDragging = value; }
    }

    private void Start()
    {
        _childPosition = Vector3.zero;
        _childRotation = Quaternion.identity;
    }

    void Update()
    {

        //Mouse Button Press Down
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hitInfo;
            getTarget = ReturnClickedObject(out hitInfo);
            if (getTarget != null)
            {
                IsMouseDragging = true;
                ObjAdd();
                //Converting world position to screen position.
                positionOfScreen = Camera.main.WorldToScreenPoint(getTarget.transform.position);
                offsetValue = getTarget.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z));
            }
        }

        //Mouse Button Up
        if (Input.GetMouseButtonUp(0))
        {
            IsMouseDragging = false;
        }

        //Is mouse Moving
        if (IsMouseDragging)
        {
            //tracking mouse position.
            Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, positionOfScreen.z);

            //converting screen position to world position with offset changes.
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offsetValue;

            //It will update target gameobject's current postion.
            getTarget.transform.position = currentPosition;
        }
    }

    //Method to Return Clicked Object
    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (hit.collider.tag.Equals("Lego"))
                target = hit.collider.gameObject;
        }
        return target;
    }

    private void ObjAdd()
    {
        if (getTarget != null && _mainChild == null && IsMouseDragging)
        {
            _mainChild = Instantiate(_childObj);
            _mainChild.transform.parent = getTarget.transform;
            _mainChild.transform.localPosition = _childPosition;
            _mainChild.transform.localRotation = _childRotation;
            _mainChild.transform.localScale = new Vector3(1, 4, 1);
        }
    }

    public void ChildObjDistroy()
    {
        Destroy(_mainChild);
    }
}