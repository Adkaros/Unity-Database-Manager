using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    public Vector3 defaultScale = new Vector3(500f, 300f, 1f);
    public Vector3 focusedScale = new Vector3(700f, 500f, 1f);

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

    void OnTriggerEnter(Collider _c)
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", focusedScale, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuad));
    }

    void OnTriggerExit(Collider _c)
    {
        iTween.ScaleTo(this.gameObject, iTween.Hash("scale", defaultScale, "time", 0.5f, "easetype", iTween.EaseType.easeOutQuad));
    }
}
