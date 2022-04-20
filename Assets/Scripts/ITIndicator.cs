using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ITIndicator : MonoBehaviour
{
    TextMeshProUGUI text;
    Color itColor = Color.red;
    Color notItColor = Color.green;
    bool previousIt;
    [SerializeField] DartTagPlayer player;
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        UpdateText();
        previousIt = player.isIt;
    }
    // Update is called once per frame
    void Update()
    {
        if (previousIt != player.isIt)
        {
            UpdateText();
            previousIt = player.isIt;
        }
    }
    void UpdateText()
    {
        if (player.isIt)
        {
            text.color = itColor;
            text.text = "IT";

        }
        else
        {
            text.color = notItColor;
            text.text = "NOT IT";
        }
    }
}
