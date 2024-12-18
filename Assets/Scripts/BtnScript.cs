using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnScript : MonoBehaviour
{
    public Transform rotatingObject;

    public void StartBtnClicked()
    {
        SceneManager.LoadScene("MainSCene");
    }

    public void RotateLeft()
    {
        if (rotatingObject != null)
        {
            rotatingObject.Rotate(0, -90, 0); 
        }
    }

    public void RotateRight()
    {
        if (rotatingObject != null)
        {
            rotatingObject.Rotate(0, 90, 0); 
        }
    }
}
