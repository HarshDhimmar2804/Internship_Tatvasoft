﻿using Data_Access_Layer.Migrations;
using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using System.Data;

namespace Data_Access_Layer
{
    public class DALLogin
    {
        private readonly AppDbContext _cIDbContext;
        public DALLogin(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }

        public User GetUserById(int userId)
        {
            try
            {
                User user = new User();
                // Retrieve the user by ID
                user = _cIDbContext.User.FirstOrDefault(u => u.Id == userId && !u.IsDeleted);

                if (user != null)
                {
                    return user;
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string Register(User user)
        {
            string result = "";
            try
            {
                // Check if the email address already exists
                bool emailExists = _cIDbContext.User.Any(u => u.EmailAddress == user.EmailAddress && !u.IsDeleted);

                if (!emailExists)
                {
                    string maxEmployeeIdStr = _cIDbContext.UserDetail.Max(ud => ud.EmployeeId);
                    int userID = _cIDbContext.User.Max(u => u.Id) + 1;
                    int userDetailID = _cIDbContext.UserDetail.Max(ud => ud.Id) + 1;
                    int maxEmployeeId = 0;

                    // Convert the maximum EmployeeId to an integer
                    if (!string.IsNullOrEmpty(maxEmployeeIdStr))
                    {
                        if (int.TryParse(maxEmployeeIdStr, out int parsedEmployeeId))
                        {
                            maxEmployeeId = parsedEmployeeId;
                        }
                        else
                        {
                            // Handle conversion error
                            throw new Exception("Error converting EmployeeId to integer.");
                        }
                    }

                    // Increment the maximum EmployeeId by 1 for the new user
                    int newEmployeeId = maxEmployeeId + 1;

                    // Create a new user entity
                    var newUser = new User
                    {
                        Id = userID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        Password = user.Password,
                        UserType = user.UserType,
                        CreatedDate = DateTime.Now.ToUniversalTime(),
                        IsDeleted = false
                    };
                    var newUserDetail = new UserDetail
                    {
                        Id = userDetailID,
                        UserId = userID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        EmailAddress = user.EmailAddress,
                        UserType = user.UserType,
                        Name = user.FirstName,
                        Surname = user.LastName,
                        EmployeeId = newEmployeeId.ToString(),
                        Title = "SDE 1",
                        Manager = "Manager",
                        Department = "IT",
                        MyProfile = "",
                        Avilability = "",
                        WhyIVolunteer = "",
                        LinkdInUrl = "",
                        MySkills = "",
                        UserImage = "",
                        Status = true
                    };
                    // Add the new user to the database
                    _cIDbContext.User.Add(newUser);
                    _cIDbContext.UserDetail.Add(newUserDetail);
                    _cIDbContext.SaveChanges();

                    result = "User register successfully.";
                }
                else
                {
                    throw new Exception("Email Address Already Exist.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public string UpdateUser(User updatedUser)
        {
            string result = "";
            try
            {
                // Check if the user with the provided email address exists and is not deleted
                var existingUser = _cIDbContext.User.FirstOrDefault(u => u.EmailAddress == updatedUser.EmailAddress && !u.IsDeleted);
                var existingUserDetail = _cIDbContext.UserDetail.FirstOrDefault(u => u.UserId == updatedUser.Id && !u.IsDeleted);

                if (existingUser != null && existingUserDetail != null)
                {
                    // Update user details
                    existingUser.FirstName = updatedUser.FirstName;
                    existingUser.LastName = updatedUser.LastName;
                    existingUser.PhoneNumber = updatedUser.PhoneNumber;
                    existingUser.UserType = updatedUser.UserType;
                    existingUser.ModifiedDate = DateTime.Now.ToUniversalTime();
                    existingUser.UserType = "user";

                    existingUserDetail.FirstName = updatedUser.FirstName;
                    existingUserDetail.LastName = updatedUser.LastName;
                    existingUserDetail.PhoneNumber = updatedUser.PhoneNumber;
                    existingUserDetail.EmailAddress = updatedUser.EmailAddress;
                    existingUserDetail.UserType = updatedUser.UserType;
                    existingUserDetail.Name = updatedUser.FirstName;
                    existingUserDetail.Surname = updatedUser.LastName;
                    existingUserDetail.ModifiedDate = DateTime.Now.ToUniversalTime();

                    // Save changes to the database
                    _cIDbContext.SaveChanges();

                    result = "User updated successfully.";
                }
                else
                {
                    throw new Exception("User not found or already deleted.");
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }

        public User LoginUser(User user)
        {
            User userObj = new User();
            try
            {
                var query = from u in _cIDbContext.User
                            where u.EmailAddress == user.EmailAddress && u.IsDeleted == false
                            select new
                            {
                                u.Id,
                                u.FirstName,
                                u.LastName,
                                u.PhoneNumber,
                                u.EmailAddress,
                                u.UserType,
                                u.Password,
                                UserImage = u.UserImage
                            };

                var userData = query.FirstOrDefault();

                if (userData != null)
                {
                    if (userData.Password == user.Password)
                    {
                        userObj.Id = userData.Id;
                        userObj.FirstName = userData.FirstName;
                        userObj.LastName = userData.LastName;
                        userObj.PhoneNumber = userData.PhoneNumber;
                        userObj.EmailAddress = userData.EmailAddress;
                        userObj.UserType = userData.UserType;
                        userObj.UserImage = userData.UserImage;
                        userObj.Message = "Login Successfully";
                    }
                    else
                    {
                        userObj.Message = "Incorrect Password.";
                    }
                }
                else
                {
                    userObj.Message = "Email Address Not Found.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userObj;
        }

        public User LoginUserDetailById(int userId)
        {
            try
            {
                User user = new User();
                // Retrieve the user by ID
                user = _cIDbContext.User.FirstOrDefault(u => u.Id == userId && !u.IsDeleted);

                if (user != null)
                {
                    return user;
                }
                else
                {
                    throw new Exception("User not found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UserDetail GetUserProfileDetailById(int userId)
        {
            try
            {
                var userDetails = (from u in _cIDbContext.User
                                   join ud in _cIDbContext.UserDetail on u.Id equals ud.UserId into UserDetailGroup
                                   from userdetail in UserDetailGroup.DefaultIfEmpty()
                                   where !u.IsDeleted && !userdetail.IsDeleted && u.UserType == "user" && userdetail.UserId == userId
                                   select new UserDetail
                                   {
                                       Id = u.Id,
                                       FirstName = u.FirstName,
                                       LastName = u.LastName,
                                       PhoneNumber = u.PhoneNumber,
                                       EmailAddress = u.EmailAddress,
                                       UserType = u.UserType,
                                       UserId = userdetail.Id,
                                       Name = userdetail.Name,
                                       Surname = userdetail.Surname,
                                       EmployeeId = userdetail.EmployeeId,
                                       Department = userdetail.Department,
                                       Title = userdetail.Title,
                                       Manager = userdetail.Manager,
                                       WhyIVolunteer = userdetail.WhyIVolunteer,
                                       CountryId = userdetail.CountryId,
                                       CityId = userdetail.CityId,
                                       Avilability = userdetail.Avilability,
                                       LinkdInUrl = userdetail.LinkdInUrl,
                                       MySkills = userdetail.MySkills,
                                       UserImage = userdetail.UserImage,
                                       Status = userdetail.Status,
                                   }).ToList().FirstOrDefault();

                return userDetails;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string LoginUserProfileUpdate(UserDetail userDetail)
        {
            string result = "";
            try
            {
                // Begin transaction
                using (var transaction = _cIDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Get the userdetails
                        var existingUserDetail = _cIDbContext.UserDetail
                            .FirstOrDefault(u => u.UserId == userDetail.UserId && u.IsDeleted == false);

                        if (existingUserDetail != null)
                        {
                            // Update user detail

                            result = "User Detail Updated Successfully!";
                        }
                        else
                        {
                            //Insert new user detail

                            result = "User Detail Created Successfully!";
                        }

                        //Update First Name and Last Name in User Table
                        var user = _cIDbContext.User
                            .FirstOrDefault(u => u.Id == userDetail.UserId && u.IsDeleted == false);
                        if (user != null)
                        {
                            //Update First and Last Name
                            user.FirstName = userDetail.FirstName;
                            user.LastName = userDetail.LastName;
                        }

                        _cIDbContext.SaveChanges();

                        // Commit transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction if an exception occurs
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return result;
        }
    }
}