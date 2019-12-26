using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Web.DAL
{
    public interface IStatisticsDAL
    {
        string NumberOfComics(int collectionId);
        List<string> TopHeroes(int collectionId);
        List<string> TopPublishers(int collectionId);
        List<string> TopWriters(int collectionId);
        string NumberofComicsALL();
        List<string> TopHeroesALL();
        List<string> TopPublishersALL();
        List<string> TopWritersALL();
    }
}
