namespace ConsoleApp.Data.Models;

public partial class Note
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Title { get; set; }

    public string Detail { get; set; }

    public DateTime Created { get; set; }


    public virtual User User { get; set; }
}