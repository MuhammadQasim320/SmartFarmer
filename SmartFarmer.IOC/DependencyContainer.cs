using Microsoft.Extensions.DependencyInjection;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.Service;
using SmartFarmer.Data.Repository;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.IOC
{
    public class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIssueCategoryService, IssueCategoryService>();
            services.AddScoped<IIssueCategoryRepository, IssueCategoryRepository>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IOperatorService, OperatorService>();
            services.AddScoped<IOperatorRepository, OperatorRepository>();
            services.AddScoped<IWelfareRoutineService, WelfareRoutineService>();
            services.AddScoped<IWelfareRoutineRepository, WelfareRoutineRepository>();
            //services.AddScoped<ISmartCourseService, SmartCourseService>();
            //services.AddScoped<ISmartCourseRepository, SmartCourseRepository>();
            services.AddScoped<IMasterService, MasterService>();
            services.AddScoped<IMasterRepository, MasterRepository>();
            services.AddScoped<ITrainingService, TrainingService>();
            services.AddScoped<ITrainingRepository, TrainingRepository>();
            services.AddScoped<IRiskAssessmentService, RiskAssessmentService>();
            services.AddScoped<IRiskAssessmentRepository, RiskAssessmentRepository>();
            services.AddScoped<IMachineService, MachineService>();
            services.AddScoped<IMachineRepository, MachineRepository>();
            services.AddScoped<IMachineTypeService, MachineTypeService>();
            services.AddScoped<IMachineTypeRepository, MachineTypeRepository>();
            services.AddScoped<ITrainingFileService, TrainingFileService>();
            services.AddScoped<ITrainingFileRepository, TrainingFileRepository>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<ICheckListService, CheckListService>();
            services.AddScoped<ICheckListRepository, CheckListRepository>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<IIssueRepository, IssueRepository>();
            services.AddScoped<ITimeSheetService, TimeSheetService>();
            services.AddScoped<ITimeSheetRepository, TimeSheetRepository>();
            services.AddScoped<IMachineCategoryService, MachineCategoryService>();
            services.AddScoped<IMachineCategoryRepository, MachineCategoryRepository>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IFieldService, FieldService>();
            services.AddScoped<IFieldRepository, FieldRepository>();
            services.AddScoped<IFarmService, FarmService>();
            services.AddScoped<IFarmRepository, FarmRepository>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();   
            services.AddScoped<IHazardKeyService, HazardKeyService>();
            services.AddScoped<IHazardKeyRepository, HazardKeyRepository>();
        }
    }
}
