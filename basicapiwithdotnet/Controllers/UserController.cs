


using basicapiwithdotnet.DataAccess;
using basicapiwithdotnet.Models;
using Microsoft.AspNetCore.Mvc;


namespace basicapiwithdotnet.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private DataContextDapper _dapper;

    // [ActivatorUtilitiesConstructor]
    public UserController(IConfiguration configuration)
    {
        _dapper = new DataContextDapper(configuration);
    }

    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        Console.WriteLine("Get all users from db....");
        string sql = @"
        SELECT [UserId],
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active] 
        FROM ComputerStoreAppSchema.Users
        ";

        IEnumerable<User> user = _dapper.LoadData<User>(sql);

        return user;
    }

    [HttpGet("GetUserById/{userId}")]
    public User GetUserById(int userId)
    {
        string sql = @"
            SELECT [UserId],
                [FirstName],
                [LastName],
                [Email],
                [Gender],
                [Active] 
            FROM ComputerStoreAppSchema.Users
                WHERE UserId = " + userId.ToString();

        User user = _dapper.LoadDataSingle<User>(sql);

        return user;
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser()
    {
        return Ok();
    }

    [HttpPut("UpdateUser")]

    // public IActionResult UpdateUser([FromBody]) // allows user to add payload 
    public IActionResult UpdateUser(User user)
    {
        string sql = @"
            UPDATE ComputerStoreAppSchema.Users
            SET [FirstName] = @FirstName,
                [LastName] = @LastName,
                [Email] = @Email,
                [Gender] = @Gender,
                [Active] = @Active
            WHERE UserId = @UserId
        ";
        var parameters = new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Gender,
            user.Active,
            user.UserId
        };

        int rowsAffected = _dapper.ExecuteSqlWithRowCount(sql, parameters);

        if (rowsAffected > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
        
    }
}