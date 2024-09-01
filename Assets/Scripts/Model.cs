using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EInteractionType
{
    GamePad,
    Treadmill
}

public enum ETaskState
{
    PathVisualization,
    Ready,
    EncodingTurnA,
    EncodingWalkA,
    EncodingTurnB,
    EncodingWalkB,
    Execution
}

public struct TriangleSet
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
}