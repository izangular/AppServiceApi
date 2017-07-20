using IAZI.Model.Wizard.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAZI.Wizard.Core
{
    public static class MockObjectProvider
    {
        public static BuildQualityWizardInputA3 GetBuildQualityInputA3()
        {
            BuildQualityWizardInputA3 test = new BuildQualityWizardInputA3
            {
                ExternalKey = "Test1",
                HeatDistribution = HeatDistribution.AirCirculation,
                HeatGeneration = HeatGeneration.HeatPump,
                HeatInsulation = HeatInsulation.BadHeatingProtected,
                KitchenBathroom = KitchenBathroom.ExaltedInterior,
                Neighborhood = Neighborhood.AttractiveORFunctionallyDesigned,
                NoiseProtection = NoiseProtection.BadlyProtected,
                RoomDesign = RoomDesign.ExaltedInterior,
                RoomLayout = RoomLayout.FlexibleLayout,
                SolarCollector = SolarCollector.No
            };

            return test;
        }
        public static BuildConditionWizardInputA3 GetBuildConditionInputA3()
        {
            BuildConditionWizardInputA3 test = new BuildConditionWizardInputA3
            {
                ExternalKey = "Test1",
                ConditionLift = CondLift.FunctionalLowLimited

            };

            return test;
        }
        public static FlatConditionWizardInputA3 GetFlatConditionWizardInputA3()
        {
            FlatConditionWizardInputA3 test = new FlatConditionWizardInputA3
            {
                ExternalKey = "Test1",
                ConditionFloor = CondFloor.FunctionalLowLimited,
                ConditionKitchenBath = CondKitchenBath.FunctionalLowLimited,
                DampDamageFlat = DampDamageAp.No,
                ConditionOutdoor = Outdoor.GoodCondition,
                ConditionSecondaryRooms = SecondaryRooms.GoodCondition,
                ConditionTechnicalFacility = TechnicalFac.GoodCondition,
                ConditionWallCieling = CondWallCiel.GoodCondition,
                ConditionLiving = CondLiving.NewLessThanEqualToThreeyears
            };

            return test;
        }

        public static QualityFlatHouseWizardInputA3 GetQualityFlatHouseInputA3()
        {
            QualityFlatHouseWizardInputA3 test = new QualityFlatHouseWizardInputA3
            {
                ExternalKey = "Test1",
                FlatNoiseProtection = FlatNoiseProtection.AtticFlat,
                FlatPosition = FlatPos.FlatWithGreaterThanTwoExternalWalls,
                FlatReach = FlatReach.OnlyStairwellOrNoLift,
                FlatStairs = FlatStairs.MoreThanThree,
                FlatViewSolar = FlatViewSolar.AtticFlat,
                Floor = 1,
                TypeFlat = TypeFlat.AtticFlat
            };

            return test;
        }

        public static QualityMicroWizardInputA3 GetQualityMicroInputA3()
        {
            QualityMicroWizardInputA3 test = new QualityMicroWizardInputA3
            {
                ExternalKey = "Test1",
                DistanceHighSchool = DistanceHighschool.FiveToTen,
                DistancePlaySchool = DistancePlaySchool.FiveToTen,
                DistanceRecreation = DistanceRecreation.FiveToTen,
                DistanceSchool = DistanceSchool.FiveToTen,
                DistanceShopping = DistanceShopping.FiveToTen,
                DistanceTourismInfra = DistanceTourismInfra.GoodTransportConnections,
                DistanceTravelStop = DistanceTravelStop.FiveToTen,
                ElectricSmogSource = ElectricSmogSource.HighVoltagePylon,
                InstallationOperation = InstallationOperation.OnlyWinterOperation,
                NoiseExposure = NoiseExposure.FewNoise,
                NoiseSource = NoiseSource.AircraftNoise,
                PrimSec = PrimSec.No,
                SolarRadiation = SolarRadiation.Hardly,
                UniqueSituation = UniqueSituation.No,
                Vista = Vista.AttractiveView
            };

            return test;
        }

    }
}
