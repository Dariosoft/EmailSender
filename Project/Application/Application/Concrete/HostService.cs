using Dariosoft.EmailSender.Core.Models;
using Dariosoft.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dariosoft.EmailSender.Application.Concrete
{
    abstract class Service
    {

    }

    internal class HostService : IHostService
    {
        public Task<Reply> Create(Request<HostModel> request)
        {
            throw new NotImplementedException();
        }

        public Task<Reply> Delete(Request<KeyModel> request)
        {
            throw new NotImplementedException();
        }

        public Task<Reply<HostModel>> Get(Request<KeyModel> request)
        {
            throw new NotImplementedException();
        }

        public Task<ListReply<HostModel>> List(Request request)
        {
            throw new NotImplementedException();
        }

        public Task<Reply> SetAvailability(Request<SetAvailabilityModel> request)
        {
            throw new NotImplementedException();
        }

        public Task<Reply> Update(Request<HostModel> request)
        {
            throw new NotImplementedException();
        }
    }
}
