using System.Collections.Generic;
using System.Threading.Tasks;
using HogwartsPotions.Models.Entities;

namespace HogwartsPotions.Services.Interfaces;

public interface IRoomService
{
    Task AddRoom(Room room);
    Task<Room> GetRoom(long id);
    Task DeleteRoom(long id);
    Task UpdateRoom(Room room);
    Task<List<Room>> GetAllRooms();
    Task<List<Room>> GetRoomsForRatOwners();
    Task<List<Room>> GetAvailableRooms();
}