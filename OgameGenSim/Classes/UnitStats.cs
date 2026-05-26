namespace OgameGenSim.Classes;

/// <summary>
/// Unit statistics, couts for defense and ships
/// It must be attached to a type
/// It will have the values with all bonuses already calculated
/// </summary>
public class UnitStats
{
    /// <summary>
    /// Amount of units 
    /// </summary>
    public int Amount { get; set; }
    /// <summary>
    /// Weapon power
    /// </summary>
    public double Weapon { get; set; }
    /// <summary>
    /// Shield power
    /// </summary>
    public double Shield { get; set; }
    /// <summary>
    /// Armor power (HP)
    /// </summary>
    public double Armor { get; set; }
}
