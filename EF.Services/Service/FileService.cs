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

		#endregion

		public IList<File> GetAllFiles()
		{
			return _fileRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();
		}

		public IList<File> GetAllFilesByProduct(int productId)
		{
			return _productRepository.Table.FirstOrDefault(x => x.Id == productId)?.Files.ToList();
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

		public File GetFileById(int fileId)
		{
			if (fileId == 0)
				return null;

			return _fileRepository.GetAll().FirstOrDefault(a => a.Id == fileId);
		}

	}
}
