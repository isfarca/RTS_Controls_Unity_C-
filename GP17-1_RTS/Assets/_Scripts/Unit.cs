using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    [SerializeField]
    private GameObject m_cSelection;

    private NavMeshAgent m_cAgent;

    private void Awake()
    {
        m_cAgent = GetComponent<NavMeshAgent>();
    }

    public void OnHighlight(bool bEnable)
    {
        m_cSelection.SetActive(bEnable);
    }

    public void OnMove(Vector3 cDes)
    {
        m_cAgent.SetDestination(cDes);
    }
}