using OES_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OES_WebAPI.Repositories.Interfaces
{
    public interface ILevelRepository
    {
        IEnumerable<Level> GetAll();
        Level GetById(int levelId);
    }
}
