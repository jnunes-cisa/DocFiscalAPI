using Repositories;
using Repositories.Entities;
using System.Linq;

namespace Services.InitialData
{

    public class FirstData
    {
        private UnityOfWork UnityOfWork;        

        public FirstData(UnityOfWork unityOfWork)
        {
             UnityOfWork = unityOfWork;            
        }
    }

}