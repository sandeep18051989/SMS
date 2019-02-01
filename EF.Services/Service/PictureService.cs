using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public partial class PictureService : IPictureService
	{
		private readonly IRepository<Picture> _pictureRepository;
		private readonly IRepository<EventPicture> _eventPictureRepository;
		private readonly IRepository<BlogPicture> _blogPictureRepository;
		private readonly IRepository<ProductPicture> _productPictureRepository;
		private readonly IRepository<NewsPicture> _newsPictureRepository;
        public PictureService(IRepository<Picture> pictureRepository, 
            IRepository<EventPicture> eventPictureRepository, 
            IRepository<BlogPicture> blogPictureRepository, 
            IRepository<ProductPicture> productPictureRepository, 
            IRepository<NewsPicture> newsPictureRepository)
		{
			this._pictureRepository = pictureRepository;
			this._eventPictureRepository = eventPictureRepository;
			this._blogPictureRepository = blogPictureRepository;
			this._productPictureRepository = productPictureRepository;
			this._newsPictureRepository = newsPictureRepository;
		}
		#region IPictureService Members

		public void Insert(Picture picture)
		{
			_pictureRepository.Insert(picture);
		}

		public void Update(Picture picture)
		{
			_pictureRepository.Update(picture);
		}

		public void Delete(int id)
		{
			if (id == 0)
				throw new ArgumentNullException("role");

			_pictureRepository.Delete(id);
		}

		#endregion

		#region Methods

		public IList<Picture> GetAllPictures()
		{
			return _pictureRepository.Table.OrderByDescending(a => a.DisplayOrder).ToList();
		}
		public IList<Picture> GetPictures(bool active = true)
		{
			if (active)
				return _pictureRepository.Table.OrderBy(a => a.DisplayOrder).ToList();

			return _pictureRepository.Table.OrderBy(a => a.DisplayOrder).ToList();

		}
		public IList<Picture> GetAllPicturesByUser(int userId)
		{
			if (userId > 0)
			{
				return _pictureRepository.Table.Where(a => a.UserId == userId).OrderBy(a => a.DisplayOrder).ToList();
			}

			return null;
		}
		public Picture GetHomeLogo()
		{
			var homelogo = _pictureRepository.GetAll().FirstOrDefault(a => a.IsActive && a.IsLogo);
			return homelogo;
		}
		public Picture GetPictureById(int pictureId)
		{
			if (pictureId == 0)
				return null;

			return _pictureRepository.GetAll().FirstOrDefault(a => a.Id == pictureId);
		}
		public virtual string GetDefaultPictureSrc()
		{
			string defaultImageFileName = "";
			var imagesDirectoryPath = HttpContext.Current.Server.MapPath("~/Content/images/");
			var filePath = Path.Combine(imagesDirectoryPath, defaultImageFileName);
			if (!System.IO.File.Exists(filePath))
			{
				return "";
			}

			return filePath;
		}
		public Image RezizeImage(Image img, int maxWidth, int maxHeight)
		{
			if (img.Height < maxHeight && img.Width < maxWidth) return img;
			using (img)
			{
				Double xRatio = (double)img.Width / maxWidth;
				Double yRatio = (double)img.Height / maxHeight;
				Double ratio = Math.Max(xRatio, yRatio);
				int nnx = (int)Math.Floor(img.Width / ratio);
				int nny = (int)Math.Floor(img.Height / ratio);
				Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
				using (Graphics gr = Graphics.FromImage(cpy))
				{
					gr.Clear(Color.Transparent);

					// This is said to give best quality when resizing images
					gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

					gr.DrawImage(img,
						 new Rectangle(0, 0, nnx, nny),
						 new Rectangle(0, 0, img.Width, img.Height),
						 GraphicsUnit.Pixel);
				}
				return cpy;
			}

		}
		public MemoryStream BytearrayToStream(byte[] arr)
		{
			return new MemoryStream(arr, 0, arr.Length);
		}
		public void TogglePicture(int id)
		{
			if (id == 0)
				throw new ArgumentNullException("picture");

			var _picture = _pictureRepository.Table.Where(x => x.Id == id).FirstOrDefault();
			if (_picture != null)
			{
				_picture.IsActive = !_picture.IsActive;
				_pictureRepository.Update(_picture);
			}

		}

		#region Picture Definetions
		public void InsertEventPicture(EventPicture eventPicture)
		{
			_eventPictureRepository.Insert(eventPicture);
		}
		public void UpdateEventPicture(EventPicture eventPicture)
		{
			_eventPictureRepository.Update(eventPicture);
		}
		public void DeleteEventPicture(int id)
		{
			var eventPicture = _eventPictureRepository.GetByID(id);
			if (eventPicture != null)
			{
				var picture = _pictureRepository.GetByID(eventPicture.PictureId);
				if (picture != null)
					_pictureRepository.Delete(picture);
			}
		}
		public EventPicture GetEventPictureById(int id)
		{
			if (id == 0)
				throw new Exception("Event picture id is missing");

			return _eventPictureRepository.GetByID(id);

        }
		public EventPicture GetEventPictureByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Event picture id is missing");

			return _eventPictureRepository.Table.FirstOrDefault(x => x.PictureId == id);

		}

		public void InsertBlogPicture(BlogPicture blogPicture)
		{
			_blogPictureRepository.Insert(blogPicture);
		}
		public void UpdateBlogPicture(BlogPicture blogPicture)
		{
			_blogPictureRepository.Update(blogPicture);
		}
		public void DeleteBlogPicture(int id)
		{
			var blogPicture = _blogPictureRepository.Table.FirstOrDefault(s => s.Id == id);
			if (blogPicture != null)
			{
				var picture = _pictureRepository.Table.FirstOrDefault(pic => pic.Id == blogPicture.PictureId);
				if (picture != null)
					_pictureRepository.Delete(picture);
			}
		}
		public BlogPicture GetBlogPictureById(int id)
		{
			if (id == 0)
				throw new Exception("Blog picture id is missing");

			return _blogPictureRepository.GetByID(id);

        }
		public BlogPicture GetBlogPictureByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Blog picture id is missing");

			return _blogPictureRepository.Table.FirstOrDefault(x => x.PictureId == id);

		}

		public void InsertProductPicture(ProductPicture productPicture)
		{
			_productPictureRepository.Insert(productPicture);
		}
		public void UpdateProductPicture(ProductPicture productPicture)
		{
			_productPictureRepository.Update(productPicture);
		}
		public void DeleteProductPicture(int id)
		{
			var productPicture = _productPictureRepository.GetByID(id);
            if (productPicture != null)
			{
				var picture = _pictureRepository.GetByID(productPicture.PictureId);
				if (picture != null)
					_pictureRepository.Delete(picture);
			}
		}
		public ProductPicture GetProductPictureById(int id)
		{
			if (id == 0)
				throw new Exception("Product picture id is missing");

			return _productPictureRepository.GetByID(id);

		}
		public ProductPicture GetProductPictureByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("Product picture id is missing");

			return _productPictureRepository.Table.FirstOrDefault(x => x.PictureId == id);

		}
		public ProductPicture GetDefaultProductPicture(int id)
		{
			if (id == 0)
				throw new Exception("Product id is missing!");

			var query = _productPictureRepository.Table.FirstOrDefault(x => x.ProductId == id && x.IsDefault);

			if (query == null)
				return _productPictureRepository.Table.FirstOrDefault(x => x.ProductId == id);
			else
				return query;

		}
		public void InsertNewsPicture(NewsPicture newsPicture)
		{
			_newsPictureRepository.Insert(newsPicture);
		}
		public void UpdateNewsPicture(NewsPicture newsPicture)
		{
			_newsPictureRepository.Update(newsPicture);
		}
		public void DeleteNewsPicture(int id)
		{
            var newsPicture = _newsPictureRepository.GetByID(id);
			if (newsPicture != null)
			{
				var picture = _pictureRepository.GetByID(newsPicture.PictureId);
				if (picture != null)
					_pictureRepository.Delete(picture);
			}
		}
		public NewsPicture GetNewsPictureById(int id)
		{
			if (id == 0)
				throw new Exception("News picture id is missing");

			return _newsPictureRepository.GetByID(id);

        }
		public NewsPicture GetNewsPictureByPictureId(int id)
		{
			if (id == 0)
				throw new Exception("News picture id is missing");

			return _newsPictureRepository.Table.FirstOrDefault(x => x.PictureId == id);

		}

		public EventPicture GetDefaultEventPicture(int id)
		{
			if (id == 0)
				throw new Exception("Event id is missing!");

			var query = _eventPictureRepository.Table.FirstOrDefault(x => x.EventId == id && x.IsDefault);

			if (query == null)
				return _eventPictureRepository.Table.FirstOrDefault(x => x.EventId == id);
			else
				return query;
		}

		public BlogPicture GetDefaultBlogPicture(int id)
		{
			if (id == 0)
				throw new Exception("Blog id is missing!");

			var query = _blogPictureRepository.Table.FirstOrDefault(x => x.BlogId == id && x.IsDefault);

			if (query == null)
				return _blogPictureRepository.Table.FirstOrDefault(x => x.BlogId == id);
			else
				return query;
		}

		public NewsPicture GetDefaultNewsPicture(int id)
		{
			if (id == 0)
				throw new Exception("Blog id is missing!");

			var query = _newsPictureRepository.Table.FirstOrDefault(x => x.NewsId == id && x.IsDefault);

			if (query == null)
				return _newsPictureRepository.Table.FirstOrDefault(x => x.NewsId == id);
			else
				return query;
		}

		public void ToggleEventPictureDefault(int id, int pictureid)
		{
			var allpictures = _eventPictureRepository.Table.Where(x => x.EventId == id).OrderBy(x => x.DisplayOrder).ToList();
			if (allpictures.Count > 0)
			{
				var firstPicture = allpictures.FirstOrDefault();
				foreach (var pic in allpictures)
				{
					if (pic.Id == pictureid)
					{
						pic.IsDefault = true;
					}
					else
					{
						pic.IsDefault = false;
					}
					_eventPictureRepository.Update(pic);
				}

				var defaultPicture = GetDefaultEventPicture(id);
				if (defaultPicture != null)
				{
					firstPicture.IsDefault = true;
					_eventPictureRepository.Update(firstPicture);
				}
			}
		}

		public void ToggleBlogPictureDefault(int id, int pictureid)
		{
			var allpictures = _blogPictureRepository.Table.Where(x => x.BlogId == id).OrderBy(x => x.DisplayOrder).ToList();
			if (allpictures.Count > 0)
			{
				var firstPicture = allpictures.FirstOrDefault();
				foreach (var pic in allpictures)
				{
					if (pic.Id == pictureid)
					{
						pic.IsDefault = true;
					}
					else
					{
						pic.IsDefault = false;
					}
					_blogPictureRepository.Update(pic);
				}

				var defaultPicture = GetDefaultBlogPicture(id);
				if (defaultPicture != null)
				{
					firstPicture.IsDefault = true;
					_blogPictureRepository.Update(firstPicture);
				}
			}
		}

		public void ToggleProductPictureDefault(int id, int pictureid)
		{
			var allpictures = _productPictureRepository.Table.Where(x => x.ProductId == id).OrderBy(x => x.DisplayOrder).ToList();
			if (allpictures.Count > 0)
			{
				var firstPicture = allpictures.FirstOrDefault();
				foreach (var pic in allpictures)
				{
					if (pic.Id == pictureid)
					{
						pic.IsDefault = true;
					}
					else
					{
						pic.IsDefault = false;
					}
					_productPictureRepository.Update(pic);
				}

				var defaultPicture = GetDefaultProductPicture(id);
				if (defaultPicture != null)
				{
					firstPicture.IsDefault = true;
					_productPictureRepository.Update(firstPicture);
				}
			}
		}

		public void ToggleNewsPictureDefault(int id, int pictureid)
		{
			var allpictures = _newsPictureRepository.Table.Where(x => x.NewsId == id).OrderBy(x => x.DisplayOrder).ToList();
			if (allpictures.Count > 0)
			{
				var firstPicture = allpictures.FirstOrDefault();
				foreach (var pic in allpictures)
				{
					if (pic.Id == pictureid)
					{
						pic.IsDefault = true;
					}
					else
					{
						pic.IsDefault = false;
					}
					_newsPictureRepository.Update(pic);
				}

				var defaultPicture = GetDefaultNewsPicture(id);
				if (defaultPicture != null)
				{
					firstPicture.IsDefault = true;
					_newsPictureRepository.Update(firstPicture);
				}
			}
		}

		public IList<EventPicture> GetEventPicturesByEvent(int id)
		{
			if (id == 0)
				throw new Exception("Event id is missing");

			return _eventPictureRepository.Table.Where(x => x.EventId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

        public IList<BlogPicture> GetBlogPictureByBlogId(int id)
		{
			if (id == 0)
				throw new Exception("Blog id is missing");

			return _blogPictureRepository.Table.Where(x => x.BlogId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

		public IList<ProductPicture> GetProductPictureByProductId(int id)
		{
			if (id == 0)
				throw new Exception("Product id is missing");

			return _productPictureRepository.Table.Where(x => x.ProductId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

		public IList<NewsPicture> GetNewsPictureByNewsId(int id)
		{
			if (id == 0)
				throw new Exception("News id is missing");

			return _newsPictureRepository.Table.Where(x => x.NewsId == id).OrderBy(x => x.DisplayOrder).ToList();

		}

		#endregion

		#endregion

		#region Utilities

		public byte[] scaleImage(Image image, int maxWidth, int maxHeight, bool padImage)
		{
			try
			{
				int newWidth;
				int newHeight;
				byte[] returnArray;

				//check if the image needs rotating (eg phone held vertical when taking a picture for example)
				foreach (var prop in image.PropertyItems)
				{
					if (prop.Id == 0x0112)
					{
						int rotateValue = image.GetPropertyItem(prop.Id).Value[0];
						RotateFlipType flipType = getRotateFlipType(rotateValue);
						image.RotateFlip(flipType);
						break;
					}
				}

				//apply padding if needed
				if (padImage == true)
				{
					image = applyPaddingToImage(image);
				}

				//check if the with or height of the image exceeds the maximum specified, if so calculate the new dimensions
				if (image.Width > maxWidth && image.Height > maxHeight)
				{
					var ratioX = (double)maxWidth / image.Width;
					var ratioY = (double)maxHeight / image.Height;
					var ratio = Math.Min(ratioX, ratioY);

					newWidth = (int)(image.Width * ratio);
					newHeight = (int)(image.Height * ratio);
				}
				else if (image.Height < maxHeight && image.Width < maxWidth)
				{
					newWidth = image.Width;
					newHeight = image.Height;
				}
				else
				{
					Double xRatio = (double)image.Width / maxWidth;
					Double yRatio = (double)image.Height / maxHeight;
					Double ratio = Math.Max(xRatio, yRatio);
					int nnx = (int)Math.Floor(image.Width / ratio);
					int nny = (int)Math.Floor(image.Height / ratio);
					Bitmap cpy = new Bitmap(nnx, nny, PixelFormat.Format32bppArgb);
					using (Graphics gr = Graphics.FromImage(cpy))
					{
						gr.Clear(Color.Transparent);

						// This is said to give best quality when resizing images
						gr.InterpolationMode = InterpolationMode.HighQualityBicubic;

						gr.DrawImage(image,
							 new Rectangle(0, 0, nnx, nny),
							 new Rectangle(0, 0, image.Width, image.Height),
							 GraphicsUnit.Pixel);
					}

					newWidth = cpy.Width;
					newHeight = cpy.Height;
				}

				//start with a new image
				var newImage = new Bitmap(newWidth, newHeight);

				//set the new resolution, 72 is usually good enough for displaying images on monitors
				newImage.SetResolution(72, 72);
				//or use the original resolution
				//newImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

				//resize the image
				using (var graphics = Graphics.FromImage(newImage))
				{
					graphics.CompositingMode = CompositingMode.SourceCopy;
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.SmoothingMode = SmoothingMode.HighQuality;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

					graphics.DrawImage(image, 0, 0, newWidth, newHeight);
				}
				image = newImage;

				//save the image to a memorystream to apply the compression level, higher compression = better quality = bigger images
				using (MemoryStream ms = new MemoryStream())
				{
					EncoderParameters encoderParameters = new EncoderParameters(1);
					encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 80L);
					image.Save(ms, getEncoderInfo("image/jpeg"), encoderParameters);

					//save the stream as byte array
					returnArray = ms.ToArray();
				}

				//cleanup
				image.Dispose();
				newImage.Dispose();

				return returnArray;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message.ToString());
			}
		}
		public ImageCodecInfo getEncoderInfo(string mimeType)
		{
			ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
			for (int j = 0; j < encoders.Length; ++j)
			{
				if (encoders[j].MimeType.ToLower() == mimeType.ToLower())
					return encoders[j];
			}
			return null;
		}
		public Image applyPaddingToImage(Image image)
		{
			//get the maximum size of the image dimensions
			int maxSize = Math.Max(image.Height, image.Width);
			Size squareSize = new Size(maxSize, maxSize);

			//create a new square image
			Bitmap squareImage = new Bitmap(squareSize.Width, squareSize.Height);

			using (Graphics graphics = Graphics.FromImage(squareImage))
			{
				//fill the new square with a color
				graphics.FillRectangle(Brushes.Red, 0, 0, squareSize.Width, squareSize.Height);

				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

				//put the original image on top of the new square
				graphics.DrawImage(image, (squareSize.Width / 2) - (image.Width / 2), (squareSize.Height / 2) - (image.Height / 2), image.Width, image.Height);
			}

			return squareImage;
		}
		public RotateFlipType getRotateFlipType(int rotateValue)
		{
			RotateFlipType flipType = RotateFlipType.RotateNoneFlipNone;

			switch (rotateValue)
			{
				case 1:
					flipType = RotateFlipType.RotateNoneFlipNone;
					break;
				case 2:
					flipType = RotateFlipType.RotateNoneFlipX;
					break;
				case 3:
					flipType = RotateFlipType.Rotate180FlipNone;
					break;
				case 4:
					flipType = RotateFlipType.Rotate180FlipX;
					break;
				case 5:
					flipType = RotateFlipType.Rotate90FlipX;
					break;
				case 6:
					flipType = RotateFlipType.Rotate90FlipNone;
					break;
				case 7:
					flipType = RotateFlipType.Rotate270FlipX;
					break;
				case 8:
					flipType = RotateFlipType.Rotate270FlipNone;
					break;
				default:
					flipType = RotateFlipType.RotateNoneFlipNone;
					break;
			}

			return flipType;
		}
		public Image byteArrayToImage(byte[] imgBytes)
		{
			using (MemoryStream imgStream = new MemoryStream(imgBytes))
			{
				return Image.FromStream(imgStream);
			}
		}

		#endregion

	}
}
