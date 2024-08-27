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
    private bool _isDragging = false;
    [SerializeField] private float _forceImpact;
    private Rigidbody _rb;

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
            _rb = hit.transform.GetComponent<Rigidbody>();
            _transformSelect = hit.collider.transform;
            _objectSelected = hit.transform.GetComponentInParent<IInteract>();
            Debug.Log("Hit object" + hit.transform.name);
        }

        if (_isDragging)
        {
            Debug.Log(" _isDragging");

            OnfingerDrag(finger);
        }
    }

    private void OnfingerDown(LeanFinger finger)
    {
        _isDragging = true;
        //  offset = _camera.ScreenToWorldPoint(_transformSelect.position) - _camera.ScreenToWorldPoint(finger.ScreenPosition);
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    private void OnfingerDrag(LeanFinger finger)
    {
        if (_transformSelect != null)
        {
            // Vector3 currentPosition = _camera.ScreenToWorldPoint(finger.ScreenPosition) + offset;
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            curPosition = Vector3.Normalize( curPosition );
            Debug.Log($"currentPosition {curPosition} offset {offset}");
            
            _rb.AddForceAtPosition(curPosition * _forceImpact, screenPoint, ForceMode.Acceleration);
            //  _objectSelected.OnDrag(curPosition);
            // LeanDragCamera.
        }
    }

    private void OnfingerUp(LeanFinger finger)
    {
        _objectSelected = null;
        _isDragging = false;
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