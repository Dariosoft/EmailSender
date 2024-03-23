using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dariosoft.EmailSender.Infrastructure.Database
{
    internal class PaginationInfo
    {
        private PaginationInfo() { }

        public int PageSize { get; private set; }

        public int PageIndex { get; private set; }

        public int Skip { get; private set; }

        public static PaginationInfo? Get(Framework.Types.ListQueryModel? model)
        {
            if (model is null) return null;
            if (model.PageSize < 5) model.PageSize = 5;

            return new PaginationInfo 
            { 
                PageSize = model.PageSize,
                PageIndex = model.Page - 1,
                Skip = model.Page * (model.Page - 1)
            };
        }
    }
}
