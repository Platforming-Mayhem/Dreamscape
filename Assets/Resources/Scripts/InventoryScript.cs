using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    GameObject selectedGameObject;
    [SerializeField] private GameObject transformPrefab;
    [SerializeField] private GameObject rotationPrefab;
    [SerializeField] private GameObject scalePrefab;
    [SerializeField] private GameObject binPrefab;
    [SerializeField] private GameObject originPrefab;
    [SerializeField] private GameObject levelMapEditor;

    [SerializeField] private GameObject makeSureDelete;

    private GenerateLevelFromFile generate;


    public void SpawnObject(GameObject gameObject)
    {
        selectedGameObject = Instantiate(gameObject, Vector3.Scale(Camera.main.transform.position, Vector3.right + Vector3.up), Quaternion.identity, GameObject.Find("Boxes").transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        generate = FindObjectOfType<GenerateLevelFromFile>();
    }

    private Vector2 GetMousePosition()
    {
        return Vector3.Scale(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.right + Vector3.up);
    }

    private bool CheckForColliding(Transform colliderObject)
    {
        if ((GetMousePosition() - (Vector2)colliderObject.transform.position).magnitude <= 2.0f)
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

    private GameObject binGizmo;

    private enum GizmoType { None, Transform, Rotation, Scale, Delete };

    private enum HideType { Hidden, Not_Hidden };

    private GizmoType gizmoType = GizmoType.None;

    private HideType hideType = HideType.Hidden;

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
                    if (gizmoType == GizmoType.Delete)
                    {
                        makeSureDelete.SetActive(true);
                        generate.gameObjectToDelete = selectedGameObject;
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedGameObject = null;
        }
        if (Input.GetMouseButton(0) && selectedGameObject != null && gizmoType == GizmoType.Transform)
        {
            MoveSelectedToMouse();
            transformGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z;
        }
        else if(Input.GetMouseButton(0) && selectedGameObject != null && gizmoType == GizmoType.Rotation)
        {
            RotateSelectedToMouse();
            rotationGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * rotationPrefab.transform.position.z;
        }
        else if (Input.GetMouseButton(0) && selectedGameObject != null && gizmoType == GizmoType.Scale)
        {
            ScaleSelectedToMouse();
            scaleGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * scalePrefab.transform.position.z;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            gizmoType = GizmoType.None;
            Destroy(transformGizmo);
            Destroy(rotationGizmo);
            Destroy(scaleGizmo);
            Destroy(binGizmo);
            foreach (Transform child in FindObjectsOfType<Transform>())
            {
                if (child.CompareTag("Origin"))
                {
                    Destroy(child.gameObject);
                }
            }
            if(hideType == HideType.Hidden)
            {
                hideType = HideType.Not_Hidden;
            }
            else
            {
                hideType = HideType.Hidden;
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            gizmoType = GizmoType.Transform;
            if (transformGizmo == null)
            {
                transformGizmo = Instantiate(transformPrefab, (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z, Quaternion.identity);
            }
            Destroy(binGizmo);
            Destroy(rotationGizmo);
            Destroy(scaleGizmo);
            foreach (Transform child in GameObject.Find("Boxes").transform)
            {
                if (child.parent.name == "Boxes")
                {
                    Instantiate(originPrefab, child.transform.position, Quaternion.identity).tag = "Origin";
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            gizmoType = GizmoType.Rotation;
            if(rotationGizmo == null)
            {
                rotationGizmo = Instantiate(rotationPrefab, (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z, Quaternion.identity);
            }
            Destroy(binGizmo);
            Destroy(transformGizmo);
            Destroy(scaleGizmo);
            foreach (Transform child in GameObject.Find("Boxes").transform)
            {
                if (child.parent.name == "Boxes")
                {
                    Instantiate(originPrefab, child.transform.position, Quaternion.identity).tag = "Origin";
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            gizmoType = GizmoType.Scale;
            if (scaleGizmo == null)
            {
                scaleGizmo = Instantiate(scalePrefab, (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z, Quaternion.identity);
            }
            Destroy(binGizmo);
            Destroy(transformGizmo);
            Destroy(rotationGizmo);
            foreach (Transform child in GameObject.Find("Boxes").transform)
            {
                if (child.parent.name == "Boxes")
                {
                    Instantiate(originPrefab, child.transform.position, Quaternion.identity).tag = "Origin";
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            gizmoType = GizmoType.Delete;
            if(binGizmo == null)
            {
                binGizmo = Instantiate(binPrefab, (Vector3)GetMousePosition() + Vector3.forward * binPrefab.transform.position.z, Quaternion.identity);
            }
            Destroy(transformGizmo);
            Destroy(rotationGizmo);
            Destroy(scaleGizmo);
            foreach (Transform child in GameObject.Find("Boxes").transform)
            {
                if (child.parent.name == "Boxes")
                {
                    Instantiate(originPrefab, child.transform.position, Quaternion.identity).tag = "Origin";
                }
            }
        }
        if(transformGizmo != null)
        {
            transformGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * transformPrefab.transform.position.z;
        }
        else if (rotationGizmo != null)
        {
            rotationGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * rotationPrefab.transform.position.z;
        }
        else if (scaleGizmo != null)
        {
            scaleGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * scalePrefab.transform.position.z;
        }
        else if (binGizmo != null)
        {
            binGizmo.transform.position = (Vector3)GetMousePosition() + Vector3.forward * binPrefab.transform.position.z;
        }
        if(hideType == HideType.Hidden)
        {
            levelMapEditor.SetActive(false);
        }
        else
        {
            levelMapEditor.SetActive(true);
        }
    }
}
