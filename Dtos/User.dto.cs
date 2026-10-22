using System.ComponentModel.DataAnnotations;

namespace WalletApi.Dtos;

public record UserRequest(
    [Required, StringLength(30)]
    string Name,

    [Required, EmailAddress]
    string Email,

    [Required, StringLength(30, MinimumLength = 3)]
    string Username,

    [Required, StringLength(100, MinimumLength = 6)]
    string Password
);

