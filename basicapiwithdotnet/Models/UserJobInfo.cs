namespace basicapiwithdotnet.Models;

public partial class UserJobInfo
{
    public int UserId {get; init;}

    public string JobTitle { get; init;}

    public string Department {get; init;}


    public UserJobInfo()
    {
        JobTitle ??= "";
        Department ??= "";
    }
}