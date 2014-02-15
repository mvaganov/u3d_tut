using UnityEngine;
using System.Collections;

/// <summary>
/// Level switch with a keypress
/// </summary>
public class LevelSwitch : MonoBehaviour
{
    public KeyCode levelSwitchKey = KeyCode.Escape;
    public string nextLevel = "tut1";

	void Update()
    {
        if (Input.GetKeyDown(levelSwitchKey))
        {
            Application.LoadLevel(nextLevel);
        }
    }
}
