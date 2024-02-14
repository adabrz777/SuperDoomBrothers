using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeObject : MonoBehaviour
{
    public string world = "1-1";
    public string pipeNr = "A";

    public string toWorld = "1-1";
    public string toPipeNr = "B";

    public enum PipeTypeNames { toPipe, toWorld };
    public PipeTypeNames pipeType = PipeTypeNames.toPipe;


    public enum PipeLayNames { Vertical, Horizontal };
    public PipeLayNames pipeLay = PipeLayNames.Vertical;


}
