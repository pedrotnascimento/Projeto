namespace Application.DTO
{
    public class JWTResponse
    {
        public bool Authenticated { get; internal set; }
        public string Created { get; internal set; }
        public string Expiration { get; internal set; }
        public string AccessToken { get; internal set; }
        public string Message { get; internal set; }
        public string UserId { get; internal set; }
    }
}
