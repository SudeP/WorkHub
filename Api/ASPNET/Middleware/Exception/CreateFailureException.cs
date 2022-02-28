namespace Api.ASPNET.Middleware.Exception
{
    public class CreateFailureException : System.Exception
    {
        public CreateFailureException()
            : base($"Kayıt oluştururken hata oluştu.")
        { }
    }
}