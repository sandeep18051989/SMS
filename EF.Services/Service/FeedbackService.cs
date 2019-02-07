using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using System.Data.Entity;

namespace EF.Services.Service
{
    public class FeedbackService : IFeedbackService
    {
        #region Fields

        public readonly IRepository<Feedback> _feedbackRepository;

        #endregion

        #region Const

        public FeedbackService(IRepository<Feedback> feedbackRepository)
        {
            this._feedbackRepository = feedbackRepository;
        }

        #endregion


        #region IFeedback Members

        public void Insert(Feedback feedback)
        {
            _feedbackRepository.Insert(feedback);
        }

        public void Update(Feedback feedback)
        {
            _feedbackRepository.Update(feedback);
        }

        #endregion

        #region Utilities

        public Feedback GetFeedbackById(int feedbackId)
        {
            if (feedbackId > 0)
            {
                var feedback = from c in _feedbackRepository.Table
                               orderby c.Id
                               where c.Id == feedbackId
                               select c;
                var query = feedback.FirstOrDefault();
                return query;
            }
            else
            {
                return null;
            }
        }

        public IList<Feedback> GetFeedbacks()
        {
            return _feedbackRepository.Table.ToList();
        }

        public virtual void DeleteQueries(IList<Feedback> feedbacks)
        {
            if (feedbacks == null)
                throw new ArgumentNullException("Feedbacks");

            foreach (var _feedback in feedbacks)
            {
                _feedbackRepository.Delete(_feedback);
            }
        }

        public virtual IList<Feedback> GetQueriesByIds(int[] feedbackIds)
        {
            if (feedbackIds == null || feedbackIds.Length == 0)
                return new List<Feedback>();

            var query = from r in _feedbackRepository.Table
                        where feedbackIds.Contains(r.Id)
                        select r;

            var fbacks = query.ToList();

            var sortedFeedbacks = new List<Feedback>();
            foreach (int id in feedbackIds)
            {
                var fback = fbacks.Find(x => x.Id == id);
                if (fback != null)
                    sortedFeedbacks.Add(fback);
            }
            return sortedFeedbacks;
        }

        public int GetFeedbackCountByCreatedDate(DateTime createddate)
        {
            if (createddate == null)
                throw new ArgumentNullException("createddate");

            return _feedbackRepository.Table.Count(x => DbFunctions.TruncateTime(x.CreatedOn) == createddate.Date);
        }

        #endregion
    }
}
