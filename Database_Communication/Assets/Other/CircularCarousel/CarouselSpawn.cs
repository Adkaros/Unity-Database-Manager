using UnityEngine;
using System.Collections;

public class CarouselSpawn : MonoBehaviour
{
    public Transform center;
    public Transform front, left, back, right;
    public GameObject galleryObject;
    public int segments = 0;

	void Start ()
    {
        PopulateCarousel();
	}

    void PopulateCarousel()
    {
        CreateSegments(front, right);
        CreateSegments(right, back);
        CreateSegments(back, left);
        CreateSegments(left, front);
    }

    private void CreateSegments(Transform _from, Transform _to)
    {
        float quotient = 0;

        for (int i = 0; i < segments; i++)
        {
            quotient = (float)i / segments;
            Vector3 determinedSpot = Vector3.Lerp(_from.localPosition, _to.localPosition, quotient);

            GameObject go = GameObject.Instantiate(galleryObject);
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(500f, 300f, 1f);
            go.transform.localPosition = determinedSpot;
        }
    }

}
