using UnityEngine;

public struct ScreenToWorldPoint
{
    public Vector3 OriginPoint;
    public Vector3 Axis;
    public Vector3 farPoint;
    public ScreenToWorldPoint(Vector3 screenPoint) 
    {
        Vector3 mousePosFar = new Vector3(screenPoint.x, screenPoint.y, Camera.main.farClipPlane);
        Vector3 mousePosNear = new Vector3(screenPoint.x, screenPoint.y, Camera.main.nearClipPlane);

        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(mousePosFar);
        Vector3 mousePosN = Camera.main.ScreenToWorldPoint(mousePosNear);

        OriginPoint = mousePosN;
        farPoint = mousePosF;
        Axis = mousePosF - mousePosN;
    }
}
