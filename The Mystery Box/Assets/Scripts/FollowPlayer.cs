using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    private Transform tFollowTarget;
    private CinemachineFreeLook vcam;

    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                tFollowTarget = player.transform;
                vcam.LookAt = tFollowTarget;
                vcam.Follow = tFollowTarget;
            }
        }
    }
}
