using System;
using System.Collections.Generic;
using Npgsql;
using Dapper;
using GrowingStrongAPI.Helpers;
using GrowingStrongAPI.Entities;
using GrowingStrongAPI.Models;
using GrowingStrongAPI.Helpers.Schemas;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GrowingStrongAPI.DataAccess
{
    public class UserRepository : IUserRepository
    {
        private IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger _logger;

        public UserRepository(IDbConnectionFactory dbConnectionFactory,
                              ILogger<IUserRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public User GetById(int id)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    string sql = $@"SELECT * FROM get_user_account_by_id({id})";

                    connection.Open();

                    User user = connection.Query<User>(sql).AsList().FirstOrDefault();

                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public User GetByEmailAddress(string emailAddress)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    string sql = $@"SELECT * FROM get_user_by_email('{emailAddress}')";

                    connection.Open();

                    User user = connection.Query<User, UserProfile, UserTargets, User>(
                        sql,
                        map: (user, profile, targets) =>
                        {
                            user.UserProfile = profile;
                            user.UserTargets = targets;
                            return user;
                        },
                        splitOn: "birth_date,goal_weight").AsList().FirstOrDefault();
                    return user;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Register(RegistrationModel registrationModel)
        {
            try
            {
                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    int id;

                    string sql = $@"select * from register_user(@EmailAddress,
                                                                @PasswordHash,
                                                                @PasswordSalt,
                                                                @BirthDate,
                                                                @Sex,
                                                                @Height,
                                                                @Weight,
                                                                @Bmr,
                                                                @ActivityLevel,
                                                                @Tdee,
                                                                @WeightGoalTimeline)";


                    NpgsqlCommand command = new NpgsqlCommand(sql, connection);
                    NpgsqlParameter emailParam = new NpgsqlParameter("@EmailAddress", NpgsqlTypes.NpgsqlDbType.Varchar);
                    NpgsqlParameter passwordHashParam = new NpgsqlParameter("@PasswordHash", NpgsqlTypes.NpgsqlDbType.Bytea);
                    NpgsqlParameter passwordSaltParam = new NpgsqlParameter("@PasswordSalt", NpgsqlTypes.NpgsqlDbType.Bytea);
                    NpgsqlParameter birthDateParam = new NpgsqlParameter("@BirthDate", NpgsqlTypes.NpgsqlDbType.Date);
                    NpgsqlParameter sexParam = new NpgsqlParameter("@Sex", NpgsqlTypes.NpgsqlDbType.Varchar);
                    NpgsqlParameter heightParam = new NpgsqlParameter("@Height", NpgsqlTypes.NpgsqlDbType.Double);
                    NpgsqlParameter weightParam = new NpgsqlParameter("@Weight", NpgsqlTypes.NpgsqlDbType.Double);
                    NpgsqlParameter bmrParam = new NpgsqlParameter("@Bmr", NpgsqlTypes.NpgsqlDbType.Double);
                    NpgsqlParameter activityLevelParam = new NpgsqlParameter("@ActivityLevel", NpgsqlTypes.NpgsqlDbType.Varchar);
                    NpgsqlParameter tdeeParam = new NpgsqlParameter("@Tdee", NpgsqlTypes.NpgsqlDbType.Double);
                    NpgsqlParameter weightGoalTimelineParam = new NpgsqlParameter("@WeightGoalTimeline", NpgsqlTypes.NpgsqlDbType.Varchar);

                    emailParam.Value = registrationModel.EmailAddress;
                    passwordHashParam.Value = registrationModel.PasswordHash;
                    passwordSaltParam.Value = registrationModel.PasswordSalt;
                    birthDateParam.Value = registrationModel.BirthDate;
                    sexParam.Value = registrationModel.Sex;
                    heightParam.Value = registrationModel.Height;
                    weightParam.Value = registrationModel.Weight;
                    bmrParam.Value = registrationModel.Bmr;
                    activityLevelParam.Value = registrationModel.ActivityLevel;
                    tdeeParam.Value = registrationModel.Tdee;
                    weightGoalTimelineParam.Value = registrationModel.WeightGoalTimeline;

                    NpgsqlParameter[] parameters = new NpgsqlParameter[] { emailParam,
                                                                           passwordHashParam,
                                                                           passwordSaltParam,
                                                                           birthDateParam,
                                                                           sexParam,
                                                                           heightParam,
                                                                           weightParam,
                                                                           bmrParam,
                                                                           activityLevelParam,
                                                                           tdeeParam,
                                                                           weightGoalTimelineParam};

                    command.Parameters.AddRange(parameters);

                    connection.Open();

                    id = Int32.Parse(command.ExecuteScalar().ToString());

                    return id;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateUserDetails(UserDetailsUpdateModel updateModel)
        {
            try
            {
                int userId = updateModel.UserId;
                string birthDate = updateModel.BirthDate.ToString("yyyy-MM-dd");
                string sex = updateModel.Sex;
                double height = updateModel.Height;
                double weight = updateModel.Weight;
                double bmr = updateModel.Bmr;
                string activityLevel = updateModel.ActivityLevel;
                double tdee = updateModel.Tdee;
                double goalWeight = updateModel.GoalWeight;
                string weightGoalTimeline = updateModel.WeightGoalTimeline;

                string sql = $"call update_user({userId},'{birthDate}','{sex}',{height},{weight},{bmr},'{activityLevel}',{tdee},{goalWeight},'{weightGoalTimeline}')";

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    connection.Execute(sql);
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public void UpdateUserProfile(UserProfile profile)
        {
            try
            {
                int userId = profile.UserId;
                string birthDate = profile.BirthDate.ToString("yyyy-MM-dd");
                string sex = profile.Sex;
                double height = profile.Height;
                double weight = profile.Weight;
                double bmr = profile.Bmr;
                string activityLevel = profile.ActivityLevel;
                double tdee = profile.Tdee;

                string sql = $"call update_user_profile({userId},'{birthDate}','{sex}',{height},{weight},{bmr},'{activityLevel}',{tdee})";

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    connection.Execute(sql);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateUserTargets(UserTargets targets)
        {
            try
            {
                int userId = targets.UserId;
                double goalWeight = targets.GoalWeight;
                string weightGoalTimeline = targets.WeightGoalTimeline;

                string sql = $"call update_user({userId},{goalWeight},'{weightGoalTimeline}')";

                using (var connection = _dbConnectionFactory.CreateConnection(ConfigurationsHelper.ConnectionString))
                {
                    connection.Open();

                    connection.Execute(sql);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

}
