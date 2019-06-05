using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BaseMono : MonoBehaviour
{
    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void OnDestroy() { }
}