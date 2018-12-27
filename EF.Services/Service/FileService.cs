using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public partial class FileService : IFileService
	{
		private readonly IRepository<File> _fileRepository;
		private readonly IRepository<Product> _productRepository;
		public FileService(IRepository<File> fileRepository, IRepository<Product> productRepository)
		{
			this._fileRepository = fileRepository;
			this._productRepository = productRepository;
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

        public void Delete(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("File");

            _fileRepository.Delete(id);
        }

        #endregion

        public IList<File> GetAllFiles()
		{
			return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();
		}

		public IList<File> GetAllFilesByProduct(int productId)
		{
			return _productRepository.GetByID(productId)?.Files.ToList();
		}

		public IList<File> GetFiles(bool active = true)
		{
			if (active)
				return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();

			return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();

		}

		public IList<File> GetAllFilesByUser(int userId)
		{
			if (userId > 0)
			{
				return _fileRepository.Table.Where(a => a.UserId == userId).OrderByDescending(a => a.CreatedOn).ToList();
			}

			return null;
		}

        public IList<File> GetAllFilesByStudent(int studentId)
        {
            if (studentId == 0)
                throw new System.Exception("Student Id Is Missing!");

            return _fileRepository.Table.Where(a => a.Students.Any(y => y.Id == studentId)).ToList();
        }

        public IList<File> GetAllFilesByTeacher(int teacherId)
        {
            if (teacherId == 0)
                throw new System.Exception("Student Id Is Missing!");

            return _fileRepository.Table.Where(a => a.Teachers.Any(y => y.Id == teacherId)).ToList();
        }

        public File GetFileById(int fileId)
		{
			if (fileId == 0)
				return null;

			return _fileRepository.GetAll().FirstOrDefault(a => a.Id == fileId);
		}

	}
}
