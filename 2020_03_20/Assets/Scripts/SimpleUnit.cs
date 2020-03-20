using UnityEngine;
using UnityEngine.AI;

public class SimpleUnit : MonoBehaviour
{
    [SerializeField] private GameObject m_cBuilding_Barracks;
    
    private NavMeshAgent m_cAgent;

    private Ray m_cRay;
    private RaycastHit m_cInfo;

    private void Awake()
    {
        m_cAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            m_cRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(m_cRay, out m_cInfo))
            {
                if (m_cInfo.collider.CompareTag("Ground"))
                {
                    m_cAgent.SetDestination(m_cInfo.point);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            m_cRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(m_cRay, out m_cInfo))
            {
                if (m_cInfo.collider.CompareTag("Ground"))
                {
                    var cObj = Instantiate(m_cBuilding_Barracks, m_cInfo.point + new Vector3(0.0f, 1.25f, 0.0f), Quaternion.identity);
                }
            }
        }*/
    }
}