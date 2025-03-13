using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface ITrainingFileService
    {
        TrainingFileViewModel AddTrainingFile(TrainingFileViewModel model);
        IEnumerable<TrainingFileViewModel> GetTrainingFiles(Guid trainingId);
        TrainingFileViewModel GetTrainingFile(Guid trainingFileId);
        bool IsTrainingFileExist(Guid trainingFileId);
        TrainingFileViewModel UploadTrainingFile(TrainingFileViewModel model);
        bool DeleteTrainingFile(Guid trainingFileId);


    }
}
