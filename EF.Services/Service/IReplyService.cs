using EF.Core.Data;
using System.Collections;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface IReplyService
    {
        void Insert(Reply replies);
        void Update(Reply replies);

        IList<Reply> GetAllRepliesByComment(int commentId);
    }
}
