using System.Net;
using System.Xml.Serialization;

namespace OgameGenSim.Classes;

/// <summary>
/// Universe data to do debri calculations, flight calculations, etc
/// Has a bunch of properties that are not used and will not be used, nevertheless it's not yet cleaned
/// </summary>
[XmlRoot("serverData")]
public class UniverseInformation
{
    [XmlElement("name")]
    public string Name { get; set; }
    [XmlElement("number")]
    public int Number { get; set; }
    [XmlElement("language")]
    public string Language { get; set; }
    [XmlElement("timezone")]
    public string Timezone { get; set; }
    [XmlElement("timezoneOffset")]
    public string TimezoneOffset { get; set; }
    [XmlElement("domain")]
    public string Domain { get; set; }
    [XmlElement("version")]
    public string Version { get; set; }
    [XmlElement("speed")]
    public int Speed { get; set; }
    [XmlElement("speedFleetPeaceful")]
    public int SpeedFleetPeaceful { get; set; }
    [XmlElement("speedFleetWar")]
    public int SpeedFleetWar { get; set; }
    [XmlElement("speedFleetHolding")]
    public int SpeedFleetHolding { get; set; }
    [XmlElement("galaxies")]
    public int Galaxies { get; set; }
    [XmlElement("systems")]
    public int Systems { get; set; }
    [XmlElement("acs")]
    public int Acs { get; set; }
    [XmlElement("rapidFire")]
    public int RapidFire { get; set; }
    [XmlElement("defToTF")]
    public int DefToTF { get; set; }
    [XmlElement("debrisFactor")]
    public double DebrisFactor { get; set; }
    [XmlElement("debrisFactorDef")]
    public double DebrisFactorDef { get; set; }
    [XmlElement("repairFactor")]
    public double RepairFactor { get; set; }
    [XmlElement("newbieProtectionLimit")]
    public long NewbieProtectionLimit { get; set; }
    [XmlElement("newbieProtectionHigh")]
    public long NewbieProtectionHigh { get; set; }
    [XmlElement("topScore")]
    public double TopScore { get; set; }
    [XmlElement("bonusFields")]
    public int BonusFields { get; set; }
    [XmlElement("donutGalaxy")]
    public int DonutGalaxy { get; set; }
    [XmlElement("donutSystem")]
    public int DonutSystem { get; set; }
    [XmlElement("wfEnabled")]
    public int WfEnabled { get; set; }
    [XmlElement("wfMinimumRessLost")]
    public long WfMinimumRessLost { get; set; }
    [XmlElement("wfMinimumLossPercentage")]
    public double WfMinimumLossPercentage { get; set; }
    [XmlElement("wfBasicPercentageRepairable")]
    public double WfBasicPercentageRepairable { get; set; }
    [XmlElement("globalDeuteriumSaveFactor")]
    public double GlobalDeuteriumSaveFactor { get; set; }
    [XmlElement("bashingSystemEnabled")]
    public int BashingSystemEnabled { get; set; }
    [XmlElement("bashlimit")]
    public int Bashlimit { get; set; }
    [XmlElement("probeCargo")]
    public int ProbeCargo { get; set; }
    [XmlElement("researchDurationDivisor")]
    public int ResearchDurationDivisor { get; set; }
    [XmlElement("darkMatterNewAccount")]
    public int DarkMatterNewAccount { get; set; }
    [XmlElement("cargoHyperspaceTechMultiplier")]
    public int CargoHyperspaceTechMultiplier { get; set; }
    [XmlElement("deuteriumInDebris")]
    public int DeuteriumInDebris { get; set; }
    [XmlElement("fleetIgnoreEmptySystems")]
    public string? FleetIgnoreEmptySystems { get; set; }
    [XmlElement("fleetIgnoreInactiveSystems")]
    public string? FleetIgnoreInactiveSystems { get; set; }
    [XmlElement("marketplaceEnabled")]
    public int MarketplaceEnabled { get; set; }
    [XmlElement("marketplaceBasicTradeRatioMetal")]
    public double MarketplaceBasicTradeRatioMetal { get; set; }
    [XmlElement("marketplaceBasicTradeRatioCrystal")]
    public double MarketplaceBasicTradeRatioCrystal { get; set; }
    [XmlElement("marketplaceBasicTradeRatioDeuterium")]
    public double MarketplaceBasicTradeRatioDeuterium { get; set; }
    [XmlElement("marketplacePriceRangeLower")]
    public double MarketplacePriceRangeLower { get; set; }
    [XmlElement("marketplacePriceRangeUpper")]
    public double MarketplacePriceRangeUpper { get; set; }
    [XmlElement("marketplaceTaxNormalUser")]
    public double MarketplaceTaxNormalUser { get; set; }
    [XmlElement("marketplaceTaxAdmiral")]
    public double MarketplaceTaxAdmiral { get; set; }
    [XmlElement("marketplaceTaxCancelOffer")]
    public double MarketplaceTaxCancelOffer { get; set; }
    [XmlElement("marketplaceTaxNotSold")]
    public double MarketplaceTaxNotSold { get; set; }
    [XmlElement("marketplaceOfferTimeout")]
    public int MarketplaceOfferTimeout { get; set; }
    [XmlElement("characterClassesEnabled")]
    public int CharacterClassesEnabled { get; set; }
    [XmlElement("minerBonusResourceProduction")]
    public double MinerBonusResourceProduction { get; set; }
    [XmlElement("minerBonusFasterTradingShips")]
    public int MinerBonusFasterTradingShips { get; set; }
    [XmlElement("minerBonusIncreasedCargoCapacityForTradingShips")]
    public double MinerBonusIncreasedCargoCapacityForTradingShips { get; set; }
    [XmlElement("minerBonusAdditionalFleetSlots")]
    public int MinerBonusAdditionalFleetSlots { get; set; }
    [XmlElement("minerBonusAdditionalMarketSlots")]
    public int MinerBonusAdditionalMarketSlots { get; set; }
    [XmlElement("minerBonusAdditionalCrawler")]
    public double MinerBonusAdditionalCrawler { get; set; }
    [XmlElement("minerBonusMaxCrawler")]
    public double MinerBonusMaxCrawler { get; set; }
    [XmlElement("minerBonusEnergy")]
    public double MinerBonusEnergy { get; set; }
    [XmlElement("minerBonusOverloadCrawler")]
    public int MinerBonusOverloadCrawler { get; set; }
    [XmlElement("resourceBuggyProductionBoost")]
    public double ResourceBuggyProductionBoost { get; set; }
    [XmlElement("resourceBuggyMaxProductionBoost")]
    public double ResourceBuggyMaxProductionBoost { get; set; }
    [XmlElement("resourceBuggyEnergyConsumptionPerUnit")]
    public int ResourceBuggyEnergyConsumptionPerUnit { get; set; }
    [XmlElement("warriorBonusFasterCombatShips")]
    public int WarriorBonusFasterCombatShips { get; set; }
    [XmlElement("warriorBonusFasterRecyclers")]
    public int WarriorBonusFasterRecyclers { get; set; }
    [XmlElement("warriorBonusFuelConsumption")]
    public double WarriorBonusFuelConsumption { get; set; }
    [XmlElement("warriorBonusRecyclerFuelConsumption")]
    public double WarriorBonusRecyclerFuelConsumption { get; set; }
    [XmlElement("warriorBonusRecyclerCargoCapacity")]
    public double WarriorBonusRecyclerCargoCapacity { get; set; }
    [XmlElement("warriorBonusAdditionalFleetSlots")]
    public int WarriorBonusAdditionalFleetSlots { get; set; }
    [XmlElement("warriorBonusAdditionalMoonFields")]
    public int WarriorBonusAdditionalMoonFields { get; set; }
    [XmlElement("warriorBonusFleetHalfSpeed")]
    public int WarriorBonusFleetHalfSpeed { get; set; }
    [XmlElement("warriorBonusAttackerWreckfield")]
    public int WarriorBonusAttackerWreckfield { get; set; }
    [XmlElement("combatDebrisFieldLimit")]
    public double CombatDebrisFieldLimit { get; set; }
    [XmlElement("explorerBonusIncreasedResearchSpeed")]
    public double ExplorerBonusIncreasedResearchSpeed { get; set; }
    [XmlElement("explorerBonusIncreasedExpeditionOutcome")]
    public double ExplorerBonusIncreasedExpeditionOutcome { get; set; }
    [XmlElement("explorerBonusLargerPlanets")]
    public double ExplorerBonusLargerPlanets { get; set; }
    [XmlElement("explorerUnitItemsPerDay")]
    public int ExplorerUnitItemsPerDay { get; set; }
    [XmlElement("explorerBonusPhalanxRange")]
    public double ExplorerBonusPhalanxRange { get; set; }
    [XmlElement("explorerBonusPlunderInactive")]
    public int ExplorerBonusPlunderInactive { get; set; }
    [XmlElement("explorerBonusExpeditionEnemyReduction")]
    public double ExplorerBonusExpeditionEnemyReduction { get; set; }
    [XmlElement("explorerBonusAdditionalExpeditionSlots")]
    public int ExplorerBonusAdditionalExpeditionSlots { get; set; }
    [XmlElement("resourceProductionIncreaseCrystalDefault")]
    public double ResourceProductionIncreaseCrystalDefault { get; set; }
    [XmlElement("resourceProductionIncreaseCrystalPos1")]
    public double ResourceProductionIncreaseCrystalPos1 { get; set; }
    [XmlElement("resourceProductionIncreaseCrystalPos2")]
    public double ResourceProductionIncreaseCrystalPos2 { get; set; }
    [XmlElement("resourceProductionIncreaseCrystalPos3")]
    public double ResourceProductionIncreaseCrystalPos3 { get; set; }
    [XmlElement("exodusRatioMetal")]
    public double ExodusRatioMetal { get; set; }
    [XmlElement("exodusRatioCrystal")]
    public double ExodusRatioCrystal { get; set; }
    [XmlElement("exodusRatioDeuterium")]
    public double ExodusRatioDeuterium { get; set; }
    [XmlElement("exodusActive")]
    public string? ExodusActive { get; set; }
}
