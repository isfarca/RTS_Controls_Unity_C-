using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance
    {
        get
        {
            if(m_cInstance == null)
            {
                m_cInstance = FindObjectOfType<UnitManager>();
            }
            return m_cInstance;
        }
    }

    private static UnitManager m_cInstance;

    [SerializeField]
    private Transform m_cUnitContainer;
    [SerializeField]
    private Unit m_cWarrior;

    private List<Unit> m_acSelection;

    private void Awake()
    {
        m_acSelection = new List<Unit>();
    }

    public void SelectSingleUnit(Unit cUnit)
    {
        m_acSelection.Add(cUnit);
        HighlightUnits(true);
    }

    public void SelectMultipleUnits(List<Unit> acUnits)
    {
        m_acSelection = acUnits;
        HighlightUnits(true);
    }

    public void DeselectUnits()
    {
        HighlightUnits(false);
        m_acSelection.Clear();
    }

    public void MoveUnits(Vector3 cDes)
    {
        foreach (Unit cUnit in m_acSelection)
        {
            cUnit.OnMove(cDes);
        }
    }

    public void InitWarrior(Vector3 cPos)
    {
        Instantiate<GameObject>(m_cWarrior.gameObject, cPos, Quaternion.identity, m_cUnitContainer);
    }

    private void HighlightUnits(bool bHighlight)
    {
        foreach(Unit cUnit in m_acSelection)
        {
            cUnit.OnHighlight(bHighlight);
        }
    }
}
