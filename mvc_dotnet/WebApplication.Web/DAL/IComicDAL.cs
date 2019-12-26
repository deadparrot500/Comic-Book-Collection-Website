using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Web.Models;

namespace WebApplication.Web.DAL
{
    public interface IComicDAL
    {
        int CreateComic(int userId, int collectionId, string author, string title, DateTime publishDate, string description, string publisher, HashSet<string> characters);
        int CreateAuthor(string authorName);
        int CreatePublisher(string publisherName);
        int CreateCharacter(string characterName, int publisherId);
        List<string> GetAllCharactersInAComic(int comicId);
        void AddCharacterToComic(int characterId, int comicId);
        void AddComicToCollection(int collectionId, int comicId);
        int CheckForValue(string tableName, string columnName, string value);
        Comic GetComicData(int comicId);
        int CheckNumberOfComicsInCollection(int collectionId);
        IList<Comic> SearchCharacter(string search);
        void DeleteCharacter(int characterId, int comicId);
        int CheckForCharacterInComic(int characterId, int comicId);
        void DeleteComicFromCollection(int comicId, int collectionId);
     

    }
}
