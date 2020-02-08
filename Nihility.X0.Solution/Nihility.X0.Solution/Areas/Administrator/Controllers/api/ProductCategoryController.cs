using Nihility.Model.Models;
using Nihility.Service;
using Nihility.X0.Solution.Infrastructure.Core;
using Nihility.X0.Solution.Models.Administrator.ViewModels;
using RedExpress.Helper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Nihility.X0.Solution.Areas.Administrator.Controllers.api
{
    [RoutePrefix("api/ProductCategory")]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize
        IProductCategoryService _productCategoryService;
        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService) : base(errorService) => this._productCategoryService = productCategoryService;
        #endregion

        //[HttpGet]
        //[Route("GetAll")]
        //public IHttpActionResult GetAll(HttpRequestMessage request)
        //{
        //    var result = CreateHttpResponse(request, () =>
        //    {
        //        List<ProductCategoryViewModel> listProductCategory = new List<ProductCategoryViewModel>();
        //        var models = _productCategoryService.GetAll().ToList();
        //        foreach (ProductCategory item in models)
        //        {
        //            ProductCategoryViewModel ui = new ProductCategoryViewModel();
        //            item.CopyData(ui);
        //            listProductCategory.Add(ui);
        //        }
        //        var reponse = request.CreateResponse(HttpStatusCode.OK, listProductCategory);
        //        return reponse;
        //    });
        //    var result1 = result.Content.ReadAsStringAsync().Result;
        //    var result2 = JsonConvert.DeserializeObject(result1);
        //    return Ok(result2);
        //}

        /// <summary>
        /// - Lấy tất cả danh sách ProductCategory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public HttpResponseMessage GetAll(HttpRequestMessage request, int page, int pageSize)
        {
            var result = CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                List<ProductCategoryViewModel> listProductCategory = new List<ProductCategoryViewModel>();
                var models = _productCategoryService.GetAll().ToList();
                totalRow = models.Count();

                var query = models.Skip(page * pageSize).Take(pageSize);

                foreach (ProductCategory item in query)
                {
                    ProductCategoryViewModel ui = new ProductCategoryViewModel();
                    item.CopyData(ui);
                    listProductCategory.Add(ui);
                }

                var panigationSet = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = listProductCategory,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPage = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var reponse = request.CreateResponse(HttpStatusCode.OK, panigationSet);
                return reponse;
            });
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="productCategoryVM">Đối tượng truyền vào để binding với datamodel</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVM)
        {
            var result = CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {

                    ProductCategory bindingProductCategory = new ProductCategory();

                    productCategoryVM.CopyData(bindingProductCategory);
                    _productCategoryService.Add(bindingProductCategory);
                    _productCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.Created, productCategoryVM);
                }
                return response;
            });

            return result;
        }

        [HttpPut]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVM)
        {
            var result = CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    ProductCategory bindingProductCategory = _productCategoryService.GetById(productCategoryVM.ID);
                    productCategoryVM.CopyData(bindingProductCategory);
                    _productCategoryService.Update(bindingProductCategory);
                    _productCategoryService.Save();
                    response = request.CreateResponse(HttpStatusCode.Created, productCategoryVM);
                }
                return response;
            });

            return result;
        }

        [HttpDelete]
        [Route("delete")]
        public HttpResponseMessage Delete(HttpRequestMessage request, int ID)
        {
            var result = CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    ProductCategory bindingModel = _productCategoryService.Delete(ID);

                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, bindingModel);
                }

                return response;
            });

            return result;
        }

        [HttpDelete]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string listID)
        {
            var result = CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var ids = new JavaScriptSerializer().Deserialize<List<int>>(listID);

                    foreach (var id in ids)
                    {
                        _productCategoryService.Delete(id);
                    }

                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, true);
                }

                return response;
            });

            return result;
        }

    }
}
