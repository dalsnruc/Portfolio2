using DataLayer;
using System.Reflection;
using System.Collections;

var titledataservice = new TitleDataService();
var userdataservice = new UserDataService();
var namedataservice = new NameDataService();
var genredataservice = new GenreDataService();

//Console.WriteLine(namedataservice.GetName(1, "nm0000035").KnownForTitles.FirstOrDefault().Title.PrimaryTitle);
//Console.WriteLine(titledataservice.GetTitle(1, "tt26693752").TitleRating.AverageRating);
//Console.WriteLine(genredataservice.GetAllGenres().FirstOrDefault().Name);

static void PrintTitles(ITitleDataService titledataService)
{
    foreach (var e in titledataService.GetTitles(1))
    {
        Console.WriteLine($"{e.Id}, {e.PrimaryTitle}, {e.StartYear}");
    }

}

static void PrintUsers(IUserDataService userdataService)
{
    foreach (var e in userdataService.GetUsers())
    {
        Console.WriteLine($"{e.Id}, {e.Username}, {e.Email}");
    }

}

static void PrintAllProperties(object obj)
{
    if (obj == null)
    {
        Console.WriteLine("Object is null.");
        return;
    }

    Type type = obj.GetType();
    Console.WriteLine($"Properties of {type.Name}:");

    foreach (PropertyInfo property in type.GetProperties())
    {
        object value = property.GetValue(obj);

        if (value is IEnumerable enumerable && !(value is string))
        {
            Console.WriteLine($"{property.Name}: Collection");

            foreach (var item in enumerable)
            {
                Console.WriteLine($" - Item in {property.Name}:");
                PrintAllProperties(item); // Recursively print each item
            }
        }
        else
        {
            Console.WriteLine($"{property.Name}: {value}");
        }
    }
}
