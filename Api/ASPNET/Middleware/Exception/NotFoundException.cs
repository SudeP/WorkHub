namespace Api.ASPNET.Middleware.Exception
{
    public class NotFoundException : System.Exception
    {
        public NotFoundException()
            : base($"Mevcutta kayıt bulunamadı.")
        { }
    }
}