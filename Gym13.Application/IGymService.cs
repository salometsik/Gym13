﻿using Gym13.Domain.Models;

namespace Gym13.Application
{
    public interface IGymService
    {
        Task<User?> GetUser(string userId);
    }
}
