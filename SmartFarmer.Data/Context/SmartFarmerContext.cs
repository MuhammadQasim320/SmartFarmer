using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SmartFarmer.Domain;
using SmartFarmer.Domain.Model;
//using Action = SmartFarmer.Domain.Model.Action;

namespace SmartFarmer.Data.Context;

public class SmartFarmerContext : IdentityDbContext<ApplicationUser>
{
    public SmartFarmerContext(DbContextOptions<SmartFarmerContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<Action> Actions { get; set; }
    //public virtual DbSet<ActionType> ActionTypes { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
    public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public virtual DbSet<ApplicationUserType> ApplicationUserTypes { get; set; }
    public virtual DbSet<ApplicationUserStatus> ApplicationUserStatuses { get; set; }
    public virtual DbSet<OperatorStatus> OperatorStatuses { get; set; }
    public virtual DbSet<CheckList> CheckLists { get; set; }
    public virtual DbSet<CheckListItem> CheckListItems { get; set; }
    public virtual DbSet<CheckType> CheckTypes { get; set; }
    public virtual DbSet<CheckListMachineMapping> CheckListMachineMappings { get; set; }
    public virtual DbSet<CheckListItemAnswerMapping> ChecListItemAnswerMappings { get; set; }
    public virtual DbSet<CheckResult> CheckResults { get; set; }
    //public virtual DbSet<CheckLogResult> CheckLogResults { get; set; }
    public virtual DbSet<Event> Events { get; set; }
    public virtual DbSet<EventType> EventTypes { get; set; }
    public virtual DbSet<EquipmentStatus> EquipmentStatuses { get; set; }
    public virtual DbSet<Farm> Farms { get; set; }
    public virtual DbSet<Field> Fields { get; set; }
    public virtual DbSet<FrequencyType> FrequencyTypes { get; set; }
    public virtual DbSet<HazardKey> HazardKeys { get; set; }
    public virtual DbSet<HazardKeyFieldMapping> HazardKeyFieldMappings { get; set; }
    public virtual DbSet<HazardType> HazardTypes { get; set; }
    public virtual DbSet<InitialRiskAndAdjustedRisk> InitialRiskAndAdjustedRisks { get; set; }
    public virtual DbSet<Issue> Issues { get; set; }
    public virtual DbSet<IssueType> IssueTypes { get; set; }
    public virtual DbSet<IssueStatus> IssueStatuses { get; set; }
    public virtual DbSet<IssueCategory> IssueCategories { get; set; }
    public virtual DbSet<IssueFile> IssueFiles { get; set; }
    //public virtual DbSet<IssueComment> IssueComments { get; set; }
    public virtual DbSet<Machine> Machines { get; set; }
    public virtual DbSet<MachineCategory> MachineCategorys { get; set; }
    public virtual DbSet<MachineStatus> MachineStatuses { get; set; }
    public virtual DbSet<MachineType> MachineTypes { get; set; }
    public virtual DbSet<OperatorAnswerMapping> OperatorAnswerMappings { get; set; }
    public virtual DbSet<MachineOperatorMapping> MachineOperatorMappings { get; set; }
    public virtual DbSet<RiskAssessment> RiskAssessments { get; set; }
    public virtual DbSet<RiskAssessmentFile> RiskAssessmentFiles { get; set; }
    public virtual DbSet<RiskAssessmentLog> RiskAssessmentLogs { get; set; }

    //public virtual DbSet<SmartCourse> SmartCourses { get; set; }
    public virtual DbSet<SmartQuestion> SmartQuestions { get; set; }
    public virtual DbSet<Training> Trainings { get; set; }
    public virtual DbSet<TrainingFile> TrainingFiles { get; set; }
    public virtual DbSet<TrainingRecord> TrainingRecords { get; set; }
    public virtual DbSet<TrainingOperatorMapping> TrainingOperatorMappings { get; set; }
    public virtual DbSet<TrainingRecordOperatorMapping> TrainingRecordOperatorMappings { get; set; }
    public virtual DbSet<TrainingType> TrainingTypes { get; set; }
    public virtual DbSet<UnitsType> UnitsTypes { get; set; }
    public virtual DbSet<UserGroup> UserGroups { get; set; }
    public virtual DbSet<WelfareRoutine> WelfareRoutines { get; set; }
    public virtual DbSet<AlarmAction> AlarmActions {  get; set; }
    public virtual DbSet<MobileActionType> MobileActionTypes {  get; set; }
    public virtual DbSet<Notification> Notifications {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<Action>(entity =>
        //{
        //    entity.ToTable("Action");

        //    entity.Property(e => e.ActionId).ValueGeneratedNever();

        //    entity.Property(e => e.Name).HasMaxLength(100);

        //    entity.HasOne(d => d.ActionType)
        //          .WithMany(p => p.Actions)
        //          .HasForeignKey(d => d.ActionTypeId)
        //          .HasConstraintName("FK_Action_ActionType");

        //    //entity.HasOne(d => d.RiskAssessment)
        //    //      .WithMany(p => p.Actions)
        //    //      .HasForeignKey(d => d.RiskAssessmentId)
        //    //      .HasConstraintName("FK_Action_RiskAssessment");
        //});

        //modelBuilder.Entity<ActionType>(entity =>
        //{
        //    entity.ToTable("ActionType");

        //    entity.Property(e => e.ActionTypeId).ValueGeneratedNever();

        //    entity.Property(e => e.Type).HasMaxLength(100);
        //});

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.ToTable("Answer");

            entity.Property(e => e.AnswerId).ValueGeneratedNever();

            entity.Property(e => e.Text).HasMaxLength(100);


            entity.HasOne(d => d.ApplicationUser)
              .WithMany(p => p.Answers)
              .HasForeignKey(d => d.CreatedBy)
              .HasConstraintName("FK_Answer_ApplicationUser");

            entity.HasOne(d => d.SmartQuestion)
                  .WithMany(p => p.Answers)
                  .HasForeignKey(d => d.SmartQuestionId)
                  .HasConstraintName("FK_Answer_SmartQuestion");
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("ApplicationUser");

            entity.Property(e => e.FirstName).HasMaxLength(100);

            entity.Property(e => e.LastName).HasMaxLength(100);

            entity.Property(e => e.ProfileImageName).HasMaxLength(100);

            entity.Property(e => e.ProfileImageLink).HasMaxLength(500);

            entity.Property(e => e.Mobile).HasMaxLength(100);

            entity.Property(e => e.HouseNameNumber).HasMaxLength(100);

            entity.Property(e => e.Street).HasMaxLength(100);

            entity.Property(e => e.Addressline2).HasMaxLength(100);

            entity.Property(e => e.Town).HasMaxLength(100);

            entity.Property(e => e.County).HasMaxLength(100);

            entity.Property(e => e.PostCode).HasMaxLength(100);

            entity.Property(e => e.Location).HasMaxLength(500);

            entity.Property(e => e.CreatedBy).HasMaxLength(100);


            entity.HasOne(d => d.ApplicationUserType)
                  .WithMany(p => p.ApplicationUsers)
                  .HasForeignKey(d => d.ApplicationUserTypeId)
                  .HasConstraintName("FK_ApplicationUser_ApplicationUserType");

            entity.HasOne(d => d.ApplicationUserStatus)
                  .WithMany(p => p.ApplicationUsers)
                  .HasForeignKey(d => d.ApplicationUserStatusId)
                  .HasConstraintName("FK_ApplicationUser_ApplicationUserStatus");
            
            entity.HasOne(d => d.UserGroup)
                  .WithMany(p => p.ApplicationUsers)
                  .HasForeignKey(d => d.UserGroupId)
                  .HasConstraintName("FK_ApplicationUser_UserGroup");

            entity.HasOne(d => d.MasterAdmin)
                 .WithMany(p => p.ApplicationUsers)
                 .HasForeignKey(d => d.MasterAdminId)
                 .HasConstraintName("FK_ApplicationUser_MasterAdmin");
            
            entity.HasOne(d => d.MainAdmin)
                 .WithMany(p => p.MainApplicationUsers)
                 .HasForeignKey(d => d.MainAdminId)
                 .HasConstraintName("FK_ApplicationUser_MainAdmin");
        });

        modelBuilder.Entity<ApplicationUserType>(entity =>
        {
            entity.ToTable("ApplicationUserType");

            entity.Property(e => e.ApplicationUserTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<ApplicationUserStatus>(entity =>
        {
            entity.ToTable("ApplicationUserStatus");

            entity.Property(e => e.ApplicationUserStatusId).ValueGeneratedNever();

            entity.Property(e => e.Status).HasMaxLength(100);

        });
        
        modelBuilder.Entity<OperatorStatus>(entity =>
        {
            entity.ToTable("OperatorStatus");

            entity.Property(e => e.OperatorStatusId).ValueGeneratedNever();

            entity.Property(e => e.Status).HasMaxLength(100);

        });

        modelBuilder.Entity<CheckList>(entity =>
        {
            entity.ToTable("CheckList");

            entity.Property(e => e.CheckListId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Frequency).HasMaxLength(100);

            entity.HasOne(d => d.FrequencyType)
                  .WithMany(p => p.CheckLists)
                  .HasForeignKey(d => d.FrequencyTypeId)
                  .HasConstraintName("FK_CheckList_FrequencyType");

            entity.HasOne(d => d.CheckType)
                  .WithMany(p => p.CheckLists)
                  .HasForeignKey(d => d.CheckTypeId)
                  .HasConstraintName("FK_CheckList_CheckType");

            entity.HasOne(d => d.MachineType)
                  .WithMany(p => p.CheckLists)
                  .HasForeignKey(d => d.MachineTypeId)
                  .HasConstraintName("FK_CheckList_MachineType");

            entity.HasOne(d => d.MachineType)
                  .WithMany(p => p.CheckLists)
                  .HasForeignKey(d => d.MachineTypeId)
                  .HasConstraintName("FK_CheckList_MachineType");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.CheckLists)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_CheckList_ApplicationUser");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.OperatorCheckLists)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_CheckList_Operator");
        });

        modelBuilder.Entity<CheckType>(entity =>
        {
            entity.ToTable("CheckType");

            entity.Property(e => e.CheckTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);

        });
        
        modelBuilder.Entity<CheckListMachineMapping>(entity =>
        {
            entity.ToTable("CheckListMachineMapping");

            entity.Property(e => e.CheckListMachineMappingId).ValueGeneratedNever();

            entity.HasOne(d => d.CheckList)
                  .WithMany(p => p.CheckListMachineMappings)
                  .HasForeignKey(d => d.CheckListId)
                  .HasConstraintName("FK_CheckListMachineMapping_CheckList");

            entity.HasOne(d => d.Machine)
                  .WithMany(p => p.CheckListMachineMappings)
                  .HasForeignKey(d => d.MachineId)
                  .HasConstraintName("FK_CheckListMachineMapping_Machine");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.CheckListMachineMappings)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_CheckListMachineMapping_ApplicationUser");

            entity.HasOne(d => d.CheckResult)
                  .WithMany(p => p.CheckListMachineMappings)
                  .HasForeignKey(d => d.ResultId)
                  .HasConstraintName("FK_CheckListMachineMapping_CheckResult");

        });
        
        modelBuilder.Entity<CheckListItemAnswerMapping>(entity =>
        {
            entity.ToTable("CheckListItemAnswerMapping");

            entity.Property(e => e.CheckListItemAnswerMappingId).ValueGeneratedNever();

            entity.Property(e => e.Answer).HasMaxLength(2000);

            entity.HasOne(d => d.CheckListItem)
                  .WithMany(p => p.ChecListItemAnswerMappings)
                  .HasForeignKey(d => d.CheckListItemId)
                  .HasConstraintName("FK_CheckListMachineMapping_CheckListItem");

            entity.HasOne(d => d.CheckListMachineMapping)
                  .WithMany(p => p.CheckListItemAnswerMappings)
                  .HasForeignKey(d => d.CheckListMachineMappingId)
                  .HasConstraintName("FK_CheckListItemAnswerMapping_CheckListMachineMapping");

            

        });

        modelBuilder.Entity<CheckListItem>(entity =>
        {
            entity.ToTable("CheckListItem");

            entity.Property(e => e.CheckListItemId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Instruction);

            entity.HasOne(d => d.CheckList)
                  .WithMany(p => p.CheckListItems)
                  .HasForeignKey(d => d.CheckListId)
                  .HasConstraintName("FK_CheckListItem_CheckList");

        });


        modelBuilder.Entity<CheckResult>(entity =>
        {
            entity.ToTable("CheckResult");
            entity.HasKey(e => e.ResultId);

            entity.Property(e => e.ResultId).ValueGeneratedNever();

            entity.Property(e => e.Result).HasMaxLength(50);

        });
        modelBuilder.Entity<Event>(entity =>
        {
            entity.ToTable("Event");

            entity.Property(e => e.EventId).ValueGeneratedNever();

            entity.HasOne(d => d.EventType)
                  .WithMany(p => p.Events)
                  .HasForeignKey(d => d.EventTypeId)
                  .HasConstraintName("FK_Event_EventType");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.Events)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_Event_ApplicationUser");

            entity.HasOne(d => d.Machine)
                  .WithMany(p => p.Events)
                  .HasForeignKey(d => d.MachineId)
                  .HasConstraintName("FK_Event_Machine");

            entity.HasOne(d => d.CheckListMachineMapping)
                  .WithMany(p => p.Events)
                  .HasForeignKey(d => d.CheckListMachineMappingId)
                  .HasConstraintName("FK_Event_CheckListMachineMapping");

        });

        modelBuilder.Entity<EventType>(entity =>
        {
            entity.ToTable("EventType");

            entity.Property(e => e.EventTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);

        });
        
        modelBuilder.Entity<EquipmentStatus>(entity =>
        {
            entity.ToTable("EquipmentStatus");

            entity.Property(e => e.EquipmentStatusId).ValueGeneratedNever();

            entity.Property(e => e.Status).HasMaxLength(100);

        });

        modelBuilder.Entity<Farm>(entity =>
        {
            entity.ToTable("Farm");

            entity.Property(e => e.FarmId).ValueGeneratedNever();

            entity.Property(e => e.FarmName).HasMaxLength(1000);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.Farms)
                  .HasForeignKey(d => d.MasterAdminId)
                  .HasConstraintName("FK_Farm_ApplicationUser");
            
            entity.HasOne(d => d.CreatedByUser)
                  .WithMany(p => p.CreatedByFarms)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_Farm_CreatedByUser");

        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.ToTable("Field");

            entity.Property(e => e.FieldId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(1000);

            entity.Property(e => e.Center).HasMaxLength(1000);

            entity.Property(e => e.Boundary).HasMaxLength(1000);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.Fields)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_Field_ApplicationUser");

            entity.HasOne(d => d.Farm)
                  .WithMany(p => p.Fields)
                  .HasForeignKey(d => d.FarmId)
                  .HasConstraintName("FK_Field_Farm");

        });

        modelBuilder.Entity<FrequencyType>(entity =>
        {
            entity.ToTable("FrequencyType");

            entity.Property(e => e.FrequencyTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);

        });

        modelBuilder.Entity<HazardKey>(entity =>
        {
            entity.ToTable("HazardKey");

            entity.Property(e => e.HazardKeyId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.Property(e => e.Color).HasMaxLength(400);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.HazardKeys)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_HazardKey_ApplicationUser");

            entity.HasOne(d => d.HazardType)
                  .WithMany(p => p.HazardKeys)
                  .HasForeignKey(d => d.HazardTypeId)
                  .HasConstraintName("FK_HazardKey_HazardType");
        });
        
        
        modelBuilder.Entity<HazardKeyFieldMapping>(entity =>
        {
            entity.ToTable("HazardKeyFieldMapping");

            entity.Property(e => e.Location).HasMaxLength(500);

            entity.Property(e => e.HazardKeyFieldMappingId).ValueGeneratedNever();

            entity.HasOne(d => d.HazardKey)
                  .WithMany(p => p.HazardKeyFieldMappings)
                  .HasForeignKey(d => d.HazardKeyId)
                  .HasConstraintName("FK_HazardKeyFieldMapping_HazardKey");

            entity.HasOne(d => d.Field)
                  .WithMany(p => p.HazardKeyFieldMappings)
                  .HasForeignKey(d => d.FieldId)
                  .HasConstraintName("FK_HazardKeyFieldMapping_Field");
        });



        modelBuilder.Entity<HazardType>(entity =>
        {
            entity.ToTable("HazardType");

            entity.Property(e => e.HazardTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);

        });

        modelBuilder.Entity<InitialRiskAndAdjustedRisk>(entity =>
        {
            entity.ToTable("InitialRiskAndAdjustedRisk");

            entity.Property(e => e.InitialRiskAndAdjustedRiskId).ValueGeneratedNever();

            entity.Property(e => e.RiskValue).HasMaxLength(100);

        });

        modelBuilder.Entity<Issue>(entity =>
        {
            entity.ToTable("Issue");

            entity.Property(e => e.IssueId).ValueGeneratedNever();

            entity.Property(e => e.IssueTitle).HasMaxLength(200);

            entity.Property(e => e.Description).HasMaxLength(4000);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.Issues)
                  .HasForeignKey(d => d.ResolvedBy)
                  .HasConstraintName("FK_Issue_ApplicationUser");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.OperatorIssues)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_Issue_Operator");

            entity.HasOne(d => d.IssueType)
                  .WithMany(p => p.Issues)
                  .HasForeignKey(d => d.IssueTypeId)
                  .HasConstraintName("FK_Issue_IssueType");

            entity.HasOne(d => d.IssueCategory)
                  .WithMany(p => p.Issues)
                  .HasForeignKey(d => d.IssueCategoryId)
                  .HasConstraintName("FK_Issue_IssueCategory");

            entity.HasOne(d => d.Machine)
                  .WithMany(p => p.Issues)
                  .HasForeignKey(d => d.MachineId)
                  .HasConstraintName("FK_Issue_Machine");

            entity.HasOne(d => d.RiskAssessmentLog)
                  .WithMany(p => p.Issues)
                  .HasForeignKey(d => d.RiskAssessmentLogId)
                  .HasConstraintName("FK_Issue_RiskAssessmentLog");

            entity.HasOne(d => d.IssueStatus)
                  .WithMany(p => p.Issues)
                  .HasForeignKey(d => d.IssueStatusId)
                  .HasConstraintName("FK_Issue_IssueStatus");

        });
        
        modelBuilder.Entity<IssueType>(entity =>
        {
            entity.ToTable("IssueType");

            entity.Property(e => e.IssueTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);

        });

        modelBuilder.Entity<IssueStatus>(entity =>
        {
            entity.ToTable("IssueStatus");

            entity.Property(e => e.IssueStatusId).ValueGeneratedNever();

            entity.Property(e => e.Status).HasMaxLength(100);

        });

        modelBuilder.Entity<IssueCategory>(entity =>
        {
            entity.ToTable("IssueCategory");

            entity.Property(e => e.IssueCategoryId).ValueGeneratedNever();

            entity.Property(e => e.Category).HasMaxLength(100);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.IssueCategories)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_IssueCategory_ApplicationUser");

        });

        modelBuilder.Entity<IssueFile>(entity =>
        {
            entity.ToTable("IssueFile");

            entity.Property(e => e.IssueFileId).ValueGeneratedNever();

            entity.Property(e => e.FileUniqueName).HasMaxLength(100);

            entity.Property(e => e.FileURL).HasMaxLength(100);

            entity.HasOne(d => d.Issue)
                  .WithMany(p => p.IssueFiles)
                  .HasForeignKey(d => d.IssueId)
                  .HasConstraintName("FK_IssueFile_Issue");

        });
        
        //modelBuilder.Entity<IssueComment>(entity =>
        //{
        //    entity.ToTable("IssueComment");

        //    entity.Property(e => e.IssueCommentId).ValueGeneratedNever();

        //    entity.Property(e => e.Comment);


        //    entity.HasOne(d => d.Issue)
        //          .WithMany(p => p.IssueComments)
        //          .HasForeignKey(d => d.IssueId)
        //          .HasConstraintName("FK_IssueComment_Issue");

        //    entity.HasOne(d => d.ApplicationUser)
        //          .WithMany(p => p.IssueComments)
        //          .HasForeignKey(d => d.CreatedBy)
        //          .HasConstraintName("FK_IssueComment_ApplicationUser");

        //});

        modelBuilder.Entity<Machine>(entity =>
        {
            entity.ToTable("Machine");

            entity.Property(e => e.MachineId).ValueGeneratedNever();

            entity.Property(e => e.MachineImage).HasMaxLength(100);

            entity.Property(e => e.MachineImageUniqueName).HasMaxLength(100);

            entity.Property(e => e.NickName).HasMaxLength(100);

            entity.Property(e => e.Make).HasMaxLength(100);

            entity.Property(e => e.Model).HasMaxLength(100);

            entity.Property(e => e.SerialNumber).HasMaxLength(100);

            entity.Property(e => e.QRCode).HasMaxLength(100);

            entity.Property(e => e.QRUniqueName).HasMaxLength(100);

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.ReasonOfServiceRemoval).HasMaxLength(4000);

            entity.Property(e => e.Description);

            entity.Property(e => e.WorkingIn).HasMaxLength(100);

            entity.Property(e => e.Location).HasMaxLength(500);

            entity.HasOne(d => d.MachineType)
                  .WithMany(p => p.Machines)
                  .HasForeignKey(d => d.MachineTypeId)
                  .HasConstraintName("FK_Machine_MachineType");

            entity.HasOne(d => d.MachineStatus)
                  .WithMany(p => p.Machines)
                  .HasForeignKey(d => d.MachineStatusId)
                  .HasConstraintName("FK_Machine_MachineStatus");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.Machines)
                  .HasForeignKey(d => d.ApplicationUserId)
                  .HasConstraintName("FK_Machine_ApplicationUser");

            entity.HasOne(d => d.MachineCategory)
                  .WithMany(p => p.Machines)
                  .HasForeignKey(d => d.MachineCategoryId)
                  .HasConstraintName("FK_Machine_MachineCategory");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.OperatorMachines)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_Machine_Operator");

            entity.HasOne(d => d.CheckResult)
                  .WithMany(p => p.Machines)
                  .HasForeignKey(d => d.ResultId)
                  .HasConstraintName("FK_Machine_CheckResult");

        });

        modelBuilder.Entity<MachineCategory>(entity =>
        {
            entity.ToTable("MachineCategory");

            entity.Property(e => e.MachineCategoryId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Description);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.MachineCategorys)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_MachineCategory_ApplicationUser");

        });

        modelBuilder.Entity<MachineStatus>(entity =>
        {
            entity.ToTable("MachineStatus");

            entity.Property(e => e.MachineStatusId).ValueGeneratedNever();

            entity.Property(e => e.Status).HasMaxLength(100);

        });

        modelBuilder.Entity<MachineType>(entity =>
        {
            entity.ToTable("MachineType");

            entity.Property(e => e.MachineTypeId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.UnitsType)
                  .WithMany(p => p.MachineTypes)
                  .HasForeignKey(d => d.UnitsTypeId)
                  .HasConstraintName("FK_MachineType_UnitsType");

            entity.HasOne(d => d.Training)
                  .WithMany(p => p.MachineTypes)
                  .HasForeignKey(d => d.TrainingId)
                  .HasConstraintName("FK_MachineType_Training");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.MachineTypes)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_MachineType_ApplicationUser");

            entity.HasOne(d => d.RiskAssessment)
                  .WithMany(p => p.MachineTypes)
                  .HasForeignKey(d => d.RiskAssessmentId)
                  .HasConstraintName("FK_MachineType_RiskAssessment");

        });

        modelBuilder.Entity<MachineOperatorMapping>(entity =>
        {
            entity.ToTable("MachineOperatorMapping");

            entity.Property(e => e.MachineOperatorMappingId).ValueGeneratedNever();

            entity.HasOne(d => d.Machine)
                  .WithMany(p => p.MachineOperatorMappings)
                  .HasForeignKey(d => d.MachineId)
                  .HasConstraintName("FK_MachineOperatorMapping_Machine");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.MachineOperatorMappings)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_MachineOperatorMapping_ApplicationUser");
        });
        //modelBuilder.Entity<CheckLogResult>(entity =>
        //{
        //    entity.ToTable("CheckLogResult");

        //    entity.Property(e => e.CheckLogResultId).ValueGeneratedNever();

        //    entity.Property(e => e.Result).HasMaxLength(100);

        //});


        modelBuilder.Entity<TrainingOperatorMapping>(entity =>
        {
            entity.ToTable("TrainingOperatorMapping");

            entity.Property(e => e.TrainingOperatorMappingId).ValueGeneratedNever();

            entity.HasOne(d => d.Training)
                  .WithMany(p => p.TrainingOperatorMappings)
                  .HasForeignKey(d => d.TrainingId)
                  .HasConstraintName("FK_TrainingOperatorMapping_Training");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.TrainingOperatorMappings)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_TrainingOperatorMapping_ApplicationUser");
        });
        
        modelBuilder.Entity<OperatorAnswerMapping>(entity =>
        {
            entity.ToTable("OperatorAnswerMapping");

            entity.Property(e => e.OperatorAnswerMappingId).ValueGeneratedNever();

            entity.HasOne(d => d.Answer)
                  .WithMany(p => p.OperatorAnswerMappings)
                  .HasForeignKey(d => d.AnswerId)
                  .HasConstraintName("FK_OperatorAnswerMapping_Answer");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.OperatorAnswerMappings)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_OperatorAnswerMapping_ApplicationUser");
        });



        modelBuilder.Entity<TrainingRecordOperatorMapping>(entity =>
        {
            entity.ToTable("TrainingRecordOperatorMapping");

            entity.Property(e => e.TrainingRecordOperatorMappingId).ValueGeneratedNever();

            entity.HasOne(d => d.TrainingRecord)
                  .WithMany(p => p.TrainingRecordOperatorMappings)
                  .HasForeignKey(d => d.TrainingRecordId)
                  .HasConstraintName("FK_TrainingRecordOperatorMapping_TrainingRecord");

            entity.HasOne(d => d.Operator)
                  .WithMany(p => p.TrainingRecordOperatorMappings)
                  .HasForeignKey(d => d.OperatorId)
                  .HasConstraintName("FK_TrainingRecordOperatorMapping_ApplicationUser");
        });


        modelBuilder.Entity<RiskAssessment>(entity =>
        {
            entity.ToTable("RiskAssessment");

            entity.Property(e => e.RiskAssessmentId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.RiskAssessments)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_RiskAssessment_ApplicationUser");

        });

        modelBuilder.Entity<RiskAssessmentFile>(entity =>
        {
            entity.ToTable("RiskAssessmentFile");

            entity.Property(e => e.RiskAssessmentFileId).ValueGeneratedNever();

            entity.Property(e => e.FileUrl).HasMaxLength(100);

            entity.Property(e => e.FileUniqueName).HasMaxLength(100);

            entity.Property(e => e.FileName).HasMaxLength(100);

            entity.HasOne(d => d.RiskAssessment)
                  .WithMany(p => p.RiskAssessmentFiles)
                  .HasForeignKey(d => d.RiskAssessmentId)
                  .HasConstraintName("FK_RiskAssessmentFile_RiskAssessment");

        });
        
        modelBuilder.Entity<RiskAssessmentLog>(entity =>
        {
            entity.ToTable("RiskAssessmentLog");

            entity.Property(e => e.RiskAssessmentLogId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.RiskAssessment)
                  .WithMany(p => p.RiskAssessmentLogs)
                  .HasForeignKey(d => d.RiskAssessmentId)
                  .HasConstraintName("FK_RiskAssessmentLog_RiskAssessment");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.RiskAssessmentLogs)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_RiskAssessmentLog_ApplicationUser");

            //entity.HasOne(d => d.Action)
            //      .WithMany(p => p.RiskAssessmentLogs)
            //      .HasForeignKey(d => d.ActionId)
            //      .HasConstraintName("FK_RiskAssessmentLog_Action");

            entity.HasOne(d => d.InitialRisk)
                  .WithMany(p => p.InitialRiskRiskAssessmentLogs)
                  .HasForeignKey(d => d.InitialRiskId)
                  .HasConstraintName("FK_RiskAssessmentLog_InitialRisk");

            entity.HasOne(d => d.AdjustedRisk)
                  .WithMany(p => p.AdjustedRiskRiskAssessmentLogs)
                  .HasForeignKey(d => d.AdjustedRiskId)
                  .HasConstraintName("FK_RiskAssessmentLog_AdjustedRisk");
        });

        //modelBuilder.Entity<SmartCourse>(entity =>
        //{
        //    entity.ToTable("SmartCourse");

        //    entity.Property(e => e.SmartCourseId).ValueGeneratedNever();

        //    entity.Property(e => e.Name).HasMaxLength(100);

        //    entity.HasOne(d => d.ApplicationUser)
        //          .WithMany(p => p.SmartCourses)
        //          .HasForeignKey(d => d.CreatedBy)
        //          .HasConstraintName("FK_SmartCourse_ApplicationUser");

        //});

        modelBuilder.Entity<SmartQuestion>(entity =>
        {
            entity.ToTable("SmartQuestion");

            entity.Property(e => e.SmartQuestionId).ValueGeneratedNever();



            entity.HasOne(d => d.Training)
                  .WithMany(p => p.SmartQuestions)
                  .HasForeignKey(d => d.TrainingId)
                  .HasConstraintName("FK_SmartQuestion_Training");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.SmartQuestions)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_SmartQuestion_ApplicationUser");

            //entity.HasOne(d => d.SmartCourse)
            //      .WithMany(p => p.SmartQuestions)
            //      .HasForeignKey(d => d.SmartCourseId)
            //      .HasConstraintName("FK_SmartQuestion_SmartCourse");

        });

        modelBuilder.Entity<Training>(entity =>
        {
            entity.ToTable("Training");

            entity.Property(e => e.TrainingId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Certification).HasMaxLength(100);

            entity.Property(e => e.Description);

            //entity.HasOne(d => d.SmartCourse)
            //      .WithMany(p => p.Trainings)
            //      .HasForeignKey(d => d.SmartCourseId)
            //      .HasConstraintName("FK_Training_SmartCourse");

            entity.HasOne(d => d.TrainingType)
                  .WithMany(p => p.Trainings)
                  .HasForeignKey(d => d.TrainingTypeId)
                  .HasConstraintName("FK_Training_TrainingType");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.Trainings)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_Training_ApplicationUser");

        });

        modelBuilder.Entity<TrainingFile>(entity =>
        {
            entity.ToTable("TrainingFile");

            entity.Property(e => e.TrainingFileId).ValueGeneratedNever();

            entity.Property(e => e.FileUrl).HasMaxLength(100);

            entity.Property(e => e.FileUniqueName).HasMaxLength(100);

            entity.Property(e => e.FileName).HasMaxLength(100);

            entity.HasOne(d => d.training)
                  .WithMany(p => p.TrainingFiles)
                  .HasForeignKey(d => d.TrainingId)
                  .HasConstraintName("FK_TrainingFile_Training");

        });

        modelBuilder.Entity<TrainingRecord>(entity =>
        {
            entity.ToTable("TrainingRecord");

            entity.Property(e => e.TrainingRecordId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.Property(e => e.Qualification).HasMaxLength(200);

            entity.HasOne(d => d.TrainingType)
                  .WithMany(p => p.TrainingRecords)
                  .HasForeignKey(d => d.TrainingTypeId)
                  .HasConstraintName("FK_TrainingRecord_TrainingType");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.TrainingRecords)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_TrainingRecord_ApplicationUser");

        });

        modelBuilder.Entity<TrainingType>(entity =>
        {
            entity.ToTable("TrainingType");

            entity.Property(e => e.TrainingTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);

        });

        modelBuilder.Entity<UnitsType>(entity =>
        {
            entity.ToTable("UnitsType");

            entity.Property(e => e.UnitsTypeId).ValueGeneratedNever();

            entity.Property(e => e.Units).HasMaxLength(100);

        });

        modelBuilder.Entity<UserGroup>(entity =>
        {
            entity.ToTable("UserGroup");

            entity.Property(e => e.UserGroupId).ValueGeneratedNever();

            entity.Property(e => e.GroupName);

            entity.Property(e => e.Description);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.UserGroups)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_UserGroup_ApplicationUser");

        });

        modelBuilder.Entity<WelfareRoutine>(entity =>
        {
            entity.ToTable("WelfareRoutine");

            entity.Property(e => e.WelfareRoutineId).ValueGeneratedNever();

            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.UserGroup)
                  .WithMany(p => p.WelfareRoutines)
                  .HasForeignKey(d => d.UserGroupId)
                  .HasConstraintName("FK_WelfareRoutine_UserGroup");

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.WelfareRoutines)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_WelfareRoutine_ApplicationUser");

        });

        modelBuilder.Entity<AlarmAction>(entity =>
        {
            entity.ToTable("AlarmAction");

            entity.Property(e => e.AlarmActionId).ValueGeneratedNever();

            entity.Property(e => e.MobileNumber).HasMaxLength(100);

            //entity.Property(e => e.SmsNumber).HasMaxLength(100);

            entity.HasOne(d => d.ApplicationUser)
                  .WithMany(p => p.AlarmActions)
                  .HasForeignKey(d => d.CreatedBy)
                  .HasConstraintName("FK_AlarmAction_ApplicationUser");

            entity.HasOne(d => d.MobileActionTypes)
                  .WithMany(p => p.AlarmActions)
                  .HasForeignKey(d => d.MobileActionTypeId)
                  .HasConstraintName("FK_MobileActionType_ApplicationUser");
        });

        modelBuilder.Entity<MobileActionType>(entity =>
        {
            entity.ToTable("MobileActionType");

            entity.Property(e => e.MobileActionTypeId).ValueGeneratedNever();

            entity.Property(e => e.Type).HasMaxLength(100);
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("Notification");

            entity.Property(e => e.NotificationId).ValueGeneratedNever();

            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.ToUser)
                  .WithMany(p => p.Notifications)
                  .HasForeignKey(d => d.ToId)
                  .HasConstraintName("FK_Notification_ApplicationUser");
        });


        base.OnModelCreating(modelBuilder);

    }

}
