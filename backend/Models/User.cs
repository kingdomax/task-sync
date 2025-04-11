namespace TaskSync.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Hashed version

        public User(int id, string username, string firstname, string lastname, string email, string password)
        {
            Id = id;
            Username = username;
            Firstname = firstname;
            Lastname = lastname;
            Email = email;
            Password = password;
        }
    }
}
