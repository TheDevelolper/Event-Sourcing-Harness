
using System.ComponentModel.DataAnnotations;
using SaasFactory.Modules.Common;

namespace Modules.Examples.Bank.Account.Options;

public class BankAccountModuleOptions: ModuleOptionsBase
{
    [Required]
    public bool? GetBalanceDisabled { get; set; }
}