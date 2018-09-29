﻿using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using System;
using System.Data;
using EF.Data;

namespace EF.Services.Service
{
    public class ProductService : IProductService
    {
        public readonly IRepository<Product> _productRepository;
        public readonly IRepository<ProductCategory> _productCategoryRepository;
        public readonly IRepository<ProductCategoryMapping> _productCategoryMappingRepository;
        public readonly IDataProvider _dataProvider;
        public readonly IDbContext _dbContext;
        public ProductService(IRepository<Product> productRepository, IRepository<ProductCategory> productCategoryRepository, IRepository<ProductCategoryMapping> productCategoryMappingRepository, IDataProvider dataProvider, IDbContext dbContext)
        {
            this._productRepository = productRepository;
            this._productCategoryRepository = productCategoryRepository;
            this._productCategoryMappingRepository = productCategoryMappingRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
        }

        #region Members

        public void Insert(Product product)
        {
            _productRepository.Insert(product);
        }

        public void Update(Product product)
        {
            _productRepository.Update(product);
        }
        public void Delete(int id)
        {
            _productRepository.Delete(id);
        }

        #endregion

        #region Methods

        public IList<Product> GetAllProduct()
        {
            return _productRepository.Table.OrderByDescending(a => a.CreatedOn).ToList();
        }

        public IList<Product> GetActiveProduct()
        {
            return _productRepository.Table.Where(a => a.IsActive == true).OrderByDescending(a => a.CreatedOn).ToList();
        }
        public Product GetProductById(int productId)
        {
            if (productId > 0)
                return _productRepository.Table.FirstOrDefault(a => a.Id == productId);

            return null;
        }
        public IList<Product> GetAllProductByUser(int userId)
        {
            if (userId > 0)
                return _productRepository.Table.Where(a => a.UserId == userId).ToList();

            return new List<Product>();
        }

        public Product GetProductByName(string productName)
        {
            if (!string.IsNullOrEmpty(productName))
                return _productRepository.Table.FirstOrDefault(a => a.Name.ToLower() == productName.ToLower() && a.IsDeleted == false);

            return null;
        }

        #endregion

        #region Product ProductCategory

        public virtual void DeleteCategory(ProductCategory productcategory)
        {
            if (productcategory == null)
                throw new ArgumentNullException("productcategory");

            productcategory.IsDeleted = true;
            UpdateCategory(productcategory);

            var subcategories = GetAllCategoriesByParentCategoryId(productcategory.Id, true);
            foreach (var subcategory in subcategories)
            {
                subcategory.ParentCategoryId = 0;
                UpdateCategory(subcategory);
            }
        }

        public virtual IPagedList<ProductCategory> GetAllCategories(string categoryName = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            var showHiddenParameter = _dataProvider.GetParameter();
            showHiddenParameter.ParameterName = "Active";
            showHiddenParameter.Value = showHidden;
            showHiddenParameter.DbType = DbType.Boolean;

            var nameParameter = _dataProvider.GetParameter();
            nameParameter.ParameterName = "Name";
            nameParameter.Value = categoryName ?? string.Empty;
            nameParameter.DbType = DbType.String;

            var pageIndexParameter = _dataProvider.GetParameter();
            pageIndexParameter.ParameterName = "PageIndex";
            pageIndexParameter.Value = pageIndex;
            pageIndexParameter.DbType = DbType.Int32;

            var pageSizeParameter = _dataProvider.GetParameter();
            pageSizeParameter.ParameterName = "PageSize";
            pageSizeParameter.Value = pageSize;
            pageSizeParameter.DbType = DbType.Int32;

            var totalRecordsParameter = _dataProvider.GetParameter();
            totalRecordsParameter.ParameterName = "TotalRecords";
            totalRecordsParameter.Direction = ParameterDirection.Output;
            totalRecordsParameter.DbType = DbType.Int32;

            //invoke stored procedure
            var categories = _dbContext.ExecuteStoredProcedureList<ProductCategory>("SP_AllCategories", showHiddenParameter, nameParameter, pageIndexParameter, pageSizeParameter, totalRecordsParameter);
            var totalRecords = (totalRecordsParameter.Value != DBNull.Value) ? Convert.ToInt32(totalRecordsParameter.Value) : 0;

            //paging
            return new PagedList<ProductCategory>(categories, pageIndex, pageSize, totalRecords);

        }

        public virtual IList<ProductCategory> GetAllCategoriesByParentCategoryId(int parentCategoryId, bool showHidden = false)
        {
            var query = _productCategoryRepository.Table;

            if (!showHidden)
                query = query.Where(c => c.IsActive);

            query = query.Where(c => c.ParentCategoryId == parentCategoryId);
            query = query.Where(c => !c.IsDeleted);
            query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

            var categories = query.ToList();

            var childCategories = new List<ProductCategory>();

            foreach (var productcategory in categories)
            {
                childCategories.AddRange(GetAllCategoriesByParentCategoryId(productcategory.Id, showHidden));
            }
            categories.AddRange(childCategories);

            return categories;
        }

        public virtual ProductCategory GetCategoryById(int categoryId)
        {
            if (categoryId == 0)
                return null;

            return _productCategoryRepository.GetByID(categoryId);
        }

        public virtual void InsertCategory(ProductCategory productcategory)
        {
            if (productcategory == null)
                throw new ArgumentNullException("productcategory");

            _productCategoryRepository.Insert(productcategory);
        }

        public virtual void UpdateCategory(ProductCategory productcategory)
        {
            if (productcategory == null)
                throw new ArgumentNullException("productcategory");

            var parentCategory = GetCategoryById(productcategory.ParentCategoryId);
            while (parentCategory != null)
            {
                if (productcategory.Id == parentCategory.Id)
                {
                    productcategory.ParentCategoryId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryId);
            }

            _productCategoryRepository.Update(productcategory);
        }

        public virtual void DeleteProductCategory(ProductCategoryMapping productCategoryMapping)
        {
            if (productCategoryMapping == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryMappingRepository.Delete(productCategoryMapping);
        }

        public virtual IPagedList<ProductCategoryMapping> GetProductCategoriesByCategoryId(int categoryId, int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (categoryId == 0)
                return new PagedList<ProductCategoryMapping>(new List<ProductCategoryMapping>(), pageIndex, pageSize);

            var query = from pc in _productCategoryMappingRepository.Table
                        join p in _productRepository.Table on pc.ProductId equals p.Id
                        where pc.ProductCategoryId == categoryId &&
                              !p.IsDeleted &&
                              (showHidden || p.IsActive)
                        orderby pc.DisplayOrder, pc.Id
                        select pc;

            query = from c in query
                    group c by c.Id
                    into cGroup
                    orderby cGroup.Key
                    select cGroup.FirstOrDefault();
            query = query.OrderBy(pc => pc.DisplayOrder).ThenBy(pc => pc.Id);

            var productCategories = new PagedList<ProductCategoryMapping>(query, pageIndex, pageSize);
            return productCategories;
        }

        public virtual IList<ProductCategoryMapping> GetProductCategoriesByProductId(int productId, bool showHidden = false)
        {
            return GetProductCategoriesByProductId(productId, showHidden);
        }

        public virtual ProductCategoryMapping GetProductCategoryById(int productCategoryMappingId)
        {
            if (productCategoryMappingId == 0)
                return null;

            return _productCategoryMappingRepository.GetByID(productCategoryMappingId);
        }

        public virtual void InsertProductCategoryMapping(ProductCategoryMapping productCategoryMapping)
        {
            if (productCategoryMapping == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryMappingRepository.Insert(productCategoryMapping);
        }

        public virtual void UpdateProductCategoryMapping(ProductCategoryMapping productCategoryMapping)
        {
            if (productCategoryMapping == null)
                throw new ArgumentNullException("productCategory");

            _productCategoryMappingRepository.Update(productCategoryMapping);
        }

        public virtual string[] GetNotExistingCategories(string[] categoryNames)
        {
            if (categoryNames == null)
                throw new ArgumentNullException("categoryNames");

            var query = _productCategoryRepository.Table;
            var queryFilter = categoryNames.Distinct().ToArray();
            var filter = query.Select(c => c.Name).Where(c => queryFilter.Contains(c)).ToList();

            return queryFilter.Except(filter).ToArray();
        }

        public virtual IDictionary<int, int[]> GetProductCategoryIds(int[] productIds)
        {
            var query = _productCategoryMappingRepository.Table;

            return query.Where(p => productIds.Contains(p.ProductId))
                .Select(p => new { p.ProductId, p.ProductCategoryId }).ToList()
                .GroupBy(a => a.ProductId)
                .ToDictionary(items => items.Key, items => items.Select(a => a.ProductCategoryId).ToArray());
        }
        #endregion

    }
}
