


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
    public IActionResult AddUser(User user)
    {
          // Input validation
        if (user == null)
        {
            return BadRequest("User data is required");
        }

        if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.FirstName) || string.IsNullOrEmpty(user.LastName))
        {
            return BadRequest("Required fields (Email, FirstName, LastName) cannot be empty");
        }

        // Email validation
        if (!IsValidEmail(user.Email))
        {
            return BadRequest("Invalid email format");
        }
        string checkSql = @"
            SELECT Email 
            FROM ComputerStoreAppSchema.Users 
            WHERE Email = @Email";

        var existingUsers = _dapper.LoadDataWithParameters<string>(checkSql, new { user.Email });
        if (existingUsers.Any())
        {
            return Conflict("A user with this email already exists");
        }

        string sql = @"
            INSERT INTO ComputerStoreAppSchema.Users
                ([FirstName],[LastName],[Email],[Gender],[Active])
            VALUES
            (   
                @FirstName,
                @LastName,
                @Email,
                @Gender,
                @Active
            )";

        var parameters = new
        {
            user.FirstName,
            user.LastName,
            user.Email,
            user.Gender,
            user.Active
        };

        try 
        {
            int rowsAffected = _dapper.ExecuteSqlWithRowCount(sql, parameters);
            if (rowsAffected > 0)
            {
                return Ok("User added successfully");
            }
            return StatusCode(500, "Failed to add user");
        }
        catch(Exception ex)
        {
            // _logger.LogError(ex, "Error occurred while adding user: {Email}", user.Email);
            Console.WriteLine(ex.Message);
            return StatusCode(500, "An error occurred while processing your request");
        }
    
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

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}