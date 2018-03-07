﻿(function () {
    'use strict';

    angular.module('Sc2bmApp').constant("buildItemsImages", [
        { name: "CommandCenter", src: "bi_commandcenter" },
        { name: "SupplyDepot", src: "bi_supplaydepo" },
        { name: "Barracks", src: "bi_barracks" },
        { name: "OrbitalCommand", src: "bi_orbitalcommand" },
        { name: "Refinery", src: "bi_refinery" },
        { name: "PlanetaryFortrees", src: "bi_planetaryfortress" },
        { name: "Bunker", src: "bi_bunker" },
        { name: "Factory", src: "bi_factory" },
        { name: "EngineeringBay", src: "bi_engineeringbay" },
        { name: "GhostAcademy", src: "bi_ghostacademy" },
        { name: "Starport", src: "bi_starport" },
        { name: "Armory", src: "bi_armory" },
        { name: "FusionCore", src: "bi_fusioncore" },
        { name: "TechLabOnBarracks", src: "bi_techlab" },
        { name: "ReactorOnFactory", src: "bi_reactor" },
        { name: "ReactorOnStarport", src: "bi_reactor" },
        { name: "MissileTurret", src: "bi_missleturret" },
        { name: "TechLabOnFactory", src: "bi_techlab" },
        { name: "TechLabOnStarport", src: "bi_techlab" },
        { name: "SensorTower", src: "bi_sensortower" },
        { name: "ReactorOnBarracks", src: "bi_reactor" },
        { name: "SCV", src: "bi_scv" },
        { name: "Marine", src: "bi_marine" },
        { name: "Marauder", src: "bi_marauder" },
        { name: "Reaper", src: "bi_reaper" },
        { name: "Ghost", src: "bi_ghost" },
        { name: "Hellion", src: "bi_helion" },
        { name: "SiegeTank", src: "bi_tank" },
        { name: "Cyclone", src: "bi_cyclone" },
        { name: "Thor", src: "bi_thor" },
        { name: "Viking", src: "bi_viking" },
        { name: "Medivac", src: "bi_medivak" },
        { name: "Raven", src: "bi_raven" },
        { name: "Banshee", src: "bi_banshee" },
        { name: "BattleCruiser", src: "bi_battlecruiser" },
        { name: "Hellbat", src: "bi_hellbat" },
        { name: "WidowMine", src: "bi_widowmine" },
        { name: "Nuke", src: "bi_nuke" },
        { name: "Liberator", src: "bi_liberator" },
        { name: "MarineOnReactor", src: "bi_marineonreactor" },
        { name: "ReaperOnReactor", src: "bi_reaperonreactor" },
        { name: "HellionOnReactor", src: "bi_helliononreactor" },
        { name: "HellbatOnReactor", src: "bi_hellbatonreactor" },
        { name: "WidowMineOnReactor", src: "bi_widowmineonreactor" },
        { name: "VikingOnReactor", src: "bi_vikingonreactor" },
        { name: "MedivacOnReactor", src: "bi_medivaconreactor" },
        { name: "LiberatorOnReactor", src: "bi_liberatoronreactor" },
        { name: "CycloneOnReactor", src: "bi_cycloneonreactor" },

        { name: "MineralScv", src: "bi_mineralscv" },
        { name: "GasScv", src: "bi_gasscv" },
        { name: "CallSupplyDrop", src: "bi_supplydrop" },
        { name: "LandRaxOnTechLab", src: "bi_landbarracks_lab" },
        { name: "LandRaxOnReactor", src: "bi_landbarracks_reactor" },
        { name: "LandFactoryOnTechLab", src: "bi_landfactory_lab" },
        { name: "LandFactoryOnReactor", src: "bi_landfactory_reactor" },
        { name: "LandStarportOnReactor", src: "bi_landstarport_reactor" },
        { name: "LandStarportOnTechLab", src: "bi_landstarport_lab" },
        { name: "ScannerSweep", src: "bi_scannersweep" },
        { name: "CallMule", src: "bi_callmule" },
        { name: "LiftStarportFromTechLab", src: "bi_liftstarport_lab" },
        { name: "LiftStarportFromReactor", src: "bi_liftstarport_reactor" },
        { name: "LiftFactoryFromTechLab", src: "bi_liftfactory_lab" },
        { name: "LiftFactoryFromReactor", src: "bi_landfactory_reactor" },
        { name: "LiftRaxFromReactor", src: "bi_liftbarracks_reactor" },
        { name: "LiftRaxFromTechLab", src: "bi_liftbarracks_lab" },
        { name: "ReturnScv", src: "bi_scout_in_terran" },
        { name: "SalvageBunker", src: "bi_salvagebunker" },
        { name: "GoOutScv", src: "bi_scout_out_terran" },

        { name: "InfantryWeaponsLevel1", src: "bi_infantryweapons1" },
        { name: "InfantryWeaponsLevel3", src: "bi_infantryweapons3" },
        { name: "InfantryWeaponsLevel2", src: "bi_infantryweapons2" },
        { name: "InfantryArmorLevel1", src: "bi_infantryarmor1" },
        { name: "NeosteelFrame", src: "bi_shipplating1" },
        { name: "DurableMaterials", src: "bi_durablematerials" },
        { name: "CorvidReactor", src: "bi_corvidreactor" },
        { name: "CaduceusReactor", src: "bi_caduceusreactor" },
        { name: "CombatShield", src: "bi_combatshield" },
        { name: "InfantryArmorLevel2", src: "bi_infantryarmor2" },
        { name: "InfantryArmorLevel3", src: "bi_infantryarmor3" },
        { name: "SiegeTech", src: "bi_siegetech" },
        { name: "BehemothReactor", src: "bi_behemothreactor" },
        { name: "ConcussiveShells", src: "bi_concussiveshells" },
        { name: "ShipPlatingLevel2", src: "bi_shipplating2" },
        { name: "ShipPlatingLevel1", src: "bi_shipplating1" },
        { name: "ShipPlatingLevel3", src: "bi_shipplating3" },
        { name: "HiSecAutoTracking", src: "bi_hisecautotracking" },
        { name: "VehiclePlatingLevel3", src: "bi_vehicleplating3" },
        { name: "VehiclePlatingLevel1", src: "bi_vehicleplating1" },
        { name: "VehiclePlatingLevel2", src: "bi_vehicleplating2" },
        { name: "DefenderMode", src: "bi_defendermode" },
        { name: "VehicleAndShipPlatingLevel1", src: "bi_vehicleplating1" },
        { name: "VehicleAndShipPlatingLevel2", src: "bi_vehicleplating2" },
        { name: "VehicleAndShipPlatingLevel3", src: "bi_vehicleplating3" },
        { name: "ShipWeaponsLevel1", src: "bi_shipweapons1" },
        { name: "ShipWeaponsLevel2", src: "bi_shipweapons2" },
        { name: "ShipWeaponsLevel3", src: "bi_shipweapons3" },
        { name: "InfernalPreIgniter", src: "bi_infernal_preigniter" },
        { name: "ThorStrikeCanons", src: "bi_mmstrikecannons" },
        { name: "VehicleWeaponsLevel1", src: "bi_vehicleweapons1" },
        { name: "VehicleWeaponsLevel2", src: "bi_vehicleweapons2" },
        { name: "VehicleWeaponsLevel3", src: "bi_vehicleweapons3" },
        { name: "VehicleAndShipWeaponsLevel1", src: "bi_vehicleweapons1" },
        { name: "VehicleAndShipWeaponsLevel2", src: "bi_vehicleweapons2" },
        { name: "VehicleAndShipWeaponsLevel3", src: "bi_vehicleweapons3" },
        { name: "CloakingField", src: "bi_cloakingfield" },
        { name: "StimPack", src: "bi_stimpack" },
        { name: "TransformationServos", src: "bi_transformation_servos" },
        //{ name:"SeekerMissile", src: "seekermissile" }, //not needed any more
        { name: "PersonalCloaking", src: "bi_personalcloaking" },
        { name: "BuildingArmor", src: "bi_buildingarmor" },
        { name: "WeaponRefit", src: "bi_weaponrefit" },
        { name: "MoebiusReactor", src: "bi_moebiusreactor" },
        { name: "NitroPacks", src: "bi_nitropacks" },
        { name: "DrillingClaws", src: "bi_drillingclaws" },
        { name: "HyperflightRotors", src: "bi_hyperflightrotors" },
        { name: "HighCapacityFuelTanks", src: "bi_highcapacityfueltanks" },
        { name: "MagFieldAccelerator", src: "bi_magfieldaccelerator" },
        { name: "ExplosiveShrapnelShells", src: "bi_explosiveshrapnelshells" },
        { name: "RecalibratedExplosives", src: "bi_recalibratedexplosives" },
        { name: "AdvancedBallistics", src: "bi_advancedballistics" },
        { name: "RapidFireLaunchers", src: "bi_rapidfirelaunchers" },
        { name: "SmartServos", src: "bi_smartservos" },
        { name: "EnhancedMunitions", src: "bi_enhancedmunitions" },

        //Protoss buildings
        { name: "Nexus", src: "bi_nexus" },
        { name: "Pylon", src: "bi_pylon" },
        { name: "DarkShrine", src: "bi_darkshrine" },
        { name: "RoboticsFacility", src: "bi_roboticsfacility" },
        { name: "TemplarArchives", src: "bi_templararchives" },
        { name: "CyberneticsCore", src: "bi_cyberneticscore" },
        { name: "RoboticsBay", src: "bi_roboticsbay" },
        { name: "Stargate", src: "bi_stargate" },
        { name: "TwilightCouncil", src: "bi_twilightcouncil" },
        { name: "PhotonCanon", src: "bi_photoncannon" },
        { name: "Forge", src: "bi_forge" },
        { name: "Assimilator", src: "bi_assimilator" },
        { name: "Gateway", src: "bi_gateway" },
        { name: "FleetBeacon", src: "bi_fleetbeacon" },
        { name: "ShieldBattery", src: "bi_shieldbattery" },
        //Protoss units
        { name: "ArchonFromTwoHT", src: "bi_archon" },
        { name: "Zealot", src: "bi_zealot" },
        { name: "WarpInHighTemplar", src: "bi_warphightemplar" },
        { name: "Phoenix", src: "bi_phoenix" },
        { name: "ArchonFromOneHTandOneDT", src: "bi_archon" },
        { name: "Colossus", src: "bi_colossus" },
        { name: "ArchonFromTwoDT", src: "bi_archon" },
        { name: "WarpInSentry", src: "bi_warpsentry" },
        { name: "Sentry", src: "bi_sentry" },
        { name: "HighTemplar", src: "bi_hightemplar" },
        { name: "WarpInStalker", src: "bi_warpstalker" },
        { name: "Probe", src: "bi_probe" },
        { name: "Observer", src: "bi_observer" },
        { name: "WarpInZealot", src: "bi_warpzealot" },
        { name: "Mothership", src: "bi_mothership" },
        { name: "DarkTemplar", src: "bi_darktemplar" },
        { name: "WarpInDarkTemplar", src: "bi_warpdarktemplar" },
        { name: "WarpPrism", src: "bi_warpprism" },
        { name: "VoidRay", src: "bi_voidray" },
        { name: "Carrier", src: "bi_carrier" },
        { name: "Stalker", src: "bi_stalker" },
        { name: "Immortal", src: "bi_immortal" },
        { name: "Oracle", src: "bi_oracle" },
        { name: "MothershipCore", src: "bi_mothership_core" },
        { name: "Tempest", src: "bi_tempest" },
        { name: "Disruptor", src: "bi_disruptor" },
        { name: "Adept", src: "bi_adept" },
        { name: "WarpInAdept", src: "bi_warpadept" },
        //Protoss specials
        { name: "Chronoboost", src: "bi_chronoboost" },
        { name: "MineralProbe", src: "bi_mineralprobe" },
        { name: "SwitchToWarpgate", src: "bi_warpgate" },
        { name: "SwitchToGateway", src: "bi_gateway" },
        { name: "GoOutProbe", src: "bi_scout_out_protoss" },
        { name: "ReturnProbe", src: "bi_scout_in_protoss" },
        { name: "GasProbe", src: "bi_gasprobe" },
        { name: "StartIdle", src: "bi_startidle" },
        { name: "StopIdleIn3Seconds", src: "bi_stopidle3" },
        { name: "StopIdleIn5Seconds", src: "bi_stopidle5" },
        { name: "StopIdleIn10Seconds", src: "bi_stopidle10" },
        //Protoss upgrades
        { name: "AirArmorLevel1", src: "bi_airarmorlevel1" },
        { name: "AirArmorLevel2", src: "bi_airarmorlevel2" },
        { name: "AirArmorLevel3", src: "bi_airarmorlevel3" },
        { name: "GroundArmorLevel1", src: "bi_groundarmorlevel1" },
        { name: "GroundArmorLevel2", src: "bi_groundarmorlevel2" },
        { name: "GroundArmorLevel3", src: "bi_groundarmorlevel3" },
        { name: "ShieldsLevel1", src: "bi_shieldslevel1" },
        { name: "ShieldsLevel2", src: "bi_shieldslevel2" },
        { name: "ShieldsLevel3", src: "bi_shieldslevel3" },
        { name: "GraviticDrive", src: "bi_graviticdrive" },
        { name: "GraviticBoosters", src: "bi_graviticbooster" },
        { name: "Charge", src: "bi_charge" },
        { name: "ResonatingGlaves", src: "bi_resonatingglaves" },
        { name: "WarpgateUpgrade", src: "bi_warpgateupgrade" },
        { name: "PsionicStorm", src: "bi_psionicstorm" },
        { name: "GravitonCatapult", src: "bi_gravitoncatapult" },
        { name: "GroundWeaponsLevel3", src: "bi_groundweaponslevel3" },
        { name: "GroundWeaponsLevel2", src: "bi_groundweaponslevel2" },
        { name: "GroundWeaponsLevel1", src: "bi_groundweaponslevel1" },
        { name: "Hallucination", src: "bi_hallucination" },
        { name: "Blink", src: "bi_blink" },
        { name: "ExtendedThermalLance", src: "bi_extendedthermallance" },
        { name: "AirWeaponsLevel2", src: "bi_airweaponslevel2" },
        { name: "AirWeaponsLevel1", src: "bi_airweaponslevel1" },
        { name: "AirWeaponsLevel3", src: "bi_airweaponslevel3" },
        { name: "AnionPulseCrystals", src: "bi_anionpulsecrystals" },
        { name: "ShadowStride", src: "bi_shadowstride" },

        //Zerg buildings
        { name: "Spire", src: "bi_spire" },
        { name: "Hive", src: "bi_hive" },
        { name: "GreaterSpire", src: "bi_greaterspire" },
        { name: "SporeCrawler", src: "bi_sporecrawler" },
        { name: "HydraliskDen", src: "bi_hydraliskden" },
        { name: "NydusNetwork", src: "bi_nydusnetwork" },
        { name: "EvolutionChamber", src: "bi_evolutionchamber" },
        { name: "SpineCrawler", src: "bi_spinecrawler" },
        { name: "InfestationPit", src: "bi_infestationpit" },
        { name: "Lair", src: "bi_lair" },
        { name: "RoachWarren", src: "bi_roachwarren" },
        { name: "Hatchery", src: "bi_hatchery" },
        { name: "MacroHatchery", src: "bi_macrohatchery" },
        { name: "UltraliskCavern", src: "bi_ultraliskcavern" },
        { name: "Extractor", src: "bi_extractor" },
        { name: "SpawningPool", src: "bi_spawningpool" },
        { name: "BanelingNest", src: "bi_banelingnest" },
        { name: "LurkerDen", src: "bi_lurkerden" },
        //Zerg units
        { name: "Queen", src: "bi_queen" },
        { name: "Mutalisk", src: "bi_mutalisk" },
        { name: "Corruptor", src: "bi_corruptor" },
        { name: "Baneling", src: "bi_baneling" },
        { name: "Roach", src: "bi_roach" },
        { name: "Zergling", src: "bi_zergling" },
        { name: "Ultralisk", src: "bi_ultralisk" },
        { name: "Drone", src: "bi_drone" },
        { name: "Infestor", src: "bi_infestor" },
        { name: "Overlord", src: "bi_overlord" },
        { name: "Hydralisk", src: "bi_hydralisk" },
        { name: "Overseer", src: "bi_overseer" },
        { name: "Viper", src: "bi_viper" },
        { name: "SwarmHost", src: "bi_swarm_host" },
        { name: "Broodlord", src: "bi_broodlord" },
        { name: "Lurker", src: "bi_lurker" },
        { name: "Ravager", src: "bi_ravager" },
        { name: "NydusWorm", src: "bi_nydusworm" },
        //Zerg specials
        { name: "InjectLarva", src: "bi_spawnlarva" },
        { name: "ReturnDrone", src: "bi_scout_in_zerg" },
        { name: "GoOutDrone", src: "bi_scout_out_zerg" },
        { name: "GasDrone", src: "bi_gasdrone" },
        { name: "SpawnCreepTumor", src: "bi_spawncreeptumor" },
        { name: "MineralDrone", src: "bi_mineraldrone" },
        { name: "CancelExtractor", src: "bi_cancelextractor" },
        //Zerg upgrades
        { name: "FlyerAttacksLevel3", src: "bi_flyerattacks3" },
        { name: "FlyerAttacksLevel1", src: "bi_flyerattacks1" },
        { name: "MetabolicBoost", src: "bi_metabolicboost" },
        { name: "FlyerAttacksLevel2", src: "bi_flyerattacks2" },
        { name: "MeleeAttacksLevel3", src: "bi_meleeattacks3" },
        { name: "AdrenalineGlands", src: "bi_adrenalglands" },
        { name: "VentralSacs", src: "bi_ventralsacs" },
        { name: "GlialReconstitution", src: "bi_glialreconstitution" },
        { name: "MissleAttacksLevel3", src: "bi_missileattacks3" },
        { name: "Burrow", src: "bi_burrow" },
        { name: "MissleAttacksLevel2", src: "bi_missileattacks2" },
        { name: "PathogenGlands", src: "bi_pathogenglands" },
        { name: "TunnelingClaws", src: "bi_tunnelingclaws" },
        { name: "GroundCarapaceLevel3", src: "bi_groundcarapace3" },
        { name: "FlyerCarapaceLevel1", src: "bi_flyercarapace1" },
        { name: "GroundCarapaceLevel1", src: "bi_groundcarapace1" },
        { name: "FlyerCarapaceLevel2", src: "bi_flyercarapace2" },
        { name: "PneumatizedCarapace", src: "bi_pneumatizedcarapace" },
        { name: "GroundCarapaceLevel2", src: "bi_groundcarapace2" },
        { name: "FlyerCarapaceLevel3", src: "bi_flyercarapace3" },
        { name: "NeuralParasite", src: "bi_neuralparasite" },
        { name: "MissleAttacksLevel1", src: "bi_missileattacks1" },
        { name: "ChitinousPlating", src: "bi_chitinousplating" },
        { name: "CentrifugalHooks", src: "bi_centrifugalhooks" },
        { name: "MeleeAtacksLevel2", src: "bi_meleeattacks2" },
        { name: "MeleeAttacksLevel1", src: "bi_meleeattacks1" },
        { name: "GroovedSpines", src: "bi_groovedspines" },
        { name: "MuscularAugments", src: "bi_muscularaugments" },
        { name: "EnduringLocusts", src: "bi_enduring_locusts" },
        { name: "FlyingLocusts", src: "bi_flyinglocusts" },
        { name: "MutateVentralSacs", src: "bi_morphventralsacs" },
        { name: "AdaptiveTalons", src: "bi_adaptivetalons" }
    ]);
})();