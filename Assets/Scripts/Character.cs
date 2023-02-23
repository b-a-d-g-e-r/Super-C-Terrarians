using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
 //   public string name;
 //   public float[,,] attacks = new float[13, 10, 5];
    private float _horizontalAcceleration;
	private float _horizontalMaxVelocity;
    private float _weight;
    private float _terminalVelocity;
    private float _maxJumpStrength;
    private float _jumpAcceleration;
    private int _totalJumps;
    private float _sheildHealth;

    public virtual float horizontalAcceleration
    {
        get { return _horizontalAcceleration; }
        set { _horizontalAcceleration = value; }
    }
	public virtual float horizontalMaxVelocity
    {
        get { return _horizontalMaxVelocity; }
        set { _horizontalMaxVelocity = value; }
    }
	public virtual float weight
    {
        get { return _weight; }
        set { _weight = value; }
    }
	public virtual float terminalVelocity
    {
        get { return _terminalVelocity; }
        set { _terminalVelocity = value; }
    }
	public virtual float maxJumpStrength
    {
        get { return _maxJumpStrength; }
        set { _maxJumpStrength = value; }
    }
	public virtual float jumpAcceleration
    {
        get { return _jumpAcceleration; }
        set { _jumpAcceleration = value; }
    }
    public virtual int totalJumps
    {
        get { return _totalJumps; }
        set { _totalJumps = value; }
    }
	public virtual float sheildHealth
    {
        get { return _sheildHealth; }
        set { _sheildHealth = value; }
    }
}

