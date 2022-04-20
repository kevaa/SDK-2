using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreIndicator : MonoBehaviour
{
    TextMeshProUGUI text;

    [SerializeField] DartTagPlayer player;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }


    void Update()
    {
        text.text = Mathf.Round(player.score).ToString();
    }
}
