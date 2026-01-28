
using System.ComponentModel.DataAnnotations;

namespace Modules.Examples.Bank.Account.Options;

public class BankAccountModuleOptions
{
    [Required]
    public bool? GetBalanceDisabled { get; set; }

    [MinLength(6)]
    public string ExampleStringOptionGT5Chars { get; set; } = default!;

}