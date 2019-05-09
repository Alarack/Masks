using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public float upForce;
    public Rigidbody2D MyBody { get; private set; }

    private TextMeshProUGUI mainText;

    void Awake()
    {
        MyBody = GetComponent<Rigidbody2D>();
        mainText = GetComponentInChildren<TextMeshProUGUI>();
    }

    //private void Start()
    //{
    //    Initialize("-5", Vector2.zero);
    //}

    public void Initialize(string text, Vector2 startPosition)
    {
        SetText(text);
        transform.localPosition = startPosition;
        Bounce();

        Destroy(gameObject, 2f);
    }

    public void SetText(string value)
    {
        mainText.text = value;
    }

    public void Bounce()
    {

        float errorX = Random.Range(-0.2f, 0.2f);
        float errorY = Random.Range(-0.1f, 0.3f);

        Vector2 errorV = new Vector2(errorX, errorY);

        MyBody.AddForce((Vector2.up + errorV) * upForce);

    }


}
