﻿using Microsoft.AspNetCore.Identity;

namespace ManagementSystem.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? ProfileImagePath { get; set; }
}
