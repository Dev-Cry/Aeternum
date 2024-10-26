﻿namespace Aeternum.DTOs.Role
{
    public class ApplicationRoleDTO
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}