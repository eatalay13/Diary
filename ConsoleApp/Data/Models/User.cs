namespace ConsoleApp.Data.Models;

public partial class User
{
    public User()
    {
        Notes = new HashSet<Note>();
    }

    public int Id { get; set; }

    public string Firstname { get; set; }

    public string Lastname { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string? Email { get; set; }


    public virtual ICollection<Note> Notes { get; set; }
}
