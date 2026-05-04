using API.Application.DTOs.UserInsertion;
using API.Application.Services.Authentication;
using API.Application.Services.Interfaces;
using API.Domain.Entities.UserManagement;
using API.Infrastructure.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace API.Application.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public UserService(ILogger<UserService> logger, 
            IUserRepository userRepository, 
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService,
            IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtService = jwtService;
            _mapper = mapper;
        }
        public async Task<AuthResultDto> CreateUser(UserDto user)
        {
            bool emailCheck = _userRepository.GetUserByEmailAsync(user.email) != null;
            if (emailCheck)
            {
                _logger.LogWarning("Attempt to create user with existing email {Email}", user.email);
                throw new Exception("Email or username already exists");
            }
            var newUser = _mapper.Map<ApplicationUser>(user);
            var result = await _userManager.CreateAsync(newUser, user.password);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new Exception("Failed to create user");
            }
            return _jwtService.GenerateToken(newUser);
        }

        public async Task<string?> LoginUser(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed for email {Email}: user not found", loginDto.Email);
                return string.Empty;
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, true);
            return result.Succeeded ? _jwtService.GenerateToken(user).Token : string.Empty;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found", email);
                throw new Exception("User not found");
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("User with username {Username} not found", username);
                throw new Exception("User not found");
            }
            return _mapper.Map<UserDto>(user);
        }

    }
}
