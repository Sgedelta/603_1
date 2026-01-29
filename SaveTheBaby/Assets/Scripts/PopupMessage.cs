using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class PopupMessage : MonoBehaviour
{
    public Sprite hitMarker;
    public Sprite electrifyMarker;
    static PopupMessage _instance;
    public static PopupMessage Instance => _instance;
    private void Awake()
    {
        if (_instance == null)
            _instance = this;
    }

    public GameObject markerPrefab;

    public void ShowMessageHit(Vector2 position)
    {
        ShowMessage(position, "*Hit*", hitMarker);
    }
    public void ShowMessageElectrify(Vector2 position)
    {
        ShowMessage(position, "*Electrified*", electrifyMarker);
    }

    void ShowMessage(Vector2 position, string message, Sprite sprite)
    {
        var markerObject = Instantiate(markerPrefab, transform);
        markerObject.transform.position = position;
        StartCoroutine(ShowMarker(markerObject, message, sprite));
    }


    IEnumerator ShowMarker(GameObject markerObject, string message, Sprite sprite)
    {

        TMP_Text text = markerObject.GetComponentInChildren<TMP_Text>();
        text.text = message;
        SpriteRenderer sr = markerObject.GetComponentInChildren<SpriteRenderer>();
        sr.sprite = sprite;

        float t = 1;
        while (true)
        {
            t = Mathf.Lerp(t, 0, Time.deltaTime);
            Color color = new Color(1, 1, 1, t);
            text.color = color;
            sr.color = color;

            Vector3 pos = markerObject.transform.position;
            pos.y = pos.y + Time.deltaTime * 0;

            markerObject.transform.position = pos;
            if (pos.y > 10)
            {
                Destroy(markerObject);
                yield break;
            }
            yield return null;
        }
    }
}
