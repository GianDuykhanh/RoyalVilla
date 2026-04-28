using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoyalVillaAPI.Models.DTO;
using RoyalVillaAPI.Services;

namespace RoyalVillaAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<UserDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<ApiResponse<UserDTO>>> Register([FromBody] RegisterationRequestDTO registerationRequestDTO)
        {
            try
            {
                if (registerationRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registeration data is required"));
                }

                if (await _authService.IsEmailExistsAsync(registerationRequestDTO.Email))
                {
                    return Conflict(ApiResponse<object>.Conflict($"User with email '{registerationRequestDTO.Email}' already exists"));
                }

                var user = await _authService.RegisterAsync(registerationRequestDTO);

                if (user == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Registeration failed"));
                }

                // auth service
                var response = ApiResponse<UserDTO>.CreatedAt(user, "User created successfully");
                return CreatedAtAction(nameof(Register), response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred during registeration: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LoginRequestDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<LoginResponseDTO>>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                if (loginRequestDTO == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login data is required"));
                }

               
                var loginResponse = await _authService.LoginAsync(loginRequestDTO);

                if (loginResponse == null)
                {
                    return BadRequest(ApiResponse<object>.BadRequest("Login failed"));
                }

                // auth service
                var response = ApiResponse<LoginResponseDTO>.Ok(loginResponse, "Login successful");
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = ApiResponse<object>.Error(500, $"An error occurred during login: {ex.Message}");
                return StatusCode(500, errorResponse);
            }
        }
    }
}
