﻿using Microsoft.AspNetCore.Identity;

namespace Aeternum.Entities.User
{
    public class ApplicationRole : IdentityRole
    {
        public string Id { get; set; }
        public string Name { get; set; }
        // Popis role, vysvětlující účel
        public string? Description { get; set; }

        // Auditní informace
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
