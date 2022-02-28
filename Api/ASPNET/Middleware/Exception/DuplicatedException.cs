namespace Api.ASPNET.Middleware.Exception
{
    public class DuplicatedException : System.Exception
    {
        public DuplicatedException()
            : base($"Kayıt oluştururken mevcutta bulunan bir kayıt ile eşleştirildiniz.")
        {
        }
    }
}