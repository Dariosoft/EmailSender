using System;
using System.Linq;

namespace Dariosoft.Framework.Auth
{
    public interface IAccessRight
    {
        string? Id { get; }

        string Name { get; }

        string[] Roles { get; }
    }

    public record AccessRight: IAccessRight
    {
        public string? Id { get; init; }

        public required string Name { get; init; }

        public string[] Roles { get; init; } = [];
    }
}
