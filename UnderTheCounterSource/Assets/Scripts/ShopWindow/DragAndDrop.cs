using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    private float zCoordinate;

    void OnMouseDown()
    {
        zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = gameObject.transform.position - GetMouseWorldPos();
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
