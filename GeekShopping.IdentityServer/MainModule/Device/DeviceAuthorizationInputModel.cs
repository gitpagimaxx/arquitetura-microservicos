using GeekShopping.IdentityServer.MainModule.Consent;

namespace GeekShopping.IdentityServer.MainModule.Device;

public class DeviceAuthorizationInputModel : ConsentInputModel
{
    public string? UserCode { get; set; }
}