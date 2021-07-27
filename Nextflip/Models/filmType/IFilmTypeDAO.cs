using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Models.filmType
{
    public interface IFilmTypeDAO
    {
        public IEnumerable<FilmType> GetFilmTypes();

        public FilmType GetFilmTypeByID(int typeID);
        public bool UpdateFilmType(int typeID, string type);

        public bool CreateNewFilmType(string filmTypeName);
        public bool RemoveFilmType(string filmTypeName);
    }
}
