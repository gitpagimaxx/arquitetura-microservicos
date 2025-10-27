using GeekShopping.IdentityServer.MainModule.Consent;

namespace GeekShopping.IdentityServer.MainModule.Device;

public class DeviceAuthorizationViewModel : ConsentViewModel
{
    public string? UserCode { get; set; }
    public bool ConfirmUserCode { get; set; }
}