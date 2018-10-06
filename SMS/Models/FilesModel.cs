using EF.Core.Data;
using EF.Services;
using System;

namespace SMS.Models
{
    public partial class FilesModel : BaseEntityModel
    {
	    public int FileId { get; set; }
	    public string Src { get; set; }
	    public string Title { get; set; }
	    public string Type { get; set; }
	    public decimal Size { get; set; }
    }
}