using EF.Core.Data;
using EF.Core.Enums;
using System.Collections.Generic;
using System;

namespace EF.Services.Service
{
	public interface IFeedbackService
	{
		void Insert(Feedback feedback);
		void Update(Feedback feedback);
		Feedback GetFeedbackById(int feedbackId);
		IList<Feedback> GetFeedbacks();
		void DeleteQueries(IList<Feedback> feedbacks);
		IList<Feedback> GetQueriesByIds(int[] feedbackIds);
		int GetFeedbackCountByCreatedDate(DateTime createddate);

	}
}
