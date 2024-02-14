using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    public int initialLevelTime = 300;
    public string levelName;
    public string nextLevelName;

    public enum MapVariant { Overworld, Underground, Underwater, Castle };
    public MapVariant mapVariant;
}
