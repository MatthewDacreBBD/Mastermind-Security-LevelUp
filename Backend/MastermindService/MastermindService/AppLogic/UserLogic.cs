using MastermindService.Data;
using MastermindService.Models;

namespace MastermindService.AppLogic
{
    //class to handle user logic
    public class UserLogic
    {
        public User getUserByID(int id, MastermindDBContext db)
        {
            var user = db.User.Find(id);
            if (user != null)
            {
                return user;
            }
            else
            {
                return new User { Id = -1 };
            }
        }

        // getting a list of all users
        public List<User> getUserList(MastermindDBContext db)
        {
            List<User> lstUsers = new List<User>();
            var users = from u in db.User
                        select u;
            foreach (var user in users)
            {
                lstUsers.Add(user);
            }
            return lstUsers;
        }

        // function to update the user
        public string registerUser(User user, MastermindDBContext db)
        {
            if (user != null)
            {
                var existingUser = db.User.Find(user.Id);
                if (existingUser != null)
                {
                    return "User already exists in database";
                }
                else
                {
                    db.User.Add(user);
                    try
                    {
                        db.SaveChanges();
                    }catch(Exception ex)
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
        public string updateUser(User user, MastermindDBContext db)
        {
            if (user != null)
            {
                var existingUser = db.User.Find(user.Id);
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