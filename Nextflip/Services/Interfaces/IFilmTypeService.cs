using Nextflip.Models.filmType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Interfaces
{
    public interface IFilmTypeService
    {
        public IEnumerable<FilmType> GetFilmTypes();

        public FilmType GetFilmTypeByID(int typeID);
        public bool UpdateFilmType(int typeID, string filmType);

        public bool CreateNewFilmType(string filmTypeName);
        public bool RemoveFilmType(string filmTypeName);
    }
}
