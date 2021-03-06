﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrisonBack.Auth;
using PrisonBack.Domain.Models;
using PrisonBack.Domain.Services;
using PrisonBack.Resources;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrisonBack.Controllers
{
    [Route("/api/[controller]")]
    [Authorize]

    public class PCellsController : Controller
    {
        private string controller = "Cele";
        private readonly ICellService _cellService;
        private readonly IMapper _mapper;
        private readonly ILoggerService _loggerService;

       public PCellsController(ICellService cellService, IMapper mapper, ILoggerService loggerService)
        {
            _cellService = cellService;
            _mapper = mapper;
            _loggerService = loggerService;
        }

        [HttpGet("{id}")]
        public ActionResult<CellVM> SelectedCell(int id)
        {
            var cell = _cellService.SelectedCell(id);
            return Ok(_mapper.Map<CellVM>(cell));
        }
        [HttpGet]
        public async Task<IEnumerable<Cell>> AllCell(int id)
        {
            var cell = await _cellService.AllCell(id);
            return cell;
        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public ActionResult<CellVM> AddCell(CellDTO cellDTO)
        {
            var cellModel = _mapper.Map<Cell>(cellDTO);
            if (cellModel != null)
            {
                _cellService.CreateCell(cellModel);
                _cellService.SaveChanges();
                _loggerService.AddLog(controller, "Dodano nową cele", cellModel.IdPrison);

            }

            return Ok();
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public ActionResult DeleteCell(int id)
        {
            var cell = _cellService.SelectedCell(id);
            if(cell == null)
            {
                return NotFound();
            }
            _cellService.DeleteCell(cell);
            _cellService.SaveChanges();
            _loggerService.AddLog(controller, "Usunięto cele o ID " + cell.Id, cell.IdPrison);
            return Ok();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRoles.Admin)]
        public ActionResult UpdateCell(int id, CellDTO cellDTO)
        {
            var cell = _cellService.SelectedCell(id);
            if(cell == null)
            {
                return NotFound();
            }
            _mapper.Map(cellDTO, cell);
            _cellService.UpdateCell(cell);
            _cellService.SaveChanges();

            _loggerService.AddLog(controller, "Edytowano cele o ID " + cell.Id, cell.IdPrison);

            return NoContent();
        }

    }
}
