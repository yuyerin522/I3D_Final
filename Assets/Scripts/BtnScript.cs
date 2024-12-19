using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnScript : MonoBehaviour
{
    public void StartBtnClicked()
    {
        SceneManager.LoadScene("MainSCene");
    }

}
