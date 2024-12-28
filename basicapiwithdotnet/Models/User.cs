namespace basicapiwithdotnet.Models;

public partial class User
{
    public int UserId {get; init;}
    public string FirstName {get; init;}
    public string LastName {get; init;}
    public string Email {get; init;}
    public string Gender {get; init;}
    public bool Active {get; init;}


    public User()
    {
        FirstName ??= "";
        LastName ??= "";
        Email ??= "";
        Gender ??= "";
    }
}