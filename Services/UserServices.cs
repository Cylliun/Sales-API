using SalesApi.Data;
using SalesApi.Dto.User;
using SalesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesApi.Services;

public class UserServices
{
    private readonly DataContext _context;

    public UserServices(DataContext context)
    {
        _context = context;
    }

    public async Task<User> CreateUserAsync(CreateUserDto userDto)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Passwordhash);

        var user = new User(userDto.Email, passwordHash, UserRole.User);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return user;
    }

    public async Task<User> UpdateUserAsync(UpdateUserDto userDto)
    {
        var user = await _context.Users.FindAsync(userDto.Id);
        if (user is null)
            throw new KeyNotFoundException("User not found");

        user.Update(userDto.Email, userDto.PasswordHash);

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            throw new KeyNotFoundException("User not found");

        user.Deactivate();

        await _context.SaveChangesAsync();
        return user;
    }
    public async Task<User> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is null)
            throw new KeyNotFoundException("User not found");
        return user;

    }
}

