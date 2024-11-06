namespace DataLayer
{
    public interface INameDataService
    {
        IList<Name> GetNames();
        IList<Name> GetNames(int userid, int page, int pageSize);
        Name? GetName(int userid, string id);

        Name CreateName(int userid, string primaryname, string birthyear, string deathyear);
        bool UpdateName(int userid, Name name);

        bool DeleteName(int userid, string id);

        int NumberOfNames();


    }
}
