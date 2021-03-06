﻿using PrisonBack.Domain.Models;
using PrisonBack.Domain.Repositories;
using PrisonBack.Persistence.Context;
using PrisonBack.Resources.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrisonBack.Persistence.Repositories
{
    public class LoggerRepository : BaseRepository, ILoggerRepository
    {
        public LoggerRepository(AppDbContext context) : base(context)
        {

        }
        public void AddLog(string controller, string action, int id)
        {
            Logger loggerDTO = new Logger();
            loggerDTO.LogData = DateTime.Now;
            loggerDTO.Controller = controller;
            loggerDTO.Action = action;
            loggerDTO.IdPrison = id;
            _context.Loggers.Add(loggerDTO);
            SaveChanges();
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
