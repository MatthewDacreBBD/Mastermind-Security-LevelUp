using MastermindService.Data;
using MastermindService.Models;

namespace MastermindService.AppLogic
{
    //class to handle user logic
    public class UserLogic
    {
        public AppUser getUserByID(int id, MastermindDBContext db)
        {
            var user = db.AppUser.Find(id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return new AppUser { Id = -1 };
            }
        }

        // getting a list of all users
        public List<AppUser> getUserList(MastermindDBContext db)
        {
            List<AppUser> lstUsers = db.AppUser.ToList();
            return lstUsers;
        }

        // function to update the user
        public string registerUser(AppUser user, MastermindDBContext db)
        {
            if (user != null)
            {
                var existingUser = db.AppUser.Find(user.Id);
                if (existingUser != null)
                {
                    return "User already exists in database";
                }
                else
                {
                    db.AppUser.Add(user);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                    }

                    return "User registered Successfully";
                }
            }
            else
            {
                return "User details are invalid";
            }
        }

        // function to update the user
        public string updateUser(AppUser user, MastermindDBContext db)
        {
            if (user != null)
            {
                var existingUser = db.AppUser.Find(user.Id);
                if (existingUser != null)
                {
                    existingUser.username = user.username;
                    existingUser.password = user.password;
                    existingUser.wins = user.wins;
                    existingUser.losses = user.losses;
                    existingUser.passwordSalt = user.passwordSalt;
                    existingUser.authenticationToken = "";
                    try
                    {
                        db.SaveChanges();
                        return "Details updated Successfully";
                    }
                    catch (Exception ex)
                    {
                        ex.GetBaseException();
                        return "Something went wrong while updating";
                    }
                }
                else
                {
                    return "Could not update details";
                }
            }
            else
            {
                return "Details provided are null";
            }
        }
    }
}