using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ITweeterProvider
    {
        //void Register();

        void PostStatus();

        object GetLatestStatus(); //Recieve JSON response
    }
}
