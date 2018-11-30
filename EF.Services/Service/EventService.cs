using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;

namespace EF.Services.Service
{
	public class EventService : IEventService
	{
		public readonly IRepository<Event> _eventRepository;
		public EventService(IRepository<Event> eventRepository)
		{
			this._eventRepository = eventRepository;
		}
		#region IEventService Members

		public void Insert(Event events)
		{
			_eventRepository.Insert(events);
		}

		public void Update(Event events)
		{
			_eventRepository.Update(events);
		}

		public void Delete(int id)
		{
			_eventRepository.Delete(id);
		}

		#endregion

		#region Methods

		public IList<Event> GetAllEvents(bool? onlyActive=null)
		{
			return _eventRepository.Table.Where(e => !onlyActive.HasValue || e.IsActive == onlyActive.Value).OrderByDescending(a => a.CreatedOn).ToList();
		}

		public IList<Event> GetActiveEvents()
		{
			return _eventRepository.Table.Where(a => a.IsActive == true && a.IsDeleted == false).OrderByDescending(a => a.CreatedOn).ToList();
		}
		public Event GetEventById(int eventId)
		{
			if (eventId > 0)
				return _eventRepository.Table.FirstOrDefault(a => a.Id == eventId);

			return null;
		}
		public IList<Event> GetAllEventsByUser(int userId)
		{
			if (userId > 0)
				return _eventRepository.Table.Where(a => a.UserId == userId).ToList();

			return new List<Event>();
		}

		public Event GetEventByName(string title)
		{
			if (!string.IsNullOrEmpty(title))
				return _eventRepository.Table.FirstOrDefault(a => a.Title.ToLower() == title.ToLower() && a.IsDeleted == false);

			return null;
		}

		public int GetEventCountByCreatedDate(DateTime createddate)
		{
			if (createddate == null)
				throw new ArgumentNullException("created date empty.");

			return _eventRepository.Table.Count(e => (e.StartDate.Value >= createddate) && (e.EndDate.Value <= createddate));
		}

		#endregion
	}
}
