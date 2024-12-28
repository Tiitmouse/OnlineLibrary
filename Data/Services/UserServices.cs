using Data.Models;

namespace Data.Services;

public interface IUserServices
{
    public Task Create(User newUser);
    public Task<User> Get(int id);
    public Task Delete(int id);
    public Task Update(int id, User user);
    public Task<List<User>> GetAll();
}

public class UserServices : IUserServices
{
    public Task Create(User newUser)
    {
        throw new NotImplementedException();
    }

    public Task<User> Get(int id)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }

    public Task Update(int id, User user)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetAll()
    {
        throw new NotImplementedException();
    }
}