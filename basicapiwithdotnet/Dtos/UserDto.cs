
namespace basicapiwithdotnet.Dtos;

public class UserDto
{
    public string FirstName {get; init;}
    public string LastName {get; init;}
    public string Email {get; init;}
    public string Gender {get; init;}
    public bool Active {get; init;}


    public UserDto()
    {
        FirstName ??= "";
        LastName ??= "";
        Email ??= "";
        Gender ??= "";
    }
}