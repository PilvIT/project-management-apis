using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Core.Features.Users.Models
{
    /// <summary>
    /// Avoid modifying this model. Keep everything separately in a Profile model.
    ///
    /// Warning: Never expose this model to API. It contains sensitive fields such as PasswordHash.
    /// </summary>
    public class AppUser : IdentityUser<Guid>
    {
        public Profile? Profile { get; set; }
    }
}
