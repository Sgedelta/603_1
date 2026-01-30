using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PopupMessage : MonoBehaviour
{
    //public Sprite hitMarker;
    //public Sprite electrifyMarker;
    //public string hitDyingMessage = "*Hit*";
    //public string electrifyDyingMessage = "*Electrified*";
    static PopupMessage _instance;
    public static PopupMessage Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public GameObject hitMarkerPrefab;
    public GameObject electrifyMarkerPrefab;

    public void ShowMessageHit(Vector2 position)
    {
        var markerObject = Instantiate(hitMarkerPrefab, transform);
        markerObject.transform.position = position;
        StartCoroutine(ShowMarker(markerObject));
    }
    public void ShowMessageElectrify(Vector2 position)
    {
        var markerObject = Instantiate(electrifyMarkerPrefab, transform);
        markerObject.transform.position = position;
        StartCoroutine(ShowMarker(markerObject));
    }

    //void ShowMessage(Vector2 position, string message, Sprite sprite)
    //{

    //    StartCoroutine(ShowMarker(markerObject, message, sprite));
    //}


    IEnumerator ShowMarker(GameObject markerObject)
    {

        TMP_Text text = markerObject.GetComponentInChildren<TMP_Text>();
        SpriteRenderer sr = markerObject.GetComponentInChildren<SpriteRenderer>();

        float t = 1;
        while (true)
        {
            t = Mathf.Lerp(t, 0, Time.deltaTime);
            Color color = new Color(1, 1, 1, t);
            text.color = color;
            sr.color = color;

            Vector3 pos = markerObject.transform.position;
            pos.y = pos.y + Time.deltaTime * 1;

            markerObject.transform.position = pos;
            if (t < 0.02f)
            {
                Destroy(markerObject);
                yield break;
            }
            yield return null;
        }
    }
}
