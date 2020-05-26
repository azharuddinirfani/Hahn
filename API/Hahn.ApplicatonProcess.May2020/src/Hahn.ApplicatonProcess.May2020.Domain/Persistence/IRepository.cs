using Hahn.ApplicatonProcess.May2020.Domain.Models;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain.Persistence

{
    public interface IRepository<T> where T : new()
    {
        Task<T> Create(T item);
        Task<T> GetById(int id);
        Task<Result> Delete(int id);
        Task<(Result, int)> Update(int id, T item);

    }

}