using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool refrigeratorIsOpen = false;  
    public bool ovenIsOpen = false;          

    public void OpenRefrigeratorDoor(GameObject refrigeratorDoor)     // ≥√¿Â∞Ì πÆ ø≠±‚
    {
        if (!refrigeratorIsOpen)
        {
            refrigeratorDoor.transform.localRotation = Quaternion.Euler(0, -90, 0);
            refrigeratorIsOpen = true;
        }
    }

    public void CloseRefrigeratorDoor(GameObject refrigeratorDoor)     // ≥√¿Â∞Ì πÆ ¥›±‚
    {
        if (refrigeratorIsOpen)
        {
            refrigeratorDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
            refrigeratorIsOpen = false;
        }
    }

    public void OpenOvenDoor(GameObject ovenDoor)     // ø¿∫Ï πÆ ø≠±‚
    {
        if (!ovenIsOpen)
        {
            ovenDoor.transform.localRotation = Quaternion.Euler(90, 0, 0);
            ovenIsOpen = true;
        }
    }

    public void CloseOvenDoor(GameObject ovenDoor)     // ø¿∫Ï πÆ ¥›±‚
    {
        if (ovenIsOpen)
        {
            ovenDoor.transform.localRotation = Quaternion.Euler(0, 0, 0);
            ovenIsOpen = false;
        }
    }
}