using System.Collections.Generic;
using System.Linq;
using EF.Core;

namespace EF.Services.Service
{
	public partial class FileService : IFileService
    {
        private readonly IRepository<EF.Core.Data.File> _fileRepository;
        public FileService(IRepository<EF.Core.Data.File> fileRepository)
        {
            this._fileRepository = fileRepository;
        }
        #region IFileService Members

        public void Insert(EF.Core.Data.File files)
        {
            _fileRepository.Insert(files);
        }

        public void Update(EF.Core.Data.File files)
        {
            _fileRepository.Update(files);
        }

        #endregion

        public IList<EF.Core.Data.File> GetAllFiles()
        {
            return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();
        }

        public IList<EF.Core.Data.File> GetFiles(bool active = true)
        {
            if (active)
                return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();

            return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();

        }
       
        public IList<EF.Core.Data.File> GetAllFilesByUser(int userId)
        {
            if (userId > 0)
            {
                return _fileRepository.Table.Where(a => a.UserId == userId).OrderByDescending(a => a.CreatedOn).ToList();
            }

            return null;
        }

        public EF.Core.Data.File GetFileById(int fileId)
        {
            if (fileId == 0)
                return null;

            return _fileRepository.GetAll().FirstOrDefault(a => a.Id == fileId);
        }

    }
}
