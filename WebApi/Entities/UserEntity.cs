namespace WebApi.Entities;

public class UserEntity
{
    public int Id { get; set; }

    public string UserName { get; set; }

    public string UserEmail { get; set; }

    public string Password { get; set; }

    public string? AssignedUserId { get; set; }
}
