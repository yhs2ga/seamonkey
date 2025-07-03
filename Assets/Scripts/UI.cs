using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{

    private TextMeshProUGUI tmp;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        tmp.text = "HP: " + PlayerMove.playerHP;
    }
}
