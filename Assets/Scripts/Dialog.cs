using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
    [SerializeField] List<string> lines = new List<string>();
    public List<string> Lines { get => lines; set => lines = value; }
}