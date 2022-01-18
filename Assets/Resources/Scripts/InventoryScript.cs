using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    GameObject selectedGameObject;
    [SerializeField] private GameObject transformPrefab;
    [SerializeField] private GameObject rotationPrefab;
    [SerializeField] private GameObject scalePrefab;

    public void SpawnObject(GameObject gameObject)
    {
        selectedGameObject = Instantiate(gameObject, Vector3.Scale(Camera.main.transform.position, Vector3.right + Vector3.up), Quaternion.identity, GameObject.Find("Boxes").transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private Vector2 GetMousePosition()
    {
        return Vector3.Scale(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.right + Vector3.up);
    }

    private bool CheckForColliding(Transform colliderObject)
    {
        if((GetMousePosition() - (Vector2)colliderObject.transform.position).magnitude <= 2.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void MoveSelectedToMouse()
    {
        selectedGameObject.transform.position = (Vector3)GetMousePosition() + Vector3.forward * selectedGameObject.transform.position.z;
    }

    private void RotateSelectedToMouse()
    {
        Vector2 direction = ((Vector2)selectedGameObject.transform.position - GetMousePosition()).normalized;
        selectedGameObject.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
        rotationGizmo.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg));
    }

    private void ScaleSelectedToMouse()
    {
        selectedGameObject.transform.localScale += Input.GetAxis("Mouse X") * Vector3.right;
        selectedGameObject.transform.localScale += Input.GetAxis("Mouse Y") * Vector3.up;
    }

    private GameObject transformGizmo;

    private GameObject rotationGizmo;

    private GameObject scaleGizmo;

    private enum GizmoType {Transform, Rotation, Scale};

    private GizmoType gizmoType = GizmoType.Transform;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach(Transform child in GameObject.Find("Boxes").transform)
            {
                if (child.parent.name == "Boxes" && CheckForColliding(child))
                {
                    selectedGameObject = child.gameObject;
                    if(gizmoType == GizmoType.Transform)
                    {
                        transformGizmo = Instantiate(transformPrefab, (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z, Quaternion.identity);
                    }
                    else if (gizmoType == GizmoType.Rotation)
                    {
                        rotationGizmo = Instantiate(rotationPrefab, (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z, Quaternion.identity);
                    }
                    else if (gizmoType == GizmoType.Scale)
                    {
                        scaleGizmo = Instantiate(scalePrefab, (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z, Quaternion.identity);
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedGameObject = null;
            Destroy(transformGizmo);
            Destroy(rotationGizmo);
            Destroy(scaleGizmo);
        }
        if (Input.GetMouseButton(0) && selectedGameObject != null && gizmoType == GizmoType.Transform)
        {
            MoveSelectedToMouse();
            transformGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z;
        }
        else if(Input.GetMouseButton(0) && selectedGameObject != null && gizmoType == GizmoType.Rotation)
        {
            RotateSelectedToMouse();
            rotationGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z;
        }
        else if (Input.GetMouseButton(0) && selectedGameObject != null && gizmoType == GizmoType.Scale)
        {
            ScaleSelectedToMouse();
            scaleGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            gizmoType = GizmoType.Transform;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            gizmoType = GizmoType.Rotation;
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            gizmoType = GizmoType.Scale;
        }
    }
}
