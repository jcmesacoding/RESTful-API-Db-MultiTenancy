namespace Api.Controllers.Models;
public class CambioClaveRequest
{
    public string PasswordActual { get; set; }
    public string PasswordNueva { get; set; }
}