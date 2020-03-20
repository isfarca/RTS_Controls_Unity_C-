using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    [SerializeField] private LayerMask clickableLayer;

    private List<GameObject> selectedObjects;

    [HideInInspector] public List<GameObject> selectableObjects;
    
    private Vector3 mousePosition1, mousePosition2;

    private void Start()
    {
        selectedObjects = new List<GameObject>();
        selectableObjects = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ClearSelection();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition1 = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            
            RaycastHit rayHit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out rayHit, 
                Mathf.Infinity, clickableLayer))
            {
                var clickOnScript = rayHit.collider.GetComponent<ClickOn>();

                if (Input.GetKey("left ctrl"))
                {
                    if (!clickOnScript.CurrentlySelected)
                    {
                        selectedObjects.Add(rayHit.collider.gameObject);
                        clickOnScript.CurrentlySelected = true;
                        clickOnScript.ClickMe();
                    }
                    else
                    {
                        selectedObjects.Remove(rayHit.collider.gameObject);
                        clickOnScript.CurrentlySelected = false;
                        clickOnScript.ClickMe();
                    }
                }
                else
                {
                    ClearSelection();

                    selectedObjects.Add(rayHit.collider.gameObject);
                    clickOnScript.CurrentlySelected = true;
                    clickOnScript.ClickMe();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            mousePosition2 = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (mousePosition1 != mousePosition2)
            {
                SelectObjects();
            }
        }
    }

    private void SelectObjects()
    {
        var removeObjects = new List<GameObject>();

        if (!Input.GetKey("left ctrl"))
        {
            ClearSelection();
        }
        
        var selectRect = new Rect(mousePosition1.x, mousePosition1.y, mousePosition2.x - mousePosition1.x, mousePosition2.y - mousePosition1.y);

        foreach (var selectObject in selectableObjects)
        {
            if (selectObject != null)
            {
                if (selectRect.Contains(Camera.main.WorldToViewportPoint(selectObject.transform.position), true))
                {
                    selectedObjects.Add(selectObject);
                    var selectObjectClickOnScript = selectObject.GetComponent<ClickOn>();
                    selectObjectClickOnScript.CurrentlySelected = true;
                    selectObjectClickOnScript.ClickMe();
                }
            }
            else
            {
                removeObjects.Add(selectObject);
            }
        }

        if (removeObjects.Count > 0)
        {
            foreach (var remObject in removeObjects)
            {
                selectableObjects.Remove(remObject);
            }
            
            removeObjects.Clear();
        }
    }

    private void ClearSelection()
    {
        if (selectedObjects.Count > 0)
        {
            foreach (var obj in selectedObjects)
            {
                var objClickOnScript = obj.GetComponent<ClickOn>();
                objClickOnScript.CurrentlySelected = false;
                objClickOnScript.ClickMe();
            }
                        
            selectedObjects.Clear();
        }
    }
}