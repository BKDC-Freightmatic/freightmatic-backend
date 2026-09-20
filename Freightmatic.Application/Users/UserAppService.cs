using System;
using AutoMapper;
using Freightmatic.Application.Shared;
using Freightmatic.Domain.Users;
using Freightmatic.Infrastructure.Shared;
using Microsoft.Extensions.Configuration;

namespace Freightmatic.Application.Users;

public class UserAppService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;

    public UserAppService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<EmptyReponse> UpdateAsync(UpdateUserDto updateUserDto)
    {
        User user = await _userRepository.GetUserByIdAsync(updateUserDto.id);
        if (user == null)
            throw new CustomException("User does not exist");

        user.UserName = updateUserDto.Username;
        user.Name = updateUserDto.Name;
        user.Email = updateUserDto.Email;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.Address = updateUserDto.Address;
        user.OtherAddress = updateUserDto.OtherAddress;
        user.Avatar = updateUserDto.Avatar;
        user.IdentityCard = _mapper.Map<IdentityCard>(updateUserDto.IdentityCard);
        user.DriverLicense = _mapper.Map<DriverLicense>(updateUserDto.DriverLicense);
        user.Truck = _mapper.Map<Truck>(updateUserDto.Truck);
        user.DeliveryCategory = _mapper.Map<DeliveryCategory>(updateUserDto.DeliveryCategory);
        user.WorkingTime = _mapper.Map<WorkingTime>(updateUserDto.WorkingTime);
        user.DriveHistory = updateUserDto.DriveHistory;
        user.MostActiveRegion = updateUserDto.MostActiveRegion;
        user.DeliveryPrice = updateUserDto.DeliveryPrice;
        
        if (updateUserDto.Password != null)
        {
            if (await _userRepository.VerifyPassword(user, updateUserDto.Password) == false)
            {
                user.Password = await _userRepository.HashPassword(user, updateUserDto.Password);
                user.RefreshToken = null;
            }
        }

        bool isSuccess = await _userRepository.UpdateUserAsync(user);
        if (isSuccess == false)
            throw new CustomException("Error when update user information");

        return new EmptyReponse();
    }

    public async Task<AuthDto?> CreateAsync(CreateUserDto createUserDto)
    {
        User user;
        if (createUserDto.UserType == Domain.Shared.Enums.UserTypeEnum.User)
        {
            user = new User(
                "",
                createUserDto.UserName,
                createUserDto.Name,
                createUserDto.Email,
                createUserDto.PhoneNumber,
                createUserDto.UserType,
                createUserDto.Address,
                createUserDto.OtherAddress
            );
        }
        else
        {
            user = new User(
                "",
                createUserDto.UserName,
                createUserDto.Name,
                createUserDto.Email,
                createUserDto.PhoneNumber,
                createUserDto.UserType,
                createUserDto.Address,
                createUserDto.OtherAddress,
                _mapper.Map<IdentityCard>(createUserDto.IdentityCard),
                _mapper.Map<DriverLicense>(createUserDto.DriverLicense),
                _mapper.Map<Truck>(createUserDto.Truck),
                createUserDto.DriveHistory,
                _mapper.Map<DeliveryCategory>(createUserDto.DeliveryCategory),
                _mapper.Map<WorkingTime>(createUserDto.WorkingTime),
                createUserDto.MostActiveRegion
            );
        }

        await CheckExistAndReturnError(createUserDto.UserName, createUserDto.Email, createUserDto.PhoneNumber);

        JwtTokenGenerator jwtTokenGenerator = new(_configuration);

        string accessToken = jwtTokenGenerator.GenerateAccessToken(user.id, user.Email, 1);
        string refreshToken = jwtTokenGenerator.GenerateRefreshToken(user.id, user.Email, 7);

        User? createdUser = await _userRepository.CreateUserAsync(user, createUserDto.Password, refreshToken);
        if (createdUser == null)
            return null;

        UserDto userDto = _mapper.Map<UserDto>(createdUser);

        return new AuthDto() { Tokens = new Token { AccessToken = accessToken, RefreshToken = refreshToken }, User = userDto };
    }

    public async Task<AuthDto?> LoginAsync(string username, string password)
    {
        bool isExist = await _userRepository.IsExist(username, username, username);
        if (isExist == false)
            throw new CustomException("User does not exists");

        User? user = await _userRepository.VerifyUser(username, password);
        if (user == null)
            throw new CustomException("Username or password incorrect");

        UserDto userDto = _mapper.Map<UserDto>(user);

        JwtTokenGenerator jwtTokenGenerator = new(_configuration);
        string accessToken = jwtTokenGenerator.GenerateAccessToken(user.id, username, 1);
        string refreshToken = jwtTokenGenerator.GenerateRefreshToken(user.id, username, 7);

        user.RefreshToken = refreshToken;
        await _userRepository.UpdateUserAsync(user);

        return new AuthDto() { Tokens = new Token { AccessToken = accessToken, RefreshToken = refreshToken }, User = userDto };
    }

    public async Task<EmptyReponse> LogoutAsync(string id)
    {
        bool isSuccess = await _userRepository.LogoutUserAsync(id);
        if (isSuccess == false)
            throw new CustomException("Fail to log out user");

        return new EmptyReponse();
    }

    public async Task CheckExistAndReturnError(string userName, string email, string phoneNumber)
    {
        if (await _userRepository.GetUserByUserNameAsync(userName) != null)
            throw new CustomException($"User with username '{userName}' already exist");

        if (await _userRepository.GetUserByEmailAsync(email) != null)
            throw new CustomException($"User with email '{email}' already exist");

        if (await _userRepository.GetUserByPhoneNumberAsync(phoneNumber) != null)
            throw new CustomException($"User with phone number '{phoneNumber}' already exist");
    }

    public async Task<IEnumerable<UserDto?>> GetTopTruckers()
    {
        List<User?> users = await _userRepository.GetAllAsync();

        IEnumerable<User?> truckers = users.Where(x => x?.UserType == Domain.Shared.Enums.UserTypeEnum.Trucker);
        if (truckers.Any() == false)
            return [];

        IEnumerable<UserDto> userDtos = truckers.OrderByDescending(x => x.Rating).Select(x => _mapper.Map<UserDto>(x)).Take(5);
        return userDtos;
    }

    public async Task<IEnumerable<UserDto?>> GetSuggestTruckers()
    {
        List<User?> users = await _userRepository.GetAllAsync();

        IEnumerable<User?> truckers = users.Where(x => x?.UserType == Domain.Shared.Enums.UserTypeEnum.Trucker);
        if (truckers.Any() == false)
            return [];

        IEnumerable<UserDto> userDtos = users.Select(x => _mapper.Map<UserDto>(x));
        return userDtos;
    }

    public async Task<UserDto?> GetById(string userId)
    {
        User? user = await _userRepository.GetUserByIdAsync(userId);
        return user == null ? throw new CustomException("User does not exist") : _mapper.Map<UserDto>(user);
    }
}
