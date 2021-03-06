﻿using EF.Core.Data;
using System.Collections.Generic;

namespace EF.Services.Service
{
	public interface IProductService
	{
		void Insert(Product product);
		void Update(Product product);
		void Delete(int id);
        IList<Product> GetAllProducts(bool? onlyActive = null);
		Product GetProductById(int productId);
		IList<Product> GetAllProductByUser(int userId);

		Product GetProductByName(string productName);

        void ToggleActiveStatusProduct(int id);

        void DeleteProducts(IList<Product> products);

        IList<Product> GetProductsByIds(int[] roleIds);

        IList<Product> GetNewProducts(bool? onlyActive = null);

        IList<Product> GetUpcomingProducts(bool? onlyActive = null);

        #region Paging

        IPagedList<Product> GetPagedProducts(string keyword = null, int productcategoryid = 0, int productsubcategoryid = 0, int pageIndex = 0, int pageSize = int.MaxValue, bool? onlyActive = null);

        #endregion

        #region ProductCategory

        void DeleteCategory(ProductCategory productcategory);

		IPagedList<ProductCategory> GetAllCategories(string categoryName = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

		IList<ProductCategory> GetAllCategoriesByParentCategoryId(int parentCategoryId, bool showHidden = false);

		ProductCategory GetCategoryById(int categoryId);

		void InsertCategory(ProductCategory productcategory);

		void UpdateCategory(ProductCategory productcategory);

		void DeleteProductCategory(ProductCategoryMapping productCategoryMapping);

		IPagedList<ProductCategoryMapping> GetProductCategoriesByCategoryId(int categoryId, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

		IList<ProductCategoryMapping> GetProductCategoriesByProductId(int productId, bool showHidden = false);

		ProductCategoryMapping GetProductCategoryById(int productCategoryMappingId);

		void InsertProductCategoryMapping(ProductCategoryMapping productCategoryMapping);

		void UpdateProductCategoryMapping(ProductCategoryMapping productCategoryMapping);

		string[] GetNotExistingCategories(string[] categoryNames);

		IDictionary<int, int[]> GetProductCategoryIds(int[] productIds);
		IList<Product> GetProductsByVendor(int vendorId);

		#endregion
	}
}
