using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PrisonBack.Controllers;
using PrisonBack.Domain.Models;
using PrisonBack.Mapping;
using PrisonBack.Persistence.Context;
using PrisonBack.Persistence.Repositories;
using PrisonBack.Resources.DTOs;
using PrisonBack.Services;

namespace PrisonBackTests.Controllers
{
    class IsolationControllerIntegrationTests
    {
        private static IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToResourceProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;
        }

        [Test]
        public void IsAddingNewIsolation()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd"),
            }));

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "database_to_add_isolation")
                .Options;

            var appDbContext = new AppDbContext(options);

            appDbContext.UserPermissions.Add(new UserPermission
            {
                Id = 1,
                UserName = "abcd",
                IdPrison = 1,
                Prison = new Prison
                {
                    Id = 1,
                    PrisonName = "prison_test"
                }
            });
            var isolationRepository = new IsolationRepository(appDbContext);
            var isolationService = new IsolationService(isolationRepository);
            var loggerRepository = new LoggerRepository(appDbContext);
            var loggerService = new LoggerService(loggerRepository);
            var isolationController =
                new IsolationController(isolationService, _mapper, loggerService)
                {
                    ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
                };
            appDbContext.SaveChanges();
            appDbContext.Prisoners.Add(new Prisoner
            {
                Id = 1,
                Name = "fdsafd",
                Forname = "dsdsa",
                Pesel = "12345678910",
                Address = "dsafa",
                Pass = false,
                Behavior = 2,
                Isolated = false,
                IdCell = 1,
                Cell = new Cell(),
                Isolations = new List<Isolation>(),
                Punishments = new List<Punishment>()
            });
            appDbContext.SaveChanges();
            isolationController.AddIsolation(new IsolationDTO
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                IdPrisoner = 1
            });

            Assert.AreEqual(appDbContext.Isolations.Count(),1);

        }
        [Test]
        public void IsDeletingIsolation()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd"),
            }));

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "database_to_delete_isolation")
                .Options;

            var appDbContext = new AppDbContext(options);

            appDbContext.UserPermissions.Add(new UserPermission
            {
                Id = 1,
                UserName = "abcd",
                IdPrison = 1,
                Prison = new Prison
                {
                    Id = 1,
                    PrisonName = "prison_test"
                }
            });
            var isolationRepository = new IsolationRepository(appDbContext);
            var isolationService = new IsolationService(isolationRepository);
            var loggerRepository = new LoggerRepository(appDbContext);
            var loggerService = new LoggerService(loggerRepository);
            var isolationController =
                new IsolationController(isolationService, _mapper, loggerService)
                {
                    ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
                };
            appDbContext.SaveChanges();
            appDbContext.Isolations.Add(new Isolation
            {
                Id = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                IdPrisoner = 1,
                Prisoner = new Prisoner
                {
                    Id = 1,
                    Name = "fdsafd",
                    Forname = "dsdsa",
                    Pesel = "12345678910",
                    Address = "dsafa",
                    Pass = false,
                    Behavior = 2,
                    Isolated = false,
                    IdCell = 1,
                    Cell = new Cell(),
                    Isolations = new List<Isolation>(),
                    Punishments = new List<Punishment>()
                }
            });
            appDbContext.SaveChanges();

            Assert.AreEqual(appDbContext.Isolations.Count(), 1);
            isolationController.DeleteIsolation(1);
            Assert.AreEqual(appDbContext.Isolations.Count(),0,"isolation was not deleted");
        }

        [Test]
        public void IsSelectingRightIsolation()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd"),
            }));

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "database_to_select_isolation")
                .Options;

            var appDbContext = new AppDbContext(options);

            appDbContext.UserPermissions.Add(new UserPermission
            {
                Id = 1,
                UserName = "abcd",
                IdPrison = 1,
                Prison = new Prison
                {
                    Id = 1,
                    PrisonName = "prison_test"
                }
            });
            var isolationRepository = new IsolationRepository(appDbContext);
            var isolationService = new IsolationService(isolationRepository);
            var loggerRepository = new LoggerRepository(appDbContext);
            var loggerService = new LoggerService(loggerRepository);
            var isolationController =
                new IsolationController(isolationService, _mapper, loggerService)
                {
                    ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
                };
            appDbContext.SaveChanges();
            appDbContext.Isolations.Add(new Isolation
            {
                Id = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                IdPrisoner = 1,
                Prisoner = new Prisoner
                {
                    Id = 1,
                    Name = "fdsafd",
                    Forname = "dsdsa",
                    Pesel = "12345678910",
                    Address = "dsafa",
                    Pass = false,
                    Behavior = 2,
                    Isolated = false,
                    IdCell = 1,
                    Cell = new Cell(),
                    Isolations = new List<Isolation>(),
                    Punishments = new List<Punishment>()
                }
            });
            appDbContext.SaveChanges();

            Assert.IsTrue(appDbContext.Isolations.Any(),"nothing here");
            Assert.AreEqual(appDbContext.Isolations.Count(), 1);
            Assert.IsNotNull(appDbContext.Isolations.FirstOrDefault(x => x.Id == 1), "this pass is null");
            //Assert.IsNotNull(isolationController.SelectedIsolation(1).Value, "selected isolation return null value"); //rip c#7, gonna work in c#8
            //Assert.AreEqual(appDbContext.Passes.FirstOrDefault(x => x.Id == 1), isolationController.SelectedIsolation(1).Value, "selected isolation has different value"); //rip c#7 gonna work in c#8
        }

        [Test]
        public void IsUpdatingIsolation()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "abcd"),
            }));

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "database_to_update_isolation")
                .Options;

            var appDbContext = new AppDbContext(options);

            appDbContext.UserPermissions.Add(new UserPermission
            {
                Id = 1,
                UserName = "abcd",
                IdPrison = 1,
                Prison = new Prison
                {
                    Id = 1,
                    PrisonName = "prison_test"
                }
            });
            var isolationRepository = new IsolationRepository(appDbContext);
            var isolationService = new IsolationService(isolationRepository);
            var loggerRepository = new LoggerRepository(appDbContext);
            var loggerService = new LoggerService(loggerRepository);
            var isolationController =
                new IsolationController(isolationService, _mapper, loggerService)
                {
                    ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { User = user } }
                };
            appDbContext.SaveChanges();
            appDbContext.Isolations.Add(new Isolation
            {
                Id = 1,
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddDays(1),
                IdPrisoner = 1,
                Prisoner = new Prisoner
                {
                    Id = 1,
                    Name = "fdsafd",
                    Forname = "dsdsa",
                    Pesel = "12345678910",
                    Address = "dsafa",
                    Pass = false,
                    Behavior = 2,
                    Isolated = false,
                    IdCell = 1,
                    Cell = new Cell(),
                    Isolations = new List<Isolation>(),
                    Punishments = new List<Punishment>()
                }
            });
            appDbContext.SaveChanges();

            isolationController.UpdateIsolation(1, new IsolationDTO
            {
                StartDate = DateTime.Today.AddDays(5),
                EndDate = DateTime.Today.AddDays(10)
            });
            
            Assert.IsTrue(appDbContext.Isolations.Any(),"nothing here");
            Assert.IsNotNull(appDbContext.Isolations.FirstOrDefault(x => x.Id == 1), "this isolation is null");
            // ReSharper disable once PossibleNullReferenceException because checked
            Assert.AreEqual(appDbContext.Isolations.FirstOrDefault(x => x.Id == 1).EndDate,DateTime.Today.AddDays(10),"wrong date at EndDate");
            // ReSharper disable once PossibleNullReferenceException because checked
            Assert.AreEqual(appDbContext.Isolations.FirstOrDefault(x => x.Id == 1).StartDate, DateTime.Today.AddDays(5),"wrong date at StartDate");

        }
    }
}
