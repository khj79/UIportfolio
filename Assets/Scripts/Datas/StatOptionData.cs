using System;
using Game.Enums;

[System.Serializable]
public class StatOptionData : GameData
{
    public StatOptionCategory Category;
    public Int32 Value;

    public override bool Validate()
    {
        return base.Validate();
    }
}