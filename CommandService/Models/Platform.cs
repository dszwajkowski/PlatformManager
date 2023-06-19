﻿using System.ComponentModel.DataAnnotations;

namespace CommandService.Models;

public class Platform
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public ICollection<Command>? Commands { get; set; }
}
