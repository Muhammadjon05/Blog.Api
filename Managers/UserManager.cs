using Blog.Api.Context;
using Blog.Api.Entities;
using Blog.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Api.Managers;

public class UserManager
{
	private readonly IdentityDbContext _dbContext;
	private readonly TokenManager _tokenManager;

	public UserManager(
		TokenManager tokenManager,
		IdentityDbContext dbContext)
	{
		_tokenManager = tokenManager;
	
		_dbContext = dbContext;
	}

	public async Task<User> Register(CreateUserModel createUserModel)
	{
		if (await _dbContext.Users.AnyAsync(u => u.UserName == createUserModel.UserName))
		{
			throw new Exception("UserName already exists.");
		}

		var user = new User()
		{
			UserName = createUserModel.UserName,
			Name = createUserModel.Name
		};

		user.PasswordHash = new PasswordHasher<User>().HashPassword(user, createUserModel.Password);

		_dbContext.Users.Add(user);
		await _dbContext.SaveChangesAsync();

		return user;
	}

	public async Task<string> Login(LoginUserModel loginUserModel)
	{
		var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == loginUserModel.UserName);
		if (user == null)
		{
			throw new Exception("UserName or Password is incorrect");
		}

		var result = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, loginUserModel.Password);

		if (result != PasswordVerificationResult.Success)
		{
			throw new Exception("UserName or Password is incorrect");
		}

		var token = _tokenManager.GenerateToken(user);

		return token;
	}

	public async Task<User?> GetUser(Guid userId)
	{
		return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId);
	}

	public async Task<User?> GetUser(string userName)
	{
		return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
	}
}