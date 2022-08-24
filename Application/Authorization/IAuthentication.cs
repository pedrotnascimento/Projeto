using Application.DTO;

namespace Application.Authorization
{
    public interface IAuthentication
    {
        UserDTO GetSignedInUser(string token);
    }
}
