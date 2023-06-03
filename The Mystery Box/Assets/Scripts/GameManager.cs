using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI code;
    public static string joinCode;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(player);
        code.text = "Join Code: " + joinCode;
    }

    // Update is called once per frame
    void Update()
    {
        if (code.text != "Join Code: " + joinCode)
        {
            code.text = "Join Code: " + joinCode;
        }
    }
}
