using Business.Abstract;
using Business.BussinessAspect.Autofac;
using Business.CCS;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Cashing;
using Core.Aspect.Autofac.Transaction;
using Core.Aspect.Autofac.Validation;
using Core.CrossCutingConcerns.Validation;
using Core.Utilities.Bussiness;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrate.InMemory;
using Entities.Concrate;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Business.Concrate
{
    public class ProductManager : IProductService
    {
        IProductDal _productDal;
        ICategoryService _categoryService;

        public ProductManager(IProductDal productDal, ICategoryService categoryService)
        {
            _productDal = productDal;
            _categoryService = categoryService;
        }

        [SecuredOperation("admin,product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Add(Product product)
        {
            IResult result = BussinessRules.Run(CheckIfProductNameExists(product.ProductName),
                 CheckIfProductCaountOfCategoryCorrect(product.CategoryId), CheckIfCategoryLimitExceded());

            if (result != null)
            {
                return result;
            }

            _productDal.Add(product);
            return new SuccessResult(Massages.ProductAdded);

        }

        [CacheAspect]
        public IDataResult<List<Product>> GetAll()
        {
            //if (DateTime.Now.Hour == 22)
            //{
            //    return new ErrorDataResult<List<Product>>(Massages.MaintenanceTime);
            //}

            return new SuccessDataResult<List<Product>>(_productDal.GetAll(), Massages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.CategoryId == id));
        }

        [CacheAspect]
        public IDataResult<Product> GetById(int productId)
        {
            return new SuccessDataResult<Product>(_productDal.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<Product>> GetByUnitPrice(decimal min, decimal max)
        {
            return new SuccessDataResult<List<Product>>(_productDal.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max));
        }

        public IDataResult<List<ProductDetailDto>> GetProductDetailDtos()
        {
            return new SuccessDataResult<List<ProductDetailDto>>(_productDal.GetProductDetails());
        }
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect("IProductService.Get")]
        public IResult Update(Product product)
        {
            if (CheckIfProductCaountOfCategoryCorrect(product.CategoryId).Success)
            {
                _productDal.Add(product);
                return new SuccessResult(Massages.ProductAdded);
            }

            return new ErrorResult();
        }
        private IResult CheckIfProductCaountOfCategoryCorrect(int categoryid)
        {
            var result = _productDal.GetAll(p => p.CategoryId == categoryid).Count;
            if (result >= 15)
            {
                return new ErrorResult(Massages.ProductCategoryError);
            }
            return new SuccessResult();
        }
        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDal.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Massages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }
        private IResult CheckIfCategoryLimitExceded()
        {
            var result = _categoryService.GetAll();
            if (result.Data.Count>15)
            {
                return new ErrorResult(Massages.CategoryLimitExceded);
            }
            return new SuccessResult();
        }
        
        [TransactionScopeAspect]
        public IResult AddTransactionalTest(Product product)
        {
            Add(product);
            if (product.UnitPrice<10)
            {
                throw new Exception("");
            }
            Add(product);
            return null;
        }
    }
}
