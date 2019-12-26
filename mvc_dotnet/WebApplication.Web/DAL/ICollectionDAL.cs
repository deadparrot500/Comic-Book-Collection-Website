using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface ICollectionDAL
    {
        List<Collection> GetAllCollectionsFromUser(int userId);
        List<Collection> GetTopPublicCollections();
        List<Comic> GetAllComicsInACollection(int collectionId);
        int GetUserId(string userName);
        int CreateCollection(int userId, string collectionName, bool publicStatus);
        bool CheckForValue(string tableName, string columnName, string value);
        Collection GetCollectionData(int collectionId);
        Collection CreateTestCollection();
        List<Collection> GetAllCollectionsFromUserAnon(int userId);
        void DeleteCollection(int collectionId);
    }
}
