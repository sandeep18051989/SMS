using System;
using System.Collections.Generic;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using System.Data.Entity;

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

        public IList<Event> GetLatestEvents(int? excepteventid = null, int userid = 0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            DateTime endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, totalDays).Date;
            return _eventRepository.Table.Where(x => (!excepteventid.HasValue || x.Id != excepteventid.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) >= startDate && DbFunctions.TruncateTime(x.CreatedOn) <= endDate))).ToList();
        }

        public IList<Event> GetOlderEvents(int? excepteventid = null, int userid = 0)
        {
            int totalDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).Date;
            return _eventRepository.Table.Where(x => (!excepteventid.HasValue || x.Id != excepteventid.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted && ((DbFunctions.TruncateTime(x.CreatedOn) < startDate))).ToList();
        }

        public IDictionary<string, int> GetDistinctVenueAndCount(bool? onlyActive = null, int userid = 0)
        {
            var allVenues = _eventRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && (userid == 0 || x.UserId == userid) && !x.IsDeleted).Distinct().ToList();
            var lstDistinct = allVenues.Select(x => x.Venue).Distinct().ToList();
            return lstDistinct.ToDictionary(x => x, x => GetCountByVenue(x));
        }

        public int GetCountByVenue(string venue, int userid = 0)
        {
            return _eventRepository.Table.Count(x => (userid == 0 || x.UserId == userid) && !x.IsDeleted && x.Venue.Trim().ToLower().Contains(venue.Trim().ToLower()));
        }

        #endregion
    }
}
