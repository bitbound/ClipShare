using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClipShare.Client.Interfaces
{
    public interface IModalChild
    {
        Task ModalOkayed();
        Task ModalCancelled();
    }
}
