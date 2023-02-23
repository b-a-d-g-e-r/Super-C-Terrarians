using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Werewolf : Character
{
    //public override string name = "Werewolf";
    //public override float[,,] attacks = new float[13, 10, 5];
    public override float horizontalAcceleration { get; set; } = 3f;
    public override float horizontalMaxVelocity { get; set; } = 5f;
    public override float weight { get; set; } = 4f;
    public override float terminalVelocity { get; set; } = 6f;
    public override float maxJumpStrength { get; set; } = 4f;
    public override float jumpAcceleration { get; set; } = 2f;
    public override int totalJumps { get; set; } = 1;
}
