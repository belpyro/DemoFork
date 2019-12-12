﻿using BulbaCourses.GlobalSearch.Logic.InterfaceServices;
using BulbaCourses.GlobalSearch.Logic.Models;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BulbaCourses.GlobalSearch.Web.Controllers
{
    [RoutePrefix("api/bookmarks")]
    public class BookmarkController : ApiController
    {
        private readonly IBookmarkService _bookmarkService;
        public BookmarkController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }
        [HttpGet, Route("")]
        [SwaggerResponse(HttpStatusCode.NotFound, "There are no bookmarks in list")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmarks were found", typeof(IEnumerable<Bookmark>))]
        public IHttpActionResult GetAll()
        {
            var result = _bookmarkService.GetAll();
            return result == null ? NotFound() : (IHttpActionResult)Ok(result);
        }

        [HttpGet, Route("{id}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid bookmark id")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Bookmark doesn't exists")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmark was found", typeof(Bookmark))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something goes wrong")]
        public IHttpActionResult GetById(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _))
            {
                return BadRequest();
            }
            try
            {
                var result = _bookmarkService.GetById(id);
                return result == null ? NotFound() : (IHttpActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet, Route("user/{id}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid UserId")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Bookmark wasn't found")]
        [SwaggerResponse(HttpStatusCode.OK, "ID users bookmarks were found", typeof(IEnumerable<Bookmark>))]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something goes wrong")]
        public IHttpActionResult GetByUserId(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            try
            {
                var result = _bookmarkService.GetByUserId(id);
                return result == null ? NotFound() : (IHttpActionResult)Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route("")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmark added")]
        public IHttpActionResult Create([FromBody]Bookmark bookmark)
        {
            //validate here
            return Ok(_bookmarkService.Add(bookmark));
        }

        [HttpDelete, Route("{id}")]
        [SwaggerResponse(HttpStatusCode.BadRequest, "Ivalid ID")]
        [SwaggerResponse(HttpStatusCode.NotFound, "Bookmark doesn't exists")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmark deleted")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, "Something goes wrong")]
        public IHttpActionResult RemoveById(string id)
        {
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var _) || _bookmarkService.GetById(id) == null)
            {
                return BadRequest();
            }
            try
            {
                _bookmarkService.RemoveById(id);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete, Route("")]
        [SwaggerResponse(HttpStatusCode.OK, "Bookmarks removed")]
        public IHttpActionResult ClearAll()
        {
            _bookmarkService.RemoveAll();
            return Ok();
        }
    }
}
