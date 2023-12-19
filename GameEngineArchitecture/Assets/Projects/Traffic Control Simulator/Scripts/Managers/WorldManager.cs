using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Top level manager.
/// </summary>
public class WorldManager : Singleton<WorldManager>
{
    public void Quit()
    {
        Application.Quit();
    }
}
