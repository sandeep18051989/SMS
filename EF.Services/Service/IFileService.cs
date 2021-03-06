﻿using System.Collections.Generic;
using EF.Core.Data;

namespace EF.Services.Service
{
	public partial interface IFileService
	{
		void Insert(File files);
		void Update(File files);
        void Delete(int id);

        IList<File> GetAllFiles();
		File GetFileById(int fileId);
		IList<File> GetFiles(bool active = true);
		IList<File> GetAllFilesByUser(int userId);
		IList<File> GetAllFilesByProduct(int productId);
        IList<File> GetAllFilesByTeacher(int teacherId);
        IList<File> GetAllFilesByStudent(int studentId);


    }
}
