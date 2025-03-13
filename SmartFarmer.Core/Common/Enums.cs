namespace SmartFarmer.Core.Common
{
    public static class Enums
    {
        public enum ResponseStatusType
        {
            Error,
            Success,
        }
        public enum ApplicationUserTypeEnum
        {
            SuperAdmin=1,
            MasterAdmin=2,
            Admin = 3,
            Portal = 4,
            Operator = 5,
            Both = 6,
            Unicorn = 7,
        }
        public enum ActionTypeEnum
        {
            Mitigative = 1,
            Corrective = 2,
        }

        public enum CheckTypeEnum
        {
            PreCheck = 1,
            Service = 2,
        }

        public enum ApplicationUserStatusEnum
        {
            Live = 1,
            Blocked = 2,
        }
        public enum OperatorStatusEnum
        {
            Working = 1,
            Idle = 2,
        }
        public enum FrequancyTypeEnum
        {
            FixedDate = 1,
            Periodically = 2,
            Hours = 3,
            Distance = 4,
        }

        public enum InitialRiskAndAdjustedRiskEnum
        {
            Red1 = 1,
            Red2 = 2,
            Red3 = 3,
            Amber1 = 4,
            Amber2 = 5,
            Amber3 = 6,
            Green1 = 7,
            Green2 = 8,
            Green3 = 9,
        }

        public enum TrainigTypeEnum
        {
            SmartFarmer = 1,
            MS_Teams = 2,
            Webinar = 3,
            Internal = 4,
            External = 5,
            Online_Learning = 6,
        }

        public enum UnitsTypeEnum
        {
            Hours = 1,
            Miles = 2,
            KM = 3,
        }
        public enum MachineStatusEnum
        {
            Active = 1,
            Idle = 2,
            OutOfService = 3,
        }
        public enum EventTypeEnum
        {
            Clock_In = 1,
            Operate = 2,
            Pre_Check = 3,
            Idle = 4,
            Check_In = 5,
            SOS = 6,
            Fall = 7,
            Welfare = 8,
            Clock_Out = 9,
            Service = 10,
            Cancelled =11,
        }
        
        public enum IssueTypeEnum
        {
            Defect = 1,
            Warning = 2,
            Corrective = 3,
        }
        
        public enum IssueStatusEnum
        {
            Open = 1,
            Resolved = 2,
        } 
        
        public enum HazardTypeEnum
        {
            Point = 1,
            Polygon = 2,
            Polyline = 3,
        }

        public enum MobileActionTypeEnum
        {
            None = 1,
            Call = 2,
        }
        
        public enum CheckResultEnum
        {
            Safe = 1,
            UnSafe = 2,
        }
    }
}
