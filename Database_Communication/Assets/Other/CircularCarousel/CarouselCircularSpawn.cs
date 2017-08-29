using UnityEngine;
using System.Collections;

public class CarouselCircularSpawn : MonoBehaviour
{
    public Transform center;
    public GameObject galleryObject;
    public int segments = 0;
    public float radius = 0;
    public float rotationSpeed = 5;

    private Vector3 segmentScale = new Vector3(500f, 300f, 1f);

    void Start()
    {
        PopulateCarouselCircle();
    }

    void Update()
    {
        center.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }

    void PopulateCarouselCircle()
    {
        Vector3 centerPos = center.transform.localPosition;

        for (int i = 0; i < segments; i++)
        {
            float quotient = (float)i / segments;
            Vector3 pos = GetCirclePos(centerPos, radius, quotient);

            GameObject go = GameObject.Instantiate(galleryObject);
            go.transform.SetParent(center);
            go.transform.localScale = segmentScale;
            go.transform.localPosition = pos;

        }
    }

    Vector3 GetCirclePos(Vector3 _center, float _radius, float _multiplier)
    {
        float ang = _multiplier * 360;
        Vector3 pos;
        pos.x = _center.x + _radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = _center.y;
        pos.z = _center.z + _radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
