using System.Security.Claims;

namespace UniSchedule.Identity.Shared;

/// <summary>
///     Типы <see cref="Claim" /> для использования в JWT-токенах
/// </summary>
public static class ClaimTypes
{
    public const string UserId = "user_id";

    public const string Email = "email";

    public const string Surname = "surname";

    public const string Name = "name";

    public const string Patronymic = "patronymic";

    public const string Role = "role";

    public const string ManagedGroupIds = "managed_group_ids";

    public const string GroupId = "group_id";
}