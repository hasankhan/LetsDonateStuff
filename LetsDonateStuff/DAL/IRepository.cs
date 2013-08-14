using System;
namespace LetsDonateStuff.DAL
{
    public interface IRepository: IDisposable
    {
        void Save();
    }
}
