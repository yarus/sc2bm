using System;
using System.Collections.Generic;
using System.Linq;
using SC2.DataManagers.Configurators;
using SC2.Entities;
using SC2.Entities.BuildItems;
using SC2.Entities.BuildItems.Requirements;
using SC2.Entities.BuildOrderProcessor;
using SC2BM.DomainModel;
using SC2BM.ServiceModel.BusinessServices;
using SC2BM.ServiceModel.Responses;
using BuildOrderInfo = SC2.PublicData.BuildOrderInfo;

namespace SC2BM.BusinessFacade.Services
{
    public class BuildProcessorService : IBuildProcessorService
    {
        private Dictionary<string, BuildOrderProcessorConfiguration> mProcConfigs = new Dictionary<string, BuildOrderProcessorConfiguration>();

        private BuildOrderProcessorConfiguration GetConfig(string sc2versionId, string faction, string versionsFolder)
        {
            var key = sc2versionId + faction;

            if (!mProcConfigs.ContainsKey(key))
            {
                var config = GenerateBuildManagerConfig(sc2versionId, (RaceEnum)Enum.Parse(typeof(RaceEnum), faction), versionsFolder);
                mProcConfigs.Add(key, config);
            }

            return mProcConfigs[key];
        }

        public ServiceListResponse<SC2.PublicData.BuildOrderInfo> ConvertBuildOrdersForMobile(IList<BuildOrder> list)
        {
            var result = new List<SC2.PublicData.BuildOrderInfo>();

            foreach (var buildOrder in list)
            {
                var buildInfo = Convert(buildOrder);

                result.Add(buildInfo);
            }

            return new ServiceListResponse<BuildOrderInfo>(result);
        }

        public ServiceResponse<BuildOrderInfo> ConvertBuildOrderForMobile(BuildOrder item)
        {
            var result = Convert(item);
            return new ServiceResponse<BuildOrderInfo>(result);
        }

        private BuildOrderInfo Convert(BuildOrder buildOrder)
        {
            var buildInfo = new BuildOrderInfo();
            buildInfo.BuildOrderItems = buildOrder.BuildItems;
            buildInfo.Description = buildOrder.Description;
            buildInfo.Name = buildOrder.Name;
            buildInfo.Race = buildOrder.Race;
            buildInfo.SC2VersionID = buildOrder.SC2VersionID;
            buildInfo.VsRace = buildOrder.VsRace;
            buildInfo.AddedDate = buildOrder.AddedDate;

            return buildInfo;
        }

        public ServiceListResponse<BuildOrderItemInfo> GetProcessedBuildItems(BuildOrder build, string versionsFolder)
        {
            var config = GetConfig(build.SC2VersionID, build.Race, versionsFolder);

            var processor = new BuildOrderProcessor(config);

            var convertedBuild = ConvertModelToEntity(build);

            if (convertedBuild == null)
            {
                throw new ApplicationException("Cannot convert build order!");
            }

            processor.LoadBuildOrder(convertedBuild);

            var result = GenerateBuildItems(processor.CurrentBuildOrder, config);

            return new ServiceListResponse<BuildOrderItemInfo>(result);
        }

        private BuildItemInfo ConvertEntityToInfo(BuildItemEntity entity)
        {
            var convertedItem = new BuildItemInfo
            {
                BuildTime = entity.BuildTimeInSeconds,
                CostGas = entity.CostGas,
                CostMinerals = entity.CostMinerals,
                CostSupply = entity.CostSupply,
                DisplayName = entity.DisplayName,
                Name = entity.Name
            };

            return convertedItem;
        }

        public ServiceResponse<KnowladgeBaseInfo> GetKbData(string versionId, string faction, string versionsFolder)
        {
            var config = GetConfig(versionId, faction, versionsFolder);

            var result = new KnowladgeBaseInfo(versionId, faction);

            var buildItems = config.BuildItemsDictionary.GetList();

            foreach (var item in buildItems)
            {
                if (item.Name.Contains("OnReactor") || item.DisplayName.Contains("on Reactor"))
                {
                    continue;
                }

                if (item.ItemType == BuildItemTypeEnum.Unit)
                {
                    result.Units.Add(ConvertEntityToInfo(item));
                }
                else if (item.ItemType == BuildItemTypeEnum.Building)
                {
                    result.Buildings.Add(ConvertEntityToInfo(item));
                }
                else if (item.ItemType == BuildItemTypeEnum.Upgrade)
                {
                    result.Upgrades.Add(ConvertEntityToInfo(item));
                }
            }

            ProcessRequirementsForList(result.Units, config);
            ProcessRequirementsForList(result.Buildings, config);
            ProcessRequirementsForList(result.Upgrades, config);

            return new ServiceResponse<KnowladgeBaseInfo>(result);
        }

        private void ProcessRequirementsForList(List<BuildItemInfo> list, BuildOrderProcessorConfiguration config)
        {
            var buildItems = config.BuildItemsDictionary.GetList();

            foreach (var item in list)
            {
                var buildItem = buildItems.First(p => p.Name == item.Name);

                if (!string.IsNullOrEmpty(buildItem.ProductionBuildingName))
                {
                    var prodBuildingName = buildItem.ProductionBuildingName;
                    if (prodBuildingName == "FreeGatewayForUnit" || prodBuildingName == "FreeWarpgateForUnit")
                    {
                        prodBuildingName = "Gateway";
                    }

                    var prodBuilding = buildItems.FirstOrDefault(p => p.Name == prodBuildingName);
                    if (prodBuilding != null)
                    {
                        item.Requirements.Add(ConvertEntityToInfo(prodBuilding));   
                    }

                    if (buildItem.Name.Contains("WarpIn"))
                    {
                        var wargateUpgrade = buildItems.FirstOrDefault(p => p.Name == "WarpgateUpgrade");
                        if (wargateUpgrade != null)
                        {
                            item.Requirements.Add(ConvertEntityToInfo(wargateUpgrade));
                        }
                    }
                }

                foreach (var requirement in buildItem.ProduceRequirements)
                {
                    if (requirement is StatBiggerOrEqualThenValueRequirement)
                    {
                        var reqTarget = ((StatBiggerOrEqualThenValueRequirement) requirement).TargetStatName;
                        var reqItem = item.Requirements.FirstOrDefault(p => p.Name == reqTarget);
                        if (reqItem == null)
                        {
                            var reqBuildItem = buildItems.FirstOrDefault(p => p.Name == reqTarget);
                            if (reqBuildItem != null)
                            {
                                item.Requirements.Add(ConvertEntityToInfo(reqBuildItem));   
                            }
                        }
                    }
                }
            }
        }

        private List<BuildOrderItemInfo> GenerateBuildItems(BuildOrderProcessorData data, BuildOrderProcessorConfiguration config)
        {
            var result = new List<BuildOrderItemInfo>();

            var items = data.GetBuildOrderItemsClone();

            foreach (var item in items)
            {
                var itemData = config.BuildItemsDictionary.GetItem(item.ItemName);
                
                var infoItem = new BuildOrderItemInfo
                {
                    Limit = item.StatisticsProvider.CurrentSupply,
                    Name = itemData.Name,
                    DisplayName = itemData.DisplayName,
                    StartedSecond = item.SecondInTimeLine,
                    FinishedSecond = item.FinishedSecond,
                    MaxLimit = item.StatisticsProvider.MaximumSupply,
                    ItemType = itemData.ItemType.ToString()
                };

                foreach (var stat in item.StatisticsProvider.CloneItemsCountDictionary())
                {
                    infoItem.AdditionalValues.Add(stat.Key, stat.Value);
                }

                result.Add(infoItem);
            }

            return result;
        }

        private BuildOrderProcessorConfiguration GenerateBuildManagerConfig(string versionID, RaceEnum race, string versionsFolder)
        {
            var configurator = new DataManagersJsonStorageConfigurator(versionsFolder + "//");

            var version = configurator.GetSC2VersionsManager().GetVersion(versionID);

            var raceSettings = version.RaceSettingsDictionary.GetRaceSettings(race);

            var boManagerConfig = new BuildOrderProcessorConfiguration
            {
                BuildItemsDictionary = raceSettings.ItemsDictionary,
                BuildManagerModules = raceSettings.Modules,
                GlobalConstants = version.GlobalConstants,
                Race = race,
                RaceConstants = raceSettings.Constants,
                SC2VersionID = version.VersionID
            };

            return boManagerConfig;
        }

        private BuildOrderEntity ConvertModelToEntity(BuildOrder build)
        {
            if (build == null)
            {
                return null;
            }

            var result = new BuildOrderEntity
            {
                BuildOrderItems = build.BuildItems,
                Description = build.Description,
                Name = build.Name,
                Race = (RaceEnum) Enum.Parse(typeof (RaceEnum), build.Race),
                SC2VersionID = build.SC2VersionID,
                VsRace = (RaceEnum) Enum.Parse(typeof (RaceEnum), build.VsRace)
            };

            return result;
        }
    }
}