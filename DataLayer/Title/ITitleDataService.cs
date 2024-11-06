namespace DataLayer;

public interface ITitleDataService
{

    IList<Title> GetTitles(int userid);
    IList<Title> GetTitles(int userid, int page, int pageSize);
    Title? GetTitle(int userid, string id);

    Title CreateTitle(int userid, string primarytitle, string originaltitle, bool isadult, string startyear);
    bool UpdateTitle(int userid, Title title);
    bool DeleteTitle(int userid, string id);
    
    int NumberOfTitles();
    






}
