using Nihility.Model.Models;
using Nihility.Service;
using Nihility.X0.Solution.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Nihility.X0.Solution.Areas.Administrator.Controllers.api
{
    [RoutePrefix("api/PostCategory")]
    public class PostCategoryController : ApiControllerBase
    {
        IPostCategoryService _postCategoryService;

        public PostCategoryController(IErrorService errorService, IPostCategoryService postCategoryService) : base(errorService)
        {
            this._postCategoryService = postCategoryService;
        }

        public HttpResponseMessage Create(HttpRequestMessage request, PostCategory postCategory)
        {
            return CreateHttpResponse(request, () =>
             {
                 HttpResponseMessage response = null;
                 if (ModelState.IsValid)
                 {
                     request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                 }
                 else
                 {
                     var category = _postCategoryService.Add(postCategory);
                     _postCategoryService.Save();

                     response = request.CreateResponse(HttpStatusCode.Created, category);
                 }

                 return response;
             });
        }


    }
}
