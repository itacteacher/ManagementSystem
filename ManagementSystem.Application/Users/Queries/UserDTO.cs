﻿namespace ManagementSystem.Application.Users.Queries;

public class UserDTO
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }
}
