﻿using System;
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

        public IList<Event> GetAllEvents(bool? onlyActive = null)
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

        public void ToggleApproveStatusEvent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objEvent = _eventRepository.GetByID(id);
            if (objEvent != null)
            {
                objEvent.IsActive = !objEvent.IsApproved;
                objEvent.ModifiedOn = DateTime.Now;
                _eventRepository.Update(objEvent);
            }

        }

        public void ToggleActiveStatusEvent(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("id");

            var objEvent = _eventRepository.GetByID(id);
            if (objEvent != null)
            {
                objEvent.IsActive = !objEvent.IsActive;
                objEvent.ModifiedOn = DateTime.Now;
                _eventRepository.Update(objEvent);
            }

        }

        public virtual void DeleteEvents(IList<Event> events)
        {
            if (events == null)
                throw new ArgumentNullException("events");

            foreach (var _event in events)
            {
                _event.IsDeleted = true;
                _eventRepository.Update(_event);
            }
        }

        public virtual IList<Event> GetEventsByIds(int[] roleIds)
        {
            if (roleIds == null || roleIds.Length == 0)
                return new List<Event>();

            var query = from r in _eventRepository.Table
                        where roleIds.Contains(r.Id)
                        select r;

            var events = query.ToList();

            var sortedEvents = new List<Event>();
            foreach (int id in roleIds)
            {
                var qevent = events.Find(x => x.Id == id);
                if (qevent != null)
                    sortedEvents.Add(qevent);
            }
            return sortedEvents;
        }

        public IDictionary<string, int> GetDistinctLocationAndCount(bool? onlyActive = null)
        {
            var allVenues = _eventRepository.Table.Where(x => (!onlyActive.HasValue || x.IsActive == onlyActive.Value) && !x.IsDeleted).Distinct().ToList();
            var lstDistinct = allVenues.Select(x => x.Venue).Distinct().ToList();
            return lstDistinct.ToDictionary(x => x, x => GetCountByVenue(x));
        }

        public int GetCountByVenue(string subject)
        {
            return _eventRepository.Table.Count(x => !x.IsDeleted && x.Venue.Trim().ToLower().Contains(subject.Trim().ToLower()));
        }

        #endregion

        #region Paging

        public virtual IPagedList<Event> GetPagedEvents(string keyword = null, string venue = null, string schedule=null, int pageIndex = 0, int pageSize = int.MaxValue, bool? onlyActive = null)
        {
            var query = _eventRepository.Table;
            if (onlyActive.HasValue)
            {
                query = query.Where(n => n.IsActive == onlyActive.Value);
            }

            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Title.Contains(keyword) || x.Headline.Contains(keyword) || x.Description.Contains(keyword) || x.Venue.Contains(keyword));

            if (!string.IsNullOrEmpty(venue))
                query = query.Where(x => x.Venue.Contains(venue));

            if (!string.IsNullOrEmpty(schedule))
            {
                var currentDate = DateTime.Now.Date;
                var archiveDate = currentDate.AddDays(-30);
                switch (schedule)
                {
                    case "0":
                        {
                            query = query.Where(x => DbFunctions.TruncateTime(x.EndDate.Value) < archiveDate);
                            break;
                        }
                    case "1":
                        {
                            query = query.Where(x => DbFunctions.TruncateTime(x.StartDate.Value) <= currentDate && DbFunctions.TruncateTime(x.EndDate.Value) >= currentDate);
                            break;
                        }
                    case "2":
                        {
                            query = query.Where(x => DbFunctions.TruncateTime(x.StartDate.Value) >= currentDate);
                            break;
                        }
                    default:
                        break;
                }
            }

            query = query.Where(n => n.IsApproved && !n.IsDeleted).OrderByDescending(n => n.ModifiedOn);

            var events = new PagedList<Event>(query, pageIndex, pageSize);
            return events;
        }

        #endregion
    }
}
