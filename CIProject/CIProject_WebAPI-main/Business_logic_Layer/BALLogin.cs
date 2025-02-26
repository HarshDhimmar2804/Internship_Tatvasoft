﻿using Data_Access_Layer;
using Data_Access_Layer.JWTService;
using Data_Access_Layer.Repository.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Business_logic_Layer
{
    public class BALLogin
    {
        private readonly DALLogin _dalLogin;
        private readonly JwtService _jwtService;
        ResponseResult result = new ResponseResult();
        public BALLogin(DALLogin dalLogin, JwtService jwtService)
        {
            _dalLogin = dalLogin;
            _jwtService = jwtService;
        }

        public string Register(User user)
        {
            return _dalLogin.Register(user);
        }
        public User GetUserById(int userId)
        {
            return _dalLogin.GetUserById(userId);
        }
        public string UpdateUser(User updatedUser)
        {
            return _dalLogin.UpdateUser(updatedUser);
        }

        public ResponseResult LoginUser(LoginRequest loginRequest)
        {
            try
            {
                User userObj = new User();
                userObj = UserLogin(loginRequest);

                if (userObj != null)
                {
                    if (userObj.Message.ToString() == "Login Successfully")
                    {
                        result.Message = userObj.Message;
                        result.Result = ResponseStatus.Success;
                        result.Data = _jwtService.GenerateToken(userObj.Id.ToString(), userObj.FirstName, userObj.LastName, userObj.PhoneNumber, userObj.EmailAddress, userObj.UserType, userObj.UserImage);
                    }
                    else
                    {
                        result.Message = userObj.Message;
                        result.Result = ResponseStatus.Error;
                    }
                }
                else
                {
                    result.Message = "Error in Login";
                    result.Result = ResponseStatus.Error;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
        public User UserLogin(LoginRequest loginRequest)
        {
            User userOb = new User()
            {
                EmailAddress = loginRequest.EmailAddress,
                Password = loginRequest.Password
            };

            return _dalLogin.LoginUser(userOb);
        }

        public User LoginUserDetailById(int userId)
        {
            return _dalLogin.LoginUserDetailById(userId);
        }

        public UserDetail GetUserProfileDetailById(int userId)
        {
            return _dalLogin.GetUserProfileDetailById(userId);
        }

        public string LoginUserProfileUpdate(UserDetail userDetail)
        {
            return _dalLogin.LoginUserProfileUpdate(userDetail);
        }
    }
}