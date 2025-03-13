using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;

namespace SmartFarmer.Core.Service
{
    public class TrainingFileService : ITrainingFileService
    {
        private ITrainingFileRepository _trainingFileRepository;
        public TrainingFileService(ITrainingFileRepository trainingFileRepository)
        {
            _trainingFileRepository = trainingFileRepository;
        }

        public TrainingFileViewModel AddTrainingFile(TrainingFileViewModel model)
        {
            return Mapper.MapTrainingFileEntityToTrainingFileViewModel(_trainingFileRepository.AddTrainingFile(Mapper.MapTrainingFileViewModelToTrainingFileEntity(model)));
        }

        public IEnumerable<TrainingFileViewModel> GetTrainingFiles(Guid trainingId)
        {
            return _trainingFileRepository.GetTrainingFiles(trainingId).Select(a => Mapper.MapTrainingFileEntityToTrainingFileViewModel(a)).ToList();

        }

        public TrainingFileViewModel GetTrainingFile(Guid trainingFileId)
        {
            return Mapper.MapTrainingFileEntityToTrainingFileViewModel(_trainingFileRepository.GetTrainingFile(trainingFileId));

        }

        public bool IsTrainingFileExist(Guid trainingFileId)
        {
            return _trainingFileRepository.IsTrainingFileExist(trainingFileId);
        }

        public TrainingFileViewModel UploadTrainingFile(TrainingFileViewModel model)
        {
            return Mapper.MapTrainingFileEntityToTrainingFileViewModel(_trainingFileRepository.UploadTrainingFile(Mapper.MapTrainingFileViewModelToTrainingFileEntity(model)));
        }

        public bool DeleteTrainingFile(Guid trainingFileId)
        {
            return _trainingFileRepository.DeleteTrainingFile(trainingFileId);
        }


    }
}
