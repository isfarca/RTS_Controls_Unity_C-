using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Image m_cSelectionRectangle;
    [SerializeField]
    private Canvas m_cUICanvas;
    [SerializeField]
    private float m_fTimeTillSelection;

    private float m_fTimeTillSelectionLeft;
    private Image m_cSelectionUI;
    private Vector3 m_cSelectionStart;
    private Vector3 m_cSelectionStartGround;
    private Vector3 m_cSelectionSize;

    private Ray m_cRay;
    private RaycastHit m_cInfo;

    private float m_fCamDist;

    private Vector3 m_cSelectionBoxCenter;
    private Vector3 m_cSelectionBoxScale;

    private void Awake()
    {
        m_fTimeTillSelectionLeft = m_fTimeTillSelection;
        m_cSelectionUI = null;
        m_cSelectionStart = Vector3.zero;
        m_cSelectionStartGround = Vector3.zero;
        m_cSelectionSize = Vector3.zero;

        m_cSelectionBoxCenter = Vector3.zero;
        m_cSelectionBoxScale = Vector3.zero;

        m_cRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(m_cRay, out m_cInfo))
        {
            if (m_cInfo.collider.CompareTag("Ground"))
            {
                m_fCamDist = Vector3.Distance(m_cInfo.point, Camera.main.transform.position);
            }
        }
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0))
        {
            UnitManager.Instance.DeselectUnits();

            if (m_fTimeTillSelectionLeft == m_fTimeTillSelection)
            {
                m_cSelectionStart = Input.mousePosition;
                CheckForTag(Input.mousePosition, "Ground", cInfo => { m_cSelectionStartGround = cInfo.point; }, null);
            }

            m_fTimeTillSelectionLeft -= Time.deltaTime;

            if(m_fTimeTillSelectionLeft <= 0.0f)
            {
                if(m_cSelectionUI == null)
                {
                    m_cSelectionUI = Instantiate<Image>(m_cSelectionRectangle, m_cSelectionStart, Quaternion.identity, m_cUICanvas.transform);
                }

                m_cSelectionSize = Input.mousePosition - m_cSelectionStart;
                m_cSelectionUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Abs(m_cSelectionSize.x), Mathf.Abs(m_cSelectionSize.y));
                m_cSelectionUI.GetComponent<RectTransform>().anchoredPosition = (Vector2)m_cSelectionStart + new Vector2(m_cSelectionSize.x * 0.5f, m_cSelectionSize.y * 0.5f);
            }
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            m_fTimeTillSelectionLeft = m_fTimeTillSelection;

            if(m_cSelectionUI != null)
            {
                Vector3 cSelectionEndGround = Vector3.zero;
                CheckForTag(Input.mousePosition, "Ground", cInfo => { cSelectionEndGround = cInfo.point; }, null);

                float fDistance = Vector3.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.forward * m_fCamDist), Camera.main.ScreenToWorldPoint(m_cSelectionStart - Camera.main.transform.forward * m_fCamDist)) * 0.5f;
                m_cSelectionBoxCenter = Vector3.Lerp(Camera.main.ScreenToWorldPoint(Input.mousePosition - Camera.main.transform.forward * m_fCamDist), Camera.main.ScreenToWorldPoint(m_cSelectionStart - Camera.main.transform.forward * m_fCamDist), 0.5f);
                m_cSelectionBoxScale = new Vector3(fDistance, fDistance, 100.0f);

                Vector3 cMid = Vector3.zero;
                Vector3 cScreenMid = Camera.main.WorldToScreenPoint(m_cSelectionBoxCenter);
                CheckForTag(cScreenMid, "Ground", cInfo => { cMid = cInfo.point; }, cInfo => { cMid = cInfo.point; });

                Collider[] acCols = Physics.OverlapBox(m_cSelectionBoxCenter, m_cSelectionBoxScale, Quaternion.LookRotation(cMid - Camera.main.transform.position, Camera.main.transform.up));

                List<Unit> acUnits = new List<Unit>();

                foreach(Collider cCol in acCols)
                {
                    if(cCol.gameObject.GetComponent<Unit>() != null)
                    {
                        acUnits.Add(cCol.gameObject.GetComponent<Unit>());
                    }
                }
                UnitManager.Instance.SelectMultipleUnits(acUnits);
                
                Destroy(m_cSelectionUI.gameObject);
                m_cSelectionUI = null;
                m_cSelectionStart = Vector3.zero;
                m_cSelectionStartGround = Vector3.zero;
                m_cSelectionSize = Vector3.zero;

                m_cSelectionBoxCenter = Vector3.zero;
                m_cSelectionBoxScale = Vector3.zero;
            }
            else
            {
                CheckForTag(Input.mousePosition, "Unit", cInfo =>
                {
                    if (cInfo.collider.gameObject.GetComponent<Unit>() != null)
                    {
                        UnitManager.Instance.SelectSingleUnit(cInfo.collider.gameObject.GetComponent<Unit>());
                    }
                }, null);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if(Input.GetKey(KeyCode.Alpha1))
            {
                CheckForTag(Input.mousePosition, "Ground", cInfo => { UnitManager.Instance.InitWarrior(cInfo.point); }, null);
            }
            else
            {
                CheckForTag(Input.mousePosition, "Ground", cInfo => { UnitManager.Instance.MoveUnits(cInfo.point); }, null);
            }
        }
    }

    private void CheckForTag(Vector3 cScreenPoint, string sTag, Action<RaycastHit> cSuccessAction, Action<RaycastHit> cFailureAction)
    {
        m_cRay = Camera.main.ScreenPointToRay(cScreenPoint);

        if (Physics.Raycast(m_cRay, out m_cInfo))
        {
            if (m_cInfo.collider.CompareTag(sTag))
            {
                if(cSuccessAction != null)
                {
                    cSuccessAction.Invoke(m_cInfo);
                }
            }
            else
            {
                if(cFailureAction != null)
                {
                    cFailureAction.Invoke(m_cInfo);
                }
            }
        }
    }
}
