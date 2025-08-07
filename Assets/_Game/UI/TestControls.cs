using UnityEngine;
using UnityEngine.Events;


public class TestControls : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            UIManager.OpenPC();
        }
    }
}
