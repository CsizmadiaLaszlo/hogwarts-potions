﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HogwartsPotions.Data;
using HogwartsPotions.Models.Entities;
using HogwartsPotions.Models.Enums;
using HogwartsPotions.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HogwartsPotions.Services;

public class RoomService : IRoomService
{
    private readonly HogwartsContext _context;

    public RoomService(HogwartsContext context)
    {
        _context = context;
    }

    public async Task AddRoom(Room room)
    {
        await _context.Rooms.AddAsync(room);
        await _context.SaveChangesAsync();
    }

    public async Task<Room> GetRoom(long roomId)
    {
        return await _context.Rooms
            .Include(room => room.Residents)
            .FirstOrDefaultAsync(room => room.Id == roomId);
    }

    public async Task DeleteRoom(long id)
    {
        var room = await GetRoom(id);
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateRoom(Room room)
    {
        _context.Rooms.Update(room);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Room>> GetAllRooms()
    {
        return await _context.Rooms
            .Include(room => room.Residents)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<Room>> GetRoomsForRatOwners()
    {
        return await _context.Rooms
            .Include(room => room.Residents)
            .Where(room =>
                !room.Residents.Any(resident => resident.PetType == PetType.Cat || resident.PetType == PetType.Owl))
            .AsNoTracking()
            .ToListAsync();
    }

    }
}