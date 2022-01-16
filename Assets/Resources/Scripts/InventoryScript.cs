using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    GameObject selectedGameObject;

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
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedGameObject = null;
        }
        if (Input.GetMouseButton(0) && selectedGameObject != null)
        {
            MoveSelectedToMouse();
        }
    }
}
