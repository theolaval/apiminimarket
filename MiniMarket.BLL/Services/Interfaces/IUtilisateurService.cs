using MiniMarket.Domain.Models;

namespace MiniMarket.BLL.Services.Interfaces
{
    public interface IUtilisateurService : IService<int, Utilisateur>
    {
        public Utilisateur GetByUsername(string username);
        public Utilisateur GetByEmail(string email);
        public Utilisateur Login(string username, string password);
        Utilisateur? GetById(int id);
        public bool UpdatePassword(int id, string newPassowrd);
    }
}
