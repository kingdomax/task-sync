namespace TaskSync.Models
{
    public class User
    {
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public User(string username, string firstname, string lastname, string email)
        {
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
        }
    }
}
