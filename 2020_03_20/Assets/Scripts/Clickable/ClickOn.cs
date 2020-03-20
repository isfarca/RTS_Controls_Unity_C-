using UnityEngine;

public class ClickOn : MonoBehaviour
{
    [SerializeField] private Material white;
    [SerializeField] private Material red;

    private MeshRenderer rend;

    [HideInInspector] public bool CurrentlySelected = false;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
        Camera.main.gameObject.GetComponent<Click>().selectableObjects.Add(gameObject);
        ClickMe();
    }

    public void ClickMe()
    {
        if (!CurrentlySelected)
        {
            rend.material = white;
        }
        else
        { 
            rend.material = red;
        }
    }
}