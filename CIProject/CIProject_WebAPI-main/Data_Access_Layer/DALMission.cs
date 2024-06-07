using Data_Access_Layer.Common;
using Data_Access_Layer.Repository;
using Data_Access_Layer.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Data_Access_Layer
{
    public class DALMission
    {
        private readonly AppDbContext _cIDbContext;

        public DALMission(AppDbContext cIDbContext)
        {
            _cIDbContext = cIDbContext;
        }
        public List<DropDown> GetMissionThemeList()
        {
            return _cIDbContext.MissionTheme
                .Select(mt => new DropDown { Value = mt.Id, Text = mt.ThemeName })
                .ToList();
        }

        public List<DropDown> GetMissionSkillList()
        {
            return _cIDbContext.MissionSkill
                .Select(ms => new DropDown { Value = ms.Id, Text = ms.SkillName })
                .ToList();
        }

        public List<Missions> MissionList()
        {
            return _cIDbContext.Missions.Where(m => m.IsDeleted == false)
                .ToList();
        }

        public string AddMission(Missions mission)
        {
            string result = "";

            mission.MissionOrganisationName = "";
            mission.CityName = "";
            mission.CountryName = "";
            mission.CreatedDate = DateTime.Now.ToUniversalTime();
            mission.MissionOrganisationDetail = "";
            mission.MissionApplyStatus = "";
            mission.MissionApproveStatus = "";
            mission.MissionAvilability = "";
            mission.MissionDateStatus = "";
            mission.MissionDeadLineStatus = "";
            mission.MissionDocuments = "";
            mission.MissionFavouriteStatus = "";
            mission.MissionSkillName = "";
            mission.MissionStatus = "";
            mission.MissionThemeName = "";
            mission.MissionType = "";
            mission.MissionVideoUrl = "";
            mission.ModifiedDate = DateTime.Now.ToUniversalTime();

            try
            {
                _cIDbContext.Missions.Add(mission);
                _cIDbContext.SaveChanges();
                result = "Mission added successfully.";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public Missions MissionDetailById(int id)
        {
            return _cIDbContext.Missions
                .FirstOrDefault(m => m.Id == id);
        }

        public async Task<string> UpdateMissionAsync(Missions mission)
        {
            try
            {

                var existingMission = await _cIDbContext.Missions.FirstOrDefaultAsync(mt => mt.Id == mission.Id && !mt.IsDeleted);
                if (existingMission != null)
                {

                    existingMission.MissionTitle = mission.MissionTitle;
                    existingMission.MissionDescription = mission.MissionDescription;
                    existingMission.MissionOrganisationName = mission.MissionOrganisationName;
                    existingMission.MissionOrganisationDetail = mission.MissionOrganisationDetail;
                    existingMission.CountryId = mission.CountryId;
                    existingMission.CityId = mission.CityId;
                    existingMission.StartDate = mission.StartDate;
                    existingMission.EndDate = mission.EndDate;
                    existingMission.MissionType = mission.MissionType;
                    existingMission.TotalSheets = mission.TotalSheets;
                    existingMission.RegistrationDeadLine = mission.RegistrationDeadLine;
                    existingMission.MissionThemeId = mission.MissionThemeId;
                    existingMission.MissionSkillId = mission.MissionSkillId;
                    existingMission.MissionImages = mission.MissionImages;
                    existingMission.MissionDocuments = mission.MissionDocuments;
                    existingMission.MissionAvilability = mission.MissionAvilability;
                    existingMission.MissionVideoUrl = mission.MissionVideoUrl;
                    existingMission.ModifiedDate = DateTime.Now.ToUniversalTime();

                    await _cIDbContext.SaveChangesAsync();

                    return "Update Mission Detail Successfully.";
                }
                else
                {
                    throw new Exception("Mission not found.");
                }
            }


            catch (Exception ex)
            {
                throw new Exception("Error in updating mission.", ex);
            }
        }

        public async Task<string> DeleteMissionAsync(int id)
        {
            try
            {
                var existingMission = await _cIDbContext.Missions.FirstOrDefaultAsync(mt => mt.Id == id && !mt.IsDeleted);
                if (existingMission != null)
                {
                    existingMission.IsDeleted = true;
                    await _cIDbContext.SaveChangesAsync();
                    return "Delete Mission Details Successfully..";
                }
                else
                {
                    throw new Exception("Mission not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in deleting mission details .", ex);
            }
        }

        public List<Missions> ClientSideMissionList(int userid)
        {
            List<Missions> clientSideMissionList = new List<Missions>();

            try
            {
                clientSideMissionList = _cIDbContext.Missions
                    .Where(m => !m.IsDeleted)
                    .Select(m => new Missions
                    {
                        Id = m.Id,
                        MissionTitle = m.MissionTitle,
                        MissionDescription = m.MissionDescription,
                        MissionOrganisationDetail = m.MissionOrganisationDetail,
                        MissionOrganisationName = m.MissionOrganisationName,
                        CountryId = m.CountryId,
                        CountryName = m.CountryName,
                        CityId = m.CityId,
                        CityName = m.CityName,
                        StartDate = m.StartDate,
                        EndDate = m.EndDate,
                        MissionType = m.MissionType,
                        TotalSheets = m.TotalSheets,
                        RegistrationDeadLine = m.RegistrationDeadLine,
                        MissionThemeId = m.MissionThemeId,
                        MissionSkillId = m.MissionSkillId,
                        MissionImages = m.MissionImages,
                        MissionDocuments = m.MissionDocuments,
                        MissionAvilability = m.MissionAvilability,
                        MissionVideoUrl = m.MissionVideoUrl,
                        MissionThemeName = m.MissionThemeName,
                        MissionSkillName = string.Join(",", m.MissionSkillName),
                        MissionStatus = m.RegistrationDeadLine < DateTime.Now.AddDays(-1) ? "Closed" : "Available",
                        MissionApplyStatus = _cIDbContext.MissionApplication.Any(ma => ma.MissionId == m.Id && ma.UserId == userid) ? "Applied" : "Apply",
                        MissionApproveStatus = _cIDbContext.MissionApplication.Any(ma => ma.MissionId == m.Id && ma.UserId == userid && ma.Status == true) ? "Approved" : "Applied",
                        MissionDateStatus = m.EndDate <= DateTime.Now.AddDays(-1) ? "MissionEnd" : "MissionRunning",
                        MissionDeadLineStatus = m.RegistrationDeadLine <= DateTime.Now.AddDays(-1) ? "Closed" : "Running",
                        MissionFavouriteStatus = "0",
                        Rating = 0,
                    })
                    .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return clientSideMissionList;
        }

        public string ApplyMission(MissionApplication missionApplication)
        {
            string result = "";
            try
            {
                // Begin transaction
                using (var transaction = _cIDbContext.Database.BeginTransaction())
                {
                    try
                    {
                        // Get the mission and check if it's available
                        var mission = _cIDbContext.Missions
                            .FirstOrDefault(m => m.Id == missionApplication.MissionId && m.IsDeleted == false);

                        if (mission != null)
                        {
                            // Check if sheets are available
                            if (mission.TotalSheets >= missionApplication.Sheet)
                            {
                                // Create a new MissionApplication entity
                                var newApplication = new MissionApplication
                                {
                                    MissionId = missionApplication.MissionId,
                                    UserId = missionApplication.UserId,
                                    AppliedDate = missionApplication.AppliedDate,
                                    Status = missionApplication.Status,
                                    Sheet = missionApplication.Sheet,

                                    CreatedDate = DateTime.Now.ToUniversalTime(),
                                    ModifiedDate = DateTime.Now.ToUniversalTime(),
                                    IsDeleted = false
                                };

                                // Add the new application to the context
                                _cIDbContext.MissionApplication.Add(newApplication);
                                _cIDbContext.SaveChanges();

                                // Update total sheets in the mission
                                mission.TotalSheets -= missionApplication.Sheet;
                                _cIDbContext.SaveChanges();

                                result = "Mission Apply Successfully.";
                            }
                            else
                            {
                                result = "Mission Housefull";
                            }
                        }
                        else
                        {
                            result = "Mission Not Found.";
                        }

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

        public List<MissionApplication> MissionApplicationList()
        {
            List<MissionApplication> missionApplicationList = new List<MissionApplication>();
            try
            {
                missionApplicationList = _cIDbContext.MissionApplication
                    .Where(ma => !ma.IsDeleted) // Assuming IsDeleted is a property on MissionApplication indicating deletion status
                    .Join(_cIDbContext.Missions.Where(m => !m.IsDeleted),
                          ma => ma.MissionId,
                          m => m.Id,
                          (ma, m) => new { ma, m })
                    .Join(_cIDbContext.User.Where(u => !u.IsDeleted),
                          mm => mm.ma.UserId,
                          u => u.Id,
                          (mm, u) => new MissionApplication
                          {
                              Id = mm.ma.Id,
                              MissionId = mm.ma.MissionId,
                              MissionTitle = mm.m.MissionTitle,
                              UserId = u.Id,
                              UserName = u.FirstName + " " + u.LastName,
                              AppliedDate = mm.ma.AppliedDate,
                              Status = mm.ma.Status
                          })
                    .ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return missionApplicationList;
        }

        public string MissionApplicationDelete(int id)
        {
            try
            {
                var missionApplication = _cIDbContext.MissionApplication.FirstOrDefault(m => m.Id == id);
                if (missionApplication != null)
                {
                    missionApplication.IsDeleted = true;
                    _cIDbContext.SaveChanges();
                    return "Success";
                }
                else
                {
                    return "Record not found";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string MissionApplicationApprove(int id)
        {
            try
            {
                var missionApplication = _cIDbContext.MissionApplication.FirstOrDefault(m => m.Id == id);
                if (missionApplication != null)
                {
                    missionApplication.Status = true;
                    _cIDbContext.SaveChanges();
                    return "Mission is approved";
                }
                else
                {
                    return "Mission is not approved";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}