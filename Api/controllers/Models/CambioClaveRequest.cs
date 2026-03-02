namespace Api.Controllers.Models;
public class CambioClaveRequest
{
    public required string PasswordActual { get; set; }
    public required string PasswordNueva { get; set; }
}