using RoyalVillaAPI.Models.DTO;

namespace RoyalVillaAPI.Services
{
    public interface IAuthService
    {
        Task<UserDTO?> RegisterAsync(RegisterationRequestDTO registerationRequestDTO);

        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO);

        Task<bool> IsEmailExistsAsync(string email);
    }
}
