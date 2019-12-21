﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BulbaCourses.GlobalSearch.Logic.InterfaceServices;
using BulbaCourses.GlobalSearch.Logic.Models;
using Swashbuckle.Swagger.Annotations;

namespace BulbaCourses.GlobalSearch.Web.Controllers
{
    [RoutePrefix("api/search")]
    public class SearchController : ApiController
    {

        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet, Route("")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Indexed courses not found")]
        [SwaggerResponse(HttpStatusCode.OK, "Indexed courses are found", typeof(IEnumerable<LearningCourse>))]
        public IHttpActionResult GetAllIndexed()
        {
            var result = _searchService.GetIndexedCourses();
            return result == null ? NotFound() : (IHttpActionResult)Ok(result);
        }

        [HttpGet, Route("{query}")]
        [SwaggerResponse(HttpStatusCode.NotFound, "courses not found")]
        [SwaggerResponse(HttpStatusCode.OK, "courses are found", typeof(IEnumerable<LearningCourse>))]
        public IHttpActionResult Search(string query)
        {
            var result = _searchService.Search(query);
            return result == null ? NotFound() : (IHttpActionResult)Ok(result);
        }
    }
}