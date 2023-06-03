using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : NetworkBehaviour
{
#if UNITY_EDITOR
    public UnityEditor.SceneAsset SceneAsset;
    private void OnValidate()
    {
        if (SceneAsset != null)
        {
            m_SceneName = SceneAsset.name;
        }
    }
#endif
    [SerializeField] private string m_SceneName;

    private Collider frame;

    private void Start()
    {
        frame = gameObject.transform.GetChild(3).GetComponent<Collider>();
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.tag == "Player" && IsServer && !string.IsNullOrEmpty(m_SceneName))
        {
            //var status = NetworkManager.SceneManager.LoadScene(m_SceneName, LoadSceneMode.Single);
            //if (status != SceneEventProgressStatus.Started)
            //{
            //    Debug.LogWarning($"Failed to load {m_SceneName} " +
            //          $"with a {nameof(SceneEventProgressStatus)}: {status}");
            //}
            SceneManager.LoadScene(m_SceneName);
        }
    }
}
