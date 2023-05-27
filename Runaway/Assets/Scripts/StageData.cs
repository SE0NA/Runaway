using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public struct Stage
{
    public int stageNo;
    public bool clear;
    public int[] blocks;
}

[Serializable]
public class StageData
{
    public string version;
    public int level;
    public Stage[] stagelist;
}
