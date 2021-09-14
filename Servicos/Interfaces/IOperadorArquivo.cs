using System.Threading.Tasks;

namespace Servicos.Interfaces
{
    public interface IOperadorArquivo
    {
        Task AdicionarAsync(string arquivo);
    }
}