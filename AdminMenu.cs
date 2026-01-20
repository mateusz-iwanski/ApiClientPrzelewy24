using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using System.Threading.Tasks;

namespace OrchardCore.PaymentGateway;

public class AdminMenu : INavigationProvider
{
    private readonly IStringLocalizer S;

    public AdminMenu(IStringLocalizer<AdminMenu> localizer) => S = localizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase))
        {
            return ValueTask.CompletedTask;
        }

        builder.Add(S["Configuration"], cfg => cfg
            .Add(S["Settings"], settings => settings
                .Add(S["Przelewy24"], S["Przelewy24"], item => item
                    .Action("Index", "Admin", new { area = "OrchardCore.Settings", groupId = "Przelewy24" })
                    .Permission(Permissions.ManagePaymentGateway)
                    .LocalNav()
                )));

        return ValueTask.CompletedTask;
    }
}
