using SmartFarmer.Domain.Model;

namespace SmartFarmer.Domain.Interface
{
    public interface ITrainingFileRepository
    {
        TrainingFile AddTrainingFile(TrainingFile trainingFile);
        IEnumerable<TrainingFile> GetTrainingFiles(Guid trainingId);
        TrainingFile GetTrainingFile(Guid trainingFileId);
        bool IsTrainingFileExist(Guid trainingFileId);
        TrainingFile UploadTrainingFile(TrainingFile trainingFile);
        bool DeleteTrainingFile(Guid trainingFileId);

    }
}
