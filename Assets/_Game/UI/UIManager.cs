using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public static event Action OnPCOpened;
    public static event Action OnPCClosed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public static void OpenPC()
    {
        OnPCOpened?.Invoke();
        Debug.Log("PC Opened");
    }

    public static void ClosePC()
    {
        OnPCClosed?.Invoke();
        Debug.Log("PC Closed");
    }
}
