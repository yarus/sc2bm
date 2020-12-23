using System;
using System.Collections.Generic;

namespace SC2BM.WebSite.Classes.Helpers
{
    public class BuildOrderEncoder
    {
        private Dictionary<String, String> _dic = new Dictionary<string, string>();

        public string getValue(string code)
        {
            foreach (var item in _dic)
            {
                if (item.Value == code)
                {
                    return item.Key;
                }
            }

            return string.Empty;
        }

        public BuildOrderEncoder(string faction)
        {
            _dic.Add("DefaultItem", "mq");
            _dic.Add("StartIdle", "mw");
            _dic.Add("StopIdleIn3Seconds", "me");
            _dic.Add("StopIdleIn5Seconds", "mr");
            _dic.Add("StopIdleIn10Seconds", "mt");
            _dic.Add("StopIdleIn1Second", "my");

            if (faction.ToLower() == "terran")
            {
                //Terran Buildings
                _dic.Add("CommandCenter", "ao");
                _dic.Add("SupplyDepot", "ap");
                _dic.Add("Barracks", "aa");
                _dic.Add("OrbitalCommand", "as");
                _dic.Add("Refinery", "ad");
                _dic.Add("PlanetaryFortrees", "af");
                _dic.Add("Bunker", "ag");
                _dic.Add("Factory", "ah");
                _dic.Add("EngineeringBay", "aj");
                _dic.Add("GhostAcademy", "ak");
                _dic.Add("Starport", "al");
                _dic.Add("Armory", "az");
                _dic.Add("FusionCore", "ax");
                _dic.Add("TechLabOnBarracks", "ac");
                _dic.Add("ReactorOnFactory", "av");
                _dic.Add("ReactorOnStarport", "ab");
                _dic.Add("MissileTurret", "an");
                _dic.Add("TechLabOnFactory", "am");
                _dic.Add("TechLabOnStarport", "sq");
                _dic.Add("SensorTower", "sw");
                _dic.Add("ReactorOnBarracks", "se");
                //Terran units
                _dic.Add("SCV", "q");
                _dic.Add("Marine", "w");
                _dic.Add("Marauder", "e");
                _dic.Add("Reaper", "r");
                _dic.Add("Ghost", "t");
                _dic.Add("Hellion", "y");
                _dic.Add("SiegeTank", "u");
                _dic.Add("Cyclone", "i");
                _dic.Add("Thor", "o");
                _dic.Add("Viking", "p");
                _dic.Add("Medivac", "a");
                _dic.Add("Raven", "s");
                _dic.Add("Banshee", "d");
                _dic.Add("BattleCruiser", "f");
                _dic.Add("DoubleMarines", "g");
                _dic.Add("DoubleHellions", "h");
                _dic.Add("DoubleVikings", "j");
                _dic.Add("DoubleMedivacs", "k");
                _dic.Add("VikingPlusMedivac", "l");
                _dic.Add("DoubleReapers", "z");
                _dic.Add("DoubleWidowMines", "x");
                _dic.Add("DoubleHellbat", "c");
                _dic.Add("Hellbat", "v");
                _dic.Add("WidowMine", "b");
                _dic.Add("Nuke", "n");
                _dic.Add("Liberator", "m");
                _dic.Add("MarineOnReactor", "aq");
                _dic.Add("ReaperOnReactor", "aw");
                _dic.Add("HellionOnReactor", "ae");
                _dic.Add("HellbatOnReactor", "ar");
                _dic.Add("WidowMineOnReactor", "at");
                _dic.Add("VikingOnReactor", "ay");
                _dic.Add("MedivacOnReactor", "au");
                _dic.Add("LiberatorOnReactor", "ai");
                //Terran specials
                _dic.Add("MineralScv", "sr");
                _dic.Add("GasScv", "st");
                _dic.Add("LandStarportOnReactor", "sy");
                _dic.Add("CallSupplyDrop", "su");
                _dic.Add("LandRaxOnTechLab", "si");
                _dic.Add("LandFactoryOnTechLab", "so");
                _dic.Add("ScannerSweep", "sp");
                _dic.Add("LiftStarportFromTechLab", "sa");
                _dic.Add("CallMule", "ss");
                _dic.Add("LiftStarportFromReactor", "sd");
                _dic.Add("LiftFactoryFromTechLab", "sf");
                _dic.Add("ReturnScv", "sg");
                _dic.Add("SalvageBunker", "sh");
                _dic.Add("LandStarportOnTechLab", "sj");
                _dic.Add("GoOutScv", "sk");
                _dic.Add("LiftRaxFromReactor", "sl");
                _dic.Add("LiftRaxFromTechLab", "sz");
                _dic.Add("LandRaxOnReactor", "sx");
                _dic.Add("LiftFactoryFromReactor", "sc");
                _dic.Add("LandFactoryOnReactor", "sv");
                //Terran upgrades
                _dic.Add("InfantryWeaponsLevel1", "sb");
                _dic.Add("InfantryWeaponsLevel3", "sn");
                _dic.Add("InfantryWeaponsLevel2", "sm");
                _dic.Add("InfantryArmorLevel1", "dq");
                _dic.Add("NeosteelFrame", "dw"); //temp...
                _dic.Add("DurableMaterials", "de");
                _dic.Add("CorvidReactor", "dr");
                _dic.Add("CaduceusReactor", "dt");
                _dic.Add("CombatShield", "dy");
                _dic.Add("InfantryArmorLevel2", "du");
                _dic.Add("InfantryArmorLevel3", "di");
                _dic.Add("SiegeTech", "do");
                _dic.Add("BehemothReactor", "dp");
                _dic.Add("ConcussiveShells", "da");
                _dic.Add("ShipPlatingLevel2", "ds");
                _dic.Add("ShipPlatingLevel1", "dd");
                _dic.Add("ShipPlatingLevel3", "df");
                _dic.Add("HiSecAutoTracking", "dg");
                _dic.Add("VehiclePlatingLevel3", "dh");
                _dic.Add("VehiclePlatingLevel1", "dj");
                _dic.Add("VehiclePlatingLevel2", "dk");
                _dic.Add("DefenderMode", "dl");

                _dic.Add("VehicleAndShipPlatingLevel1", "dz");
                _dic.Add("VehicleAndShipPlatingLevel2", "dx");
                _dic.Add("VehicleAndShipPlatingLevel3", "dc");

                _dic.Add("ShipWeaponsLevel1", "dv");
                _dic.Add("ShipWeaponsLevel2", "db");
                _dic.Add("ShipWeaponsLevel3", "dn");
                _dic.Add("InfernalPreIgniter", "dm");
                _dic.Add("ThorStrikeCanons", "fq");

                _dic.Add("VehicleWeaponsLevel1", "fw");
                _dic.Add("VehicleWeaponsLevel2", "fe");
                _dic.Add("VehicleWeaponsLevel3", "fr");

                _dic.Add("VehicleAndShipWeaponsLevel1", "ft");
                _dic.Add("VehicleAndShipWeaponsLevel2", "fy");
                _dic.Add("VehicleAndShipWeaponsLevel3", "fu");

                _dic.Add("CloakingField", "fi");
                _dic.Add("StimPack", "fo");
                _dic.Add("TransformationServos", "fp");
                //_dic.Add("SeekerMissile", R.drawable.seekermissile); //not needed any more
                _dic.Add("PersonalCloaking", "fa");
                _dic.Add("BuildingArmor", "fs");
                _dic.Add("WeaponRefit", "fd");
                _dic.Add("MoebiusReactor", "ff");
                _dic.Add("NitroPacks", "fg");
                _dic.Add("DrillingClaws", "fh");
                _dic.Add("HyperflightRotors", "fj");
                _dic.Add("HighCapacityFuelTanks", "fk");
                _dic.Add("MagFieldAccelerator", "fl");
                _dic.Add("ExplosiveShrapnelShells", "fz");

                _dic.Add("SmartServos", "fx");
                _dic.Add("EnhancedMunitions", "fc");

                _dic.Add("RecalibratedExplosives", "fv");
                _dic.Add("AdvancedBallistics", "fb");
                _dic.Add("EnhancedShockwaves", "fn");
                _dic.Add("NeosteelArmor", "fm");
                _dic.Add("RapidReignitionSystem", "gq");
            }
            else if (faction.ToLower() == "protoss")
            {
                //Protoss buildings
                _dic.Add("Nexus", "ae");
                _dic.Add("Pylon", "ar");
                _dic.Add("DarkShrine", "at");
                _dic.Add("RoboticsFacility", "ay");
                _dic.Add("TemplarArchives", "au");
                _dic.Add("CyberneticsCore", "ai");
                _dic.Add("RoboticsBay", "ao");
                _dic.Add("Stargate", "ap");
                _dic.Add("TwilightCouncil", "aa");
                _dic.Add("PhotonCanon", "as");
                _dic.Add("Forge", "ad");
                _dic.Add("Assimilator", "af");
                _dic.Add("Gateway", "ag");
                _dic.Add("FleetBeacon", "ah");
                _dic.Add("ShieldBattery", "aj");
                //Protoss units
                _dic.Add("ArchonFromTwoHT", "s");
                _dic.Add("Zealot", "w");
                _dic.Add("WarpInHighTemplar", "a");
                _dic.Add("Phoenix", "p");
                _dic.Add("ArchonFromOneHTandOneDT", "o");
                _dic.Add("Colossus", "e");
                _dic.Add("ArchonFromTwoDT", "i");
                _dic.Add("WarpInSentry", "u");
                _dic.Add("Sentry", "r");
                _dic.Add("HighTemplar", "t");
                _dic.Add("WarpInStalker", "y");
                _dic.Add("Probe", "q");
                _dic.Add("Observer", "d");
                _dic.Add("WarpInZealot", "f");
                _dic.Add("Mothership", "g");
                _dic.Add("DarkTemplar", "h");
                _dic.Add("WarpInDarkTemplar", "j");
                _dic.Add("WarpPrism", "k");
                _dic.Add("VoidRay", "l");
                _dic.Add("Carrier", "z");
                _dic.Add("Stalker", "x");
                _dic.Add("Immortal", "c");
                _dic.Add("Oracle", "v");
                _dic.Add("MothershipCore", "b");
                _dic.Add("Tempest", "n");
                _dic.Add("Disruptor", "m");
                _dic.Add("Adept", "aq");
                _dic.Add("WarpInAdept", "aw");
                //Protoss specials
                _dic.Add("Chronoboost", "aj");
                _dic.Add("MineralProbe", "ak");
                _dic.Add("SwitchToWarpgate", "al");
                _dic.Add("SwitchToGateway", "az");
                _dic.Add("GoOutProbe", "ax");
                _dic.Add("ReturnProbe", "ac");
                _dic.Add("GasProbe", "av");
                _dic.Add("MassRecall", "ab");
                //Protoss upgrades
                _dic.Add("AirArmorLevel1", "sw");
                _dic.Add("AirArmorLevel2", "se");
                _dic.Add("AirArmorLevel3", "sr");
                _dic.Add("GroundArmorLevel1", "st");
                _dic.Add("GroundArmorLevel2", "sy");
                _dic.Add("GroundArmorLevel3", "su");
                _dic.Add("ShieldsLevel1", "si");
                _dic.Add("ShieldsLevel2", "so");
                _dic.Add("ShieldsLevel3", "sp");
                _dic.Add("GraviticDrive", "sa");
                _dic.Add("GraviticBoosters", "ss");
                _dic.Add("Charge", "sd");
                _dic.Add("ResonatingGlaves", "sf");
                _dic.Add("WarpgateUpgrade", "sg");
                _dic.Add("PsionicStorm", "sh");
                _dic.Add("GravitonCatapult", "sj");
                _dic.Add("GroundWeaponsLevel3", "sk");
                _dic.Add("GroundWeaponsLevel2", "sl");
                _dic.Add("GroundWeaponsLevel1", "sz");
                _dic.Add("Hallucination", "sx");
                _dic.Add("Blink", "sc");
                _dic.Add("ExtendedThermalLance", "sv");
                _dic.Add("AirWeaponsLevel2", "sb");
                _dic.Add("AirWeaponsLevel1", "sn");
                _dic.Add("AirWeaponsLevel3", "sm");
                _dic.Add("AnionPulseCrystals", "dq");
                _dic.Add("ShadowStride", "dw");
                _dic.Add("TectonicDestabilizers", "de");
                _dic.Add("FluxVanes", "dr");
            }
            else if (faction.ToLower() == "zerg")
            {
                //Zerg buildings
                _dic.Add("Spire", "z");
                _dic.Add("Hive", "x");
                _dic.Add("GreaterSpire", "c");
                _dic.Add("SporeCrawler", "v");
                _dic.Add("HydraliskDen", "b");
                _dic.Add("NydusNetwork", "n");
                _dic.Add("EvolutionChamber", "m");
                _dic.Add("SpineCrawler", "aq");
                _dic.Add("InfestationPit", "aw");
                _dic.Add("Lair", "ae");
                _dic.Add("RoachWarren", "ar");
                _dic.Add("Hatchery", "at");
                _dic.Add("MacroHatchery", "ay");
                _dic.Add("UltraliskCavern", "au");
                _dic.Add("Extractor", "ai");
                _dic.Add("SpawningPool", "ao");
                _dic.Add("BanelingNest", "ap");
                _dic.Add("LurkerDen", "aa");
                //Zerg units
                _dic.Add("Queen", "q");
                _dic.Add("Mutalisk", "w");
                _dic.Add("Corruptor", "e");
                _dic.Add("Baneling", "r");
                _dic.Add("Roach", "t");
                _dic.Add("Zergling", "y");
                _dic.Add("Ultralisk", "u");
                _dic.Add("Drone", "i");
                _dic.Add("Infestor", "o");
                _dic.Add("Overlord", "p");
                _dic.Add("Hydralisk", "a");
                _dic.Add("Overseer", "s");
                _dic.Add("Viper", "d");
                _dic.Add("SwarmHost", "f");
                _dic.Add("Broodlord", "g");
                _dic.Add("Lurker", "h");
                _dic.Add("Ravager", "j");
                _dic.Add("NydusWorm", "k");
                //Zerg specials
                _dic.Add("InjectLarva", "l");
                _dic.Add("ReturnDrone", "as");
                _dic.Add("GoOutDrone", "ad");
                _dic.Add("GasDrone", "af");
                _dic.Add("SpawnCreepTumor", "ag");
                _dic.Add("MineralDrone", "ah");
                _dic.Add("CancelExtractor", "aj");
                //Zerg upgrades
                _dic.Add("FlyerAttacksLevel3", "ak");
                _dic.Add("FlyerAttacksLevel1", "al");
                _dic.Add("MetabolicBoost", "az");
                _dic.Add("FlyerAttacksLevel2", "ax");
                _dic.Add("MeleeAttacksLevel3", "ac");
                _dic.Add("AdrenalineGlands", "av");
                _dic.Add("VentralSacs", "ab");
                _dic.Add("GlialReconstitution", "an");
                _dic.Add("MissleAttacksLevel3", "am");
                _dic.Add("Burrow", "sq");
                _dic.Add("MissleAttacksLevel2", "sw");
                _dic.Add("PathogenGlands", "se");
                _dic.Add("TunnelingClaws", "sr");
                _dic.Add("GroundCarapaceLevel3", "st");
                _dic.Add("FlyerCarapaceLevel1", "sy");
                _dic.Add("GroundCarapaceLevel1", "su");
                _dic.Add("FlyerCarapaceLevel2", "si");
                _dic.Add("PneumatizedCarapace", "so");
                _dic.Add("GroundCarapaceLevel2", "sp");
                _dic.Add("FlyerCarapaceLevel3", "sa");
                _dic.Add("NeuralParasite", "ss");
                _dic.Add("MissleAttacksLevel1", "sd");
                _dic.Add("ChitinousPlating", "sf");
                _dic.Add("CentrifugalHooks", "sg");
                _dic.Add("MeleeAtacksLevel2", "sh");
                _dic.Add("MeleeAttacksLevel1", "sj");
                _dic.Add("GroovedSpines", "sk");
                _dic.Add("MuscularAugments", "sl");
                _dic.Add("EnduringLocusts", "sz");
                _dic.Add("FlyingLocusts", "sx");
                _dic.Add("MutateVentralSacs", "sc");
                _dic.Add("AdaptiveTalons", "sv");
                _dic.Add("SeismicSpines", "sb");
                _dic.Add("AnabolicSynthesis", "sn");
            }
        }
    }
}