using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct Level
{
    public int level;
    public string title;
    public int total;
    public int clear;
}

[Serializable]
public class LevelData
{
    public string version;
    public Level[] levellist;
}
