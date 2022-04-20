using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(TextMeshProUGUI))]
public class Ammo : MonoBehaviour
{
    [SerializeField] DartTagPlayer player;
    TextMeshProUGUI ammoText;

    private void Awake()
    {
        ammoText = GetComponent<TextMeshProUGUI>();
    }
    private void Update()
    {
        if (player.isIt)
        {
            ammoText.text = player.dartCount + "/" + player.dartTotal;
        }
        else
        {
            ammoText.text = "";
        }
    }

}
