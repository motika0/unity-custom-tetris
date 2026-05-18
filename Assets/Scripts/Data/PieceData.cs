using System;
using System.Collections.Generic;

[Serializable]
public class PieceData
{
    public string id;

    public string color;

    public PivotData pivot;

    public List<BlockData> blocks;
}