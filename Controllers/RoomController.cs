using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HogwartsPotions.Controllers
{
    [ApiController, Route("/room")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Room>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRooms();
            if (rooms == null)
            {
                return NotFound();
            }
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<ActionResult> AddRoom([FromBody] Room room)
        {
            await _roomService.AddRoom(room);
            return CreatedAtAction("AddRoom", room);
        }

        [HttpGet("{id:long}")]
        public async Task<ActionResult<Room>> GetRoomById(long id)
        {
            var room = await _roomService.GetRoom(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult> UpdateRoomById(long id, [FromBody] Room updatedRoom)
        {
            if (id != updatedRoom.Id)
            {
                return BadRequest();
            }
            await _roomService.UpdateRoom(updatedRoom);
            return Ok(updatedRoom);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteRoomById(long id)
        {
            await _roomService.DeleteRoom(id);
            return Ok();
        }

    }
}
