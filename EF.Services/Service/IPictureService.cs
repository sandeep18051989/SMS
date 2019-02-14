using System.Collections;
using System.Collections.Generic;
using EF.Core.Data;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace EF.Services.Service
{
	public partial interface IPictureService
	{
		#region Methods
		void Insert(Picture picture);
		void Update(Picture picture);
		void Delete(int id);
		Picture GetHomeLogo();
		Picture GetPictureById(int pictureId);
        IList<Picture> GetAllPictures(bool? onlyActive = null, bool? onlyOpenResource = null);

        IList<Picture> GetAllPicturesByUser(int userId, bool? onlyActive = null, bool? onlyOpenResource = null);
        string GetDefaultPictureSrc();
		MemoryStream BytearrayToStream(byte[] arr);
		Image ResizeImage(Image img, int maxWidth, int maxHeight);
		void TogglePicture(int id);

		#region Picture Definetions
		void InsertEventPicture(EventPicture newsPicture);
		void UpdateEventPicture(EventPicture newsPicture);
		void DeleteEventPicture(int id);
		EventPicture GetEventPictureById(int id);
		EventPicture GetEventPictureByPictureId(int id);
        Image ConvertBase64IntoImage(string base64Image);

        void InsertBlogPicture(BlogPicture newsPicture);
		void UpdateBlogPicture(BlogPicture newsPicture);
		void DeleteBlogPicture(int id);
		BlogPicture GetBlogPictureById(int id);
		BlogPicture GetBlogPictureByPictureId(int id);

		void InsertProductPicture(ProductPicture newsPicture);
		void UpdateProductPicture(ProductPicture newsPicture);
		void DeleteProductPicture(int id);
		ProductPicture GetProductPictureById(int id);
		ProductPicture GetProductPictureByPictureId(int id);

		void InsertNewsPicture(NewsPicture newsPicture);
		void UpdateNewsPicture(NewsPicture newsPicture);
		void DeleteNewsPicture(int id);
		NewsPicture GetNewsPictureById(int id);
		NewsPicture GetNewsPictureByPictureId(int id);

		EventPicture GetDefaultEventPicture(int eventId);

		BlogPicture GetDefaultBlogPicture(int blogId);

		ProductPicture GetDefaultProductPicture(int productId);

		NewsPicture GetDefaultNewsPicture(int newsId);

		void ToggleEventPictureDefault(int id, int pictureid);

		void ToggleBlogPictureDefault(int id, int pictureid);

		void ToggleProductPictureDefault(int id, int pictureid);

		void ToggleNewsPictureDefault(int id, int pictureid);

		IList<EventPicture> GetEventPicturesByEvent(int id);

		IList<BlogPicture> GetBlogPictureByBlogId(int id);

		IList<ProductPicture> GetProductPictureByProductId(int id);

		IList<NewsPicture> GetNewsPictureByNewsId(int id);

        void ToggleActiveStatusPicture(int id);

        #endregion

        #endregion

        #region Utilities

        byte[] scaleImage(Image image, int maxWidth, int maxHeight, bool padImage);

		ImageCodecInfo getEncoderInfo(string mimeType);
		Image applyPaddingToImage(Image image);
		RotateFlipType getRotateFlipType(int rotateValue);
		Image byteArrayToImage(byte[] imgBytes);
        string ImageToBase64String(Image bitmap, ImageFormat format);

        #endregion
    }
}
