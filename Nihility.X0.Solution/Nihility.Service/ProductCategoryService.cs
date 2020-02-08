using Nihility.Data.Interface;
using Nihility.Data.Repositories;
using Nihility.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihility.Service
{
    public interface IProductCategoryService
    {
        ProductCategory Add(ProductCategory productCategory);
        void Update(ProductCategory productCategory);
        ProductCategory Delete(int id);
        IEnumerable<ProductCategory> GetAll();
        IEnumerable<ProductCategory> GetAllByParentId(int parentID);
        ProductCategory GetById(int id);
        void Save();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        IProductCategoryRepository _productCategoryRepository;
        IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork)
        {
            this._productCategoryRepository = productCategoryRepository;
            this._unitOfWork = unitOfWork;
        }

        public ProductCategory Add(ProductCategory productCategory)
        {
            return _productCategoryRepository.Add(productCategory);
        }

        public ProductCategory Delete(int id)
        {
            return _productCategoryRepository.Delete(id);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            return _productCategoryRepository.GetAll();
        }

        public IEnumerable<ProductCategory> GetAllByParentId(int parentID)
        {
            return _productCategoryRepository.GetMulti(x => x.ParentID == parentID);
        }

        public ProductCategory GetById(int id)
        {
            return _productCategoryRepository.GetSingleById(id);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductCategory productCategory)
        {
            _productCategoryRepository.Update(productCategory);
        }
    }
}
