using ConsoleApp.Data.Models;

internal class Program
{
    static readonly DiaryContext _context = new DiaryContext();
    static User _loginUser;

    private static void Main(string[] args)
    {
        LoginPage();

        Console.WriteLine(args);
    }

    public static void LoginPage()
    {
        Console.WriteLine("1- Kayıt Ol");
        Console.WriteLine("2- Giriş Yap");
        Console.WriteLine("3- Çıkış");

        string selection = Console.ReadLine();

        switch (selection)
        {
            case "1":
                Register();
                break;
            case "2":
                Login();
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Yanlış seçim yaptınız!");
                break;
        }
    }

    public static void Register()
    {
        Console.WriteLine("Ad:");
        string firstname = Console.ReadLine();
        Console.WriteLine("Soyad:");
        string lastname = Console.ReadLine();
        Console.WriteLine("Kullanıcı Adı:");
        string username = Console.ReadLine();
        Console.WriteLine("Şifre:");
        string password = Console.ReadLine();
        Console.WriteLine("E-Posta:");
        string email = Console.ReadLine();

        User user = new User();
        user.Firstname = firstname;
        user.Lastname = lastname;
        user.Username = username;
        user.Password = password;
        user.Email = email;

        _context.Users.Add(user);
        _context.SaveChanges();

        Console.WriteLine("Kayıt başarılı!");
    }

    public static void Login()
    {
        bool isLogin = false;

        while (!isLogin)
        {
            Console.WriteLine("Kullanıcı Adı:");
            string username = Console.ReadLine();
            Console.WriteLine("Şifre:");
            string password = Console.ReadLine();

            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                _loginUser = user;
                isLogin = true;

                Console.Clear();
                MainMenu();
            }
            else
            {
                Console.WriteLine("Kullanıcı adı veya şifre hatalı!");
            }
        }
    }

    public static void MainMenu()
    {
        Console.WriteLine("1- Yeni Kayıt Ekle");
        Console.WriteLine("2- Kayıtları Listele");
        Console.WriteLine("3- Çıkış");

        string selection = Console.ReadLine();

        switch (selection)
        {
            case "1":
                AddNote();
                break;
            case "2":
                ListNotes();
                break;
            case "3":
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Yanlış seçim yaptınız!");
                break;
        }
    }

    // List Notes with pagination.
    private static void ListNotes()
    {
        int page = 1;
        int pageSize = 1;

        bool isExit = false;

        while (!isExit)
        {
            var notes = _context.Notes
                .Where(n => n.UserId == _loginUser.Id)
                .OrderByDescending(n => n.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var note in notes)
            {
                Console.WriteLine(note.Title);
                Console.WriteLine(note.Detail);
                Console.WriteLine("-------------");
            }

            Console.WriteLine("Sayfa: " + page);

            Console.WriteLine("1- Önceki Sayfa");

            Console.WriteLine("2- Sonraki Sayfa");

            string selection = Console.ReadLine();

            switch (selection)
            {
                case "1":
                    if (page > 1)
                    {
                        page--;
                    }
                    break;
                case "2":
                    page++;
                    break;
                default:
                    isExit = true;
                    break;
            }

            Console.Clear();
        }

        MainMenu();
    }

    private static void AddNote()
    {
        var hasTodayNote = HasTodayNote();

        if(hasTodayNote)
        {
            Console.WriteLine("Bugün not girdiniz! Yeni bir not eklemek ister misin? (e/h)");

            string selection = Console.ReadLine();

            if(selection == "h")
            {
                Console.Clear();
                MainMenu();
            }
        }

        Console.Clear();

        Console.WriteLine("Başlık:");
        string title = Console.ReadLine();
        Console.WriteLine("Detay:");
        string detail = Console.ReadLine();

        Note note = new Note();
        note.Title = title;
        note.Detail = detail;
        note.Created = DateTime.Now;
        note.UserId = 1;

        _context.Notes.Add(note);
        _context.SaveChanges();

        Console.WriteLine("Kayıt eklendi!");

        Thread.Sleep(2000);

        Console.Clear();

        MainMenu();
    }

    private static bool HasTodayNote()
    {
        var todayNotes = _context.Notes.Where(n => n.Created.Date == DateTime.Now.Date && n.UserId == _loginUser.Id).ToList();

        if (todayNotes.Count > 0)
        {
            Console.WriteLine("Bugün not girdiniz!");
            Console.WriteLine("Başlık: " + todayNotes[0].Title);
            Console.WriteLine("Detay: " + todayNotes[0].Detail);
            Console.WriteLine("Tarih: " + todayNotes[0].Created);

            Console.WriteLine("Değiştirmek ister misiniz? (e/h)");

            string selection = Console.ReadLine();

            if (selection == "e")
            {
                Console.WriteLine("Başlık:");
                string title = Console.ReadLine();
                Console.WriteLine("Detay:");
                string detail = Console.ReadLine();

                todayNotes[0].Title = title;
                todayNotes[0].Detail = detail;

                _context.SaveChanges();

                Console.WriteLine("Kayıt güncellendi!");

                Thread.Sleep(2000);

                Console.Clear();

                MainMenu();
            }

            return true;
        }

        return false;
    }
}