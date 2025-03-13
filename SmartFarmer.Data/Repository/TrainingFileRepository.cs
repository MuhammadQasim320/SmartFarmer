using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class TrainingFileRepository : ITrainingFileRepository
    {
        private readonly SmartFarmerContext _context;
        public TrainingFileRepository(SmartFarmerContext context)
        {
            _context = context;
        }
        public TrainingFile AddTrainingFile(TrainingFile trainingFile)
        {
            _context.TrainingFiles.Add(trainingFile);
            _context.SaveChanges();
            return trainingFile;
        }
        public IEnumerable<TrainingFile> GetTrainingFiles(Guid trainingId)
        {
            return _context.TrainingFiles.Where(a => a.TrainingId == trainingId).OrderByDescending(a => a.CreatedDate).ToList();
        }
        public TrainingFile GetTrainingFile(Guid trainingFileId)
        {
            return _context.TrainingFiles.Find(trainingFileId);
        }

        public bool IsTrainingFileExist(Guid trainingFileId)
        {
            return _context.TrainingFiles.Find(trainingFileId) == null ? false : true;
        }

        public TrainingFile UploadTrainingFile(TrainingFile trainingFile)
        {
            var res = _context.TrainingFiles.Add(trainingFile);
            if ( res == null)
            {
                return null;
            };
            _context.SaveChanges();
            return _context.TrainingFiles.FirstOrDefault(a => a.TrainingFileId == trainingFile.TrainingFileId);
        }
        public bool DeleteTrainingFile(Guid trainingFileId)
        {
            var res = _context.TrainingFiles.FirstOrDefault(a => a.TrainingFileId == trainingFileId);
            var result = _context.TrainingFiles.Remove(res);
            if (result == null)
            {
                return false;
            }
            _context.SaveChanges();
            return true;
        }

    }
}
