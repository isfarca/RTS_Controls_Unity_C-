using UnityEngine;

public class SelectBox : MonoBehaviour
{
    [SerializeField] private RectTransform selectSquareImage;

    private Vector3 startPosition, endPosition;

    private void Start()
    {
        selectSquareImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                startPosition = hit.point;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            selectSquareImage.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0))
        {
            if (!selectSquareImage.gameObject.activeInHierarchy)
            {
                selectSquareImage.gameObject.SetActive(true);
            }

            endPosition = Input.mousePosition;

            var squareStart = Camera.main.WorldToScreenPoint(startPosition);
            squareStart.z = 0.0f;

            var centre = (squareStart + endPosition) / 2.0f;
            selectSquareImage.position = centre;

            var sizeX = Mathf.Abs(squareStart.x - endPosition.x);
            var sizeY = Mathf.Abs(squareStart.y - endPosition.y);
            
            selectSquareImage.sizeDelta = new Vector2(sizeX, sizeY);
        }
    }
}