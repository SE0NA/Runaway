using System.Collections;
using System.Collections.Generic;
using System;

public struct Level
{
    public int level;
    public Stage[] stagelist;
}
public struct Stage
{
    public int stageNo;
    public bool clear;
    public int[] blocks;
}

[Serializable]
public class StageData {
    public Level[] levellist = new Level[3];
}
