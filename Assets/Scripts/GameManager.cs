using Lean.Touch;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Button _resetButton;
    [SerializeField] private Button _ragdollBtn;
    [SerializeField] private RagdollControll _ragdollControll;
    [SerializeField] private Camera _camera;
    [SerializeField] private IInteract _objectSelected;
    private Vector3 offset;
    private Vector3 screenPoint;
    [SerializeField] private Transform _transformSelect;
    private RaycastHit hit;
    private bool _canDragging = false;
    private bool _isDragging = false;
    [SerializeField] private float _forceImpact;
    private Rigidbody _rb;
    private float mass;

    private void Start()
    {
        // _resetButton.onClick.AddListener(_ragdollControll.AnimatorState);
        _ragdollBtn.onClick.AddListener(() => { _ragdollControll.SetStateRagdoll(false); });
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerUpdate += OnfingerUpdate;
        LeanTouch.OnFingerUp += OnfingerUp;
        LeanTouch.OnFingerDown += OnfingerDown;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerUpdate -= OnfingerUpdate;
        LeanTouch.OnFingerUp -= OnfingerUp;
        LeanTouch.OnFingerDown -= OnfingerDown;
    }

    private void OnfingerUpdate(LeanFinger finger)
    {
        if (finger.IsOverGui == true)
        {
            Debug.Log("OVer UI ");
            return;
        }
        Ray ray = _camera.ScreenPointToRay(finger.ScreenPosition);

        int layer = 1 << LayerMask.NameToLayer("Doll");

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            _transformSelect = hit.collider.transform;
            _objectSelected = hit.transform.GetComponentInParent<IInteract>();
            // Debug.Log("Hit object" + hit.transform.name);
        }

        if (_canDragging)
        {
            OnfingerDrag(finger);
        }
    }

    private void OnfingerDown(LeanFinger finger)
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        if (_transformSelect != null)
        {
            _rb = _transformSelect.GetComponent<Rigidbody>();
        }
        if (_rb != null)
        {
            mass = _rb.mass;
            Debug.Log($"Fingerdown Mass {mass} + _rb.mass {_rb.mass} ");
        }
        _canDragging = true;
        _isDragging = true;
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, screenPoint.z));
    }

    private void OnfingerDrag(LeanFinger finger)
    {
        if (_transformSelect != null)
        {
            // Vector3 currentPosition = _camera.ScreenToWorldPoint(finger.ScreenPosition) + offset;
            Vector3 curScreenPoint = new Vector3(finger.ScreenPosition.x, finger.ScreenPosition.y, screenPoint.z);
            Vector3 curPosition = _camera.ScreenToWorldPoint(curScreenPoint) + offset;
            curPosition = Vector3.Normalize(curPosition);
              Debug.Log($"currentPosition {curPosition} offset {offset} magnitude {offset.magnitude}");
            if (_isDragging == true && _rb != null)
            {
                _rb.mass = 75;
                Debug.Log("RbMass " + _rb.mass);
                _isDragging = false;
            }
              _rb.AddForceAtPosition(curPosition * (offset.magnitude), screenPoint,ForceMode.Impulse);
            //_rb.mass = mass;
            //  _objectSelected.OnDrag(curPosition);
            // LeanDragCamera.
        }
    }

    private void OnfingerUp(LeanFinger finger)
    {
        _isDragging = false;
        _canDragging = false;
        if ((_rb != null))
        {
            _rb.mass = mass;
            Debug.Log($"FingerUp Mass {mass} + _rb.mass {_rb.mass} ");

            _rb = null;
        }
        _objectSelected = null;
        _transformSelect = null;
        // _objectSelected.OnRelease();
    }

    private void Update()
    {
    }

    private void OnSelectObj(LeanFinger finger)
    {
        if (finger.IsOverGui)
        {
            return;
        }
    }
}