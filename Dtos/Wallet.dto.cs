using System.ComponentModel.DataAnnotations;

namespace WalletApi.Dtos;

public record WalletRequest(
    [Required] decimal balance
);

