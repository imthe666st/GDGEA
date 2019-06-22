using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    public float MoveSpeed = 2f;
    public float FadeSpeed = 1f;

    private float fade = 0;

    private TextMeshProUGUI text;

    private void Awake()
    {
        this.text = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        this.transform.position += Time.deltaTime * this.MoveSpeed * Vector3.up;

        var color = this.text.color;
        color.a -= Time.deltaTime * this.FadeSpeed;
        this.text.color = color;

        if (Math.Abs(color.a) <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
