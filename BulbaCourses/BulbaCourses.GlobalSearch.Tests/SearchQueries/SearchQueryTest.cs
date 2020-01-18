﻿using AutoMapper;
using BulbaCourses.GlobalSearch.Data;
using BulbaCourses.GlobalSearch.Data.Models;
using BulbaCourses.GlobalSearch.Data.Services;
using BulbaCourses.GlobalSearch.Logic;
using BulbaCourses.GlobalSearch.Logic.DTO;
using BulbaCourses.GlobalSearch.Logic.InterfaceServices;
using BulbaCourses.GlobalSearch.Logic.Services;
using Moq;
using Ninject;
using Moq.EntityFramework.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace BulbaCourses.GlobalSearch.Tests.SearchQueries
{
    [TestFixture]
    public class SearchQueryTest
    {
        StandardKernel kernel;
        IMapper mapper;
        IQueryable<SearchQueryDB> queries;
        Mock<GlobalSearchContext> mockContext;
        Mock<DbSet<SearchQueryDB>> mockSet;

        [OneTimeSetUp]
        public void Setup()
        {
            kernel = new StandardKernel();
            kernel.Load<AutoMapperModule>();
            mapper = kernel.Get<IMapper>();
        }

        [SetUp]
        public void setupMocks()
        {
            queries = new List<SearchQueryDB>()
            {
                new SearchQueryDB
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.Now,
                    Query = "course that will make me smarter"
                },
                new SearchQueryDB
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.Now,
                    Query = "basic c#"
                },
                new SearchQueryDB
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.Now,
                    Query = "advanced php"
                },
                new SearchQueryDB
                {
                    Id = Guid.NewGuid().ToString(),
                    Created = DateTime.Now,
                    Query = "beginner course"
                },
            }.AsQueryable();

            mockSet = new Mock<DbSet<SearchQueryDB>>();
            mockSet.As<IQueryable<SearchQueryDB>>().Setup(m => m.Provider).Returns(queries.Provider);
            mockSet.As<IQueryable<SearchQueryDB>>().Setup(m => m.Expression).Returns(queries.Expression);
            mockSet.As<IQueryable<SearchQueryDB>>().Setup(m => m.ElementType).Returns(queries.ElementType);
            mockSet.As<IQueryable<SearchQueryDB>>().Setup(m => m.GetEnumerator()).Returns(queries.GetEnumerator());

            mockContext = new Mock<GlobalSearchContext>();
            mockContext.Setup(x => x.SearchQueries).Returns(mockSet.Object);
        }

        [Test, Category("SearchQuery")]
        public void add_search_query()
        {

            var DbService = new SearchQueryDbService(mockContext.Object);
            var mockLogicService = new Mock<SearchQueryService>();
            var service = new SearchQueryService(mapper, DbService);

            //Act
            var b = new SearchQueryDTO
            {
                Id = "1",
                Date = DateTime.Now,
                Query = "search query"
            };
            var q = service.Add(b);
            var all = DbService.GetAll().Count();

            //Assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            Assert.AreEqual(q.Query, b.Query);
        }

        [Test, Category("SearchQuery")]
        public void remove_all_search_queries()
        {
            var DbService = new SearchQueryDbService(mockContext.Object);
            var mockLogicService = new Mock<SearchQueryService>();
            var service = new SearchQueryService(mapper, DbService);

            //Act
            service.RemoveAll();

            //Assert
            mockContext.Verify(m => m.SaveChanges(), Times.Once());
            mockSet.Verify(m => m.RemoveRange(It.IsAny<IEnumerable<SearchQueryDB>>()), Times.Once());
        }
    }
}
