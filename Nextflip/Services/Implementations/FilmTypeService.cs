using Nextflip.Models.filmType;
using Nextflip.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nextflip.Services.Implementations
{
    public class FilmTypeService : IFilmTypeService
    {
        private readonly IFilmTypeDAO _filmTypeDAO;
        public FilmTypeService(IFilmTypeDAO filmTypeDAO) => _filmTypeDAO = filmTypeDAO;

        public bool CreateNewFilmType(string filmTypeName) => _filmTypeDAO.CreateNewFilmType(filmTypeName);

        public FilmType GetFilmTypeByID(int typeID) => _filmTypeDAO.GetFilmTypeByID(typeID);

        public IEnumerable<FilmType> GetFilmTypes() => _filmTypeDAO.GetFilmTypes();

        public bool UpdateFilmType(int typeID, string filmType) => _filmTypeDAO.UpdateFilmType(typeID, filmType);
    }
}
