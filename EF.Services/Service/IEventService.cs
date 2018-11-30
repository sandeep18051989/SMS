using EF.Core.Data;
using System.Collections.Generic;
using System;

namespace EF.Services.Service
{
	public interface IEventService
	{
		void Insert(Event events);
		void Update(Event events);
		IList<Event> GetAllEvents(bool? onlyActive=null);
		IList<Event> GetActiveEvents();
		Event GetEventById(int eventId);
		IList<Event> GetAllEventsByUser(int userId);
		void Delete(int id);

		Event GetEventByName(string title);

		int GetEventCountByCreatedDate(DateTime createddate);

	}
}
