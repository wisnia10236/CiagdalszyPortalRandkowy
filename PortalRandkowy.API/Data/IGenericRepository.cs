using System.Threading.Tasks;

namespace PortalRandkowy.API.Data
{
    public interface IGenericRepository
    {
        void Add<T>(T entity) where T : class; // metoda do dodawania czegokolwiek
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll(); // zapis danych
    }
}