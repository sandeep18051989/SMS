﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EF.Core;
using EF.Core.Data;
using EF.Core.Enums;
using EF.Services.Http;
using System.Web;
using System.Text.RegularExpressions;
using System.Linq.Dynamic;

namespace EF.Services.Service
{
    public class TemplateService : ITemplateService
    {
        private readonly IRepository<Template> _templateRepository;
        private readonly IRepository<DataToken> _tokenRepository;
        private readonly IRepository<CustomPage> _customPageRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Assessment> _assessmentRepository;
        private readonly IRepository<AssessmentStudent> _studentAssessmentRepository;
        private readonly IRepository<AcadmicYear> _acadmicYearRepository;
        private readonly IRepository<Vendor> _vendorRepository;

        public TemplateService(IRepository<Template> templateRepository, IRepository<DataToken> tokenRepository, IRepository<CustomPage> customPageRepository, IRepository<User> userRepository, IRepository<Assessment> assessmentRepository, IRepository<AssessmentStudent> studentAssessmentRepository, IRepository<AcadmicYear> acadmicYearRepository, IRepository<Vendor> vendorRepository)
        {
            this._templateRepository = templateRepository;
            this._tokenRepository = tokenRepository;
            this._customPageRepository = customPageRepository;
            this._userRepository = userRepository;
            this._assessmentRepository = assessmentRepository;
            this._studentAssessmentRepository = studentAssessmentRepository;
            this._acadmicYearRepository = acadmicYearRepository;
            this._vendorRepository = vendorRepository;
        }
        #region ITemplateService Members

        public void Insert(Template template)
        {
            _templateRepository.Insert(template);
        }

        public void Update(Template template)
        {
            _templateRepository.Update(template);
        }

        public void DeleteTemplate(int id)
        {
            _templateRepository.Delete(id);
        }

        public void Insert(DataToken token)
        {
            _tokenRepository.Insert(token);
        }

        public void Update(DataToken token)
        {
            _tokenRepository.Update(token);
        }

        public void DeleteToken(int id)
        {
            _tokenRepository.Delete(id);
        }

        #endregion

        public IList<Template> GetAllTemplates(bool? onlyActive = null, bool? showSystemDefined = null)
        {
            return _templateRepository.Table.Where(x => (!onlyActive.HasValue || onlyActive.Value == x.IsActive) && (!showSystemDefined.HasValue || showSystemDefined.Value == x.IsSystemDefined) && x.IsDeleted == false).ToList();
        }

        public DataToken GetDataTokenById(int tokenId)
        {
            if (tokenId > 0)
                return _tokenRepository.GetByID(tokenId);

            return null;
        }

        public DataToken GetDataTokenByName(string dataTokenName)
        {
            if (!string.IsNullOrEmpty(dataTokenName))
                return _tokenRepository.Table.FirstOrDefault(a => a.Name.ToLower() == dataTokenName.ToLower() && a.IsDeleted == false || a.Name.ToLower() == "[" + dataTokenName.ToLower() + "]");

            return null;
        }

        public virtual void DeleteTemplates(IList<Template> templates)
        {
            if (templates == null)
                throw new ArgumentNullException("templates");

            foreach (var template in templates)
            {
                template.IsDeleted = true;
                _templateRepository.Update(template);

            }
        }

        public IList<DataToken> GetAllDataTokens(bool? active = null, bool? showSystemDefined = null)
        {
            var query = _tokenRepository.Table.OrderBy(x => x.Name).ToList();

            if (active.HasValue)
                query = query.Where(x => x.IsActive == active.Value).ToList();

            if (showSystemDefined.HasValue)
                query = query.Where(x => x.IsSystemDefined == showSystemDefined.Value).ToList();

            return query.OrderBy(x => x.Name).ToList();
        }

        public virtual IList<Template> GetTemplatesByIds(int[] templateIds)
        {
            if (templateIds == null)
                return new List<Template>();

            if (templateIds.Length == 0)
                return new List<Template>();

            var query = from r in _templateRepository.Table
                        where templateIds.Contains(r.Id)
                        select r;

            var templates = query.ToList();

            var sortedTemplates = new List<Template>();
            foreach (int id in templateIds)
            {
                var template = templates.Find(x => x.Id == id);
                if (template != null)
                    sortedTemplates.Add(template);
            }
            return sortedTemplates;
        }

        public IList<DataToken> GetAllDataTokensByTemplate(int templateId)
        {
            if (templateId == 0)
                throw new Exception("Template id is missing");

            var query = _templateRepository.Table.FirstOrDefault(a => a.Id == templateId);
            return query.Tokens.OrderBy(dt => dt.SystemName).ToList();
        }

        public Template GetTemplateById(int templateId)
        {
            if (templateId == 0)
                throw new Exception("Template id is missing");

            return _templateRepository.GetByID(templateId);
        }

        public void ToggleTemplate(int id)
        {
            if (id == 0)
                throw new ArgumentNullException("template");

            var template = _templateRepository.GetByID(id);
            if (template != null)
            {
                template.IsActive = !template.IsActive;
                _templateRepository.Update(template);
            }

        }

        public Template GetTemplateByCustomPage(int customPageId)
        {
            if (customPageId == 0)
                throw new Exception("Custom Page id is missing");

            return _customPageRepository.GetByID(customPageId).Template;
        }

        public Template GetTemplateByName(string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
                throw new Exception("Template name is missing");

            return _templateRepository.Table.FirstOrDefault(a => a.Name.ToLower() == templateName.ToLower() && a.IsDeleted == false);
        }

        public virtual void AddUserTokens(IList<DataToken> tokens, User user)
        {
            //var _userinfodata = _userInfoRepository.Table.FirstOrDefault(x => x.UserId == user.Id);

            tokens.Add(new DataToken() { Name = "Username", SystemName = "UserName", Value = user.UserName });
            tokens.Add(new DataToken() { Name = "User Email Address", SystemName = "UserMail", Value = user.Email });
            tokens.Add(new DataToken() { Name = "User Id", SystemName = "UserId", Value = user.Id.ToString() });
            tokens.Add(new DataToken() { Name = "User Password", SystemName = "UserPassword", Value = user.Password.ToString() });
            tokens.Add(new DataToken() { Name = "User Approved", SystemName = "UserApproved", Value = user.IsApproved ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "User Active/Inactive", SystemName = "UserActive", Value = user.IsActive ? "Yes" : "No" });
            if (user.CreatedOn.HasValue)
                tokens.Add(new DataToken() { Name = "User Created On", SystemName = "UserCreationDate", Value = user.CreatedOn.Value.ToString("dd MMM yyyy HH:mm") });
        }

        public virtual void AddFeedbackTokens(IList<DataToken> tokens, Feedback feedback)
        {
            tokens.Add(new DataToken() { Name = "Visitor Name", SystemName = "VisitorName", Value = feedback.FullName });
            tokens.Add(new DataToken() { Name = "Feedback Email", SystemName = "FeedbackEmail", Value = feedback.Email });
            tokens.Add(new DataToken() { Name = "Feedback Id", SystemName = "FeedbackId", Value = feedback.Id.ToString() });
            tokens.Add(new DataToken() { Name = "Feedback Contact", SystemName = "FeedbackContact", Value = feedback.Contact });
            tokens.Add(new DataToken() { Name = "Feedback Location", SystemName = "FeedbackLocation", Value = feedback.Location });
            tokens.Add(new DataToken() { Name = "Feedback Description", SystemName = "FeedbackDescription", Value = feedback.Description });
        }

        public virtual void AddAssessmentTokens(IList<DataToken> tokens, Assessment assessment)
        {
            var assessmentData = _assessmentRepository.GetByID(assessment.Id);

            if (assessmentData != null)
            {
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Start Time",
                    SystemName = "AssessmentStartTime",
                    Value = assessmentData.StartTime?.ToString("dd MMM yyyy HH:mm") ?? "Time Not Specified"
                });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment End Time",
                    SystemName = "AssessmentEndTime",
                    Value = assessmentData.EndTime?.ToString("dd MMM yyyy HH:mm") ?? "Time Not Specified"
                });
                tokens.Add(new DataToken() { Name = "Assessment Name", SystemName = "AssessmentName", Value = assessmentData.Name });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Passing marks",
                    SystemName = "AssessmentPassingMarks",
                    Value = Math.Round(assessmentData.PassingMarks.Value).ToString(CultureInfo.InvariantCulture)
                });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Maximum Marks",
                    SystemName = "AssessmentMaxMarks",
                    Value = Math.Round(assessmentData.MaxMarks.Value).ToString(CultureInfo.InvariantCulture)
                });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Instructions",
                    SystemName = "AssessmentInstructions",
                    Value = assessmentData.Instructions
                });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Is Time Bound",
                    SystemName = "AssessmentIsTimeBound",
                    Value = assessmentData.IsTimeBound ? "Yes" : "No"
                });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Total Questions",
                    SystemName = "AssessmentTotalQuestions",
                    Value = assessmentData.TotalQuestions.ToString()
                });
                tokens.Add(new DataToken()
                {
                    Name = "Assessment Duration(In Mins.)",
                    SystemName = "AssessmentDuration",
                    Value = assessmentData.DurationInMinutes.HasValue ? assessmentData.DurationInMinutes.Value.ToString(CultureInfo.InvariantCulture) : "Unknown"
                });
                tokens.Add(new DataToken()
                {
                    Name = "Mandatory To Solve All Questions",
                    SystemName = "AssessmentMandatoryToSolveAll",
                    Value = assessmentData.MandatoryToSolveAll ? "Yes" : "No"
                });
            }
        }

        public virtual void AddStudentAssessmentTokens(IList<DataToken> tokens, AssessmentStudent stuassessment)
        {
            var stuAssessmentData = _studentAssessmentRepository.GetByID(stuassessment.Id);
            var assessmentData = new Assessment();
            var studentData = new Student();
            if (stuAssessmentData != null && stuAssessmentData.Assessment != null)
            {
                assessmentData = stuAssessmentData.Assessment;
                studentData = stuAssessmentData.Student;
            }

            tokens.Add(new DataToken() { Name = "Assessment Student First Name", SystemName = "AssessmentStudentFirstName", Value = studentData.FName });
            tokens.Add(new DataToken() { Name = "Assessment Student Last Name", SystemName = "AssessmentStudentLastName", Value = studentData.FName });
            tokens.Add(new DataToken() { Name = "Assessment Student User Name", SystemName = "AssessmentStudentUserName", Value = studentData.UserName });
            if (studentData.AcadmicYearId > 0)
                tokens.Add(new DataToken() { Name = "Assessment Student Acadmic Year", SystemName = "AssessmentStudentAcadmicYear", Value = _acadmicYearRepository.GetByID(studentData.AcadmicYearId)?.Name });
            tokens.Add(new DataToken() { Name = "Assessment Student Date Of Birth", SystemName = "AssessmentStudentDOB", Value = studentData.DateOfBirth?.ToString("dd MMM yyyy") });

            if (stuAssessmentData != null)
                tokens.Add(new DataToken()
                {
                    Name = "Student Assessment Start On",
                    SystemName = "StudentAssessmentStartOn",
                    Value = assessmentData.OpenToAnonymousUsers
                        ? assessmentData.StartTime?.ToString("dd MMM yyyy HH:mm") ?? "Start Time Not Specified"
                        : stuAssessmentData.StartOn?.ToString("dd MMM yyyy HH:mm") ?? "Start Time Not Specified"
                });

            if (stuAssessmentData != null)
                tokens.Add(new DataToken()
                {
                    Name = "Student Assessment End On",
                    SystemName = "StudentAssessmentEndOn",
                    Value = assessmentData.OpenToAnonymousUsers
                        ? assessmentData.EndTime?.ToString("dd MMM yyyy HH:mm") ?? "End Time Not Specified"
                        : stuAssessmentData.EndOn?.ToString("dd MMM yyyy HH:mm") ?? "End Time Not Specified"
                });

            if (stuAssessmentData != null)
                tokens.Add(new DataToken()
                {
                    Name = "Student Assessment Url",
                    SystemName = "StudentAssessmentUrl",
                    Value = stuAssessmentData.Url
                });

            if (stuAssessmentData != null)
                tokens.Add(new DataToken()
                {
                    Name = "Student Assessment Marks Obtained",
                    SystemName = "StudentAssessmentMarksObtained",
                    Value = stuAssessmentData.MarksObtained.ToString("f2")
                });

            if (stuAssessmentData != null)
                tokens.Add(new DataToken()
                {
                    Name = "Student Assessment Result",
                    SystemName = "StudentAssessmentResultStatus",
                    Value = Enum.GetName(typeof(ResultStatus), stuAssessmentData.ResultStatusId)
                });

            tokens.Add(new DataToken() { Name = "Assessment Start Time", SystemName = "AssessmentStartTime", Value = assessmentData.StartTime?.ToString("dd MMM yyyy HH:mm") ?? "Start Time Not Specified" });
            tokens.Add(new DataToken() { Name = "Assessment End Time", SystemName = "AssessmentEndTime", Value = assessmentData.EndTime?.ToString("dd MMM yyyy HH:mm") ?? "Start Time Not Specified" });
            tokens.Add(new DataToken() { Name = "Assessment Url", SystemName = "AssessmentUrl", Value = assessmentData.Url });
            tokens.Add(new DataToken() { Name = "Assessment Name", SystemName = "AssessmentName", Value = assessmentData.Name });
            tokens.Add(new DataToken() { Name = "Assessment Passing Marks", SystemName = "AssessmentPassingMarks", Value = Math.Round(assessmentData.PassingMarks.Value).ToString(CultureInfo.InvariantCulture) });
            tokens.Add(new DataToken() { Name = "Assessment Max Marks", SystemName = "AssessmentMaxMarks", Value = Math.Round(assessmentData.MaxMarks.Value).ToString(CultureInfo.InvariantCulture) });
            tokens.Add(new DataToken() { Name = "Assessment Instructions", SystemName = "AssessmentInstructions", Value = assessmentData.Instructions });
            tokens.Add(new DataToken() { Name = "Assessment Is Time bound", SystemName = "AssessmentIsTimeBound", Value = assessmentData.IsTimeBound ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "Assessment Total Questions", SystemName = "AssessmentTotalQuestions", Value = assessmentData.TotalQuestions.ToString() });
            tokens.Add(new DataToken() { Name = "Assessment Duration(In Mins.)", SystemName = "AssessmentDuration", Value = assessmentData.DurationInMinutes.HasValue ? assessmentData.DurationInMinutes.Value.ToString(CultureInfo.InvariantCulture) : "Unknown" });
            tokens.Add(new DataToken() { Name = "Assessment Mandatory To Solve All Questions", SystemName = "AssessmentMandatoryToSolveAll", Value = assessmentData.MandatoryToSolveAll ? "True" : "False" });
        }

        public virtual void AddEventTokens(IList<DataToken> tokens, Event events)
        {
            tokens.Add(new DataToken() { Name = "Event Start Date", SystemName = "EventStartDate", Value = events.StartDate.HasValue ? CodeHelper.TimeAgo(events.StartDate.Value) : "" });
            tokens.Add(new DataToken() { Name = "Event End Date", SystemName = "EventEndDate", Value = events.EndDate.HasValue ? CodeHelper.TimeAgo(events.EndDate.Value) : "" });
            tokens.Add(new DataToken() { Name = "Event Id", SystemName = "EventId", Value = events.Id.ToString() });
            tokens.Add(new DataToken() { Name = "Event Location", SystemName = "EventVenue", Value = events.Venue });
            tokens.Add(new DataToken() { Name = "Event Title", SystemName = "EventTitle", Value = events.Title });
            tokens.Add(new DataToken() { Name = "Event Active", SystemName = "EventActive", Value = events.IsActive ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "Event Approved", SystemName = "EventApproved", Value = events.IsApproved ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "Event Description", SystemName = "EventDescription", Value = events.Description });
            tokens.Add(new DataToken() { Name = "Event Url", SystemName = "EventUrl", Value = events.GetSystemName() });
        }

        public virtual void AddBlogTokens(IList<DataToken> tokens, Blog blog)
        {
            if (blog.CreatedOn.HasValue)
                tokens.Add(new DataToken() { Name = "Blog Created On", SystemName = "BlogCreatedOn", Value = CodeHelper.TimeAgo(blog.CreatedOn.Value) });

            tokens.Add(new DataToken() { Name = "Blog Email", SystemName = "BlogEmail", Value = blog.Email });
            tokens.Add(new DataToken() { Name = "Blog Id", SystemName = "BlogId", Value = blog.Id.ToString() });
            tokens.Add(new DataToken() { Name = "Blog User Id", SystemName = "BlogUserId", Value = blog.UserId.ToString() });
            tokens.Add(new DataToken() { Name = "Blog Name", SystemName = "BlogName", Value = blog.Name });
            tokens.Add(new DataToken() { Name = "Blog Active", SystemName = "BlogActive", Value = blog.IsActive ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "Blog Approved", SystemName = "BlogApproved", Value = blog.IsApproved ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "Blog Subject", SystemName = "BlogSubject", Value = blog.Subject });
            tokens.Add(new DataToken() { Name = "Blog Url", SystemName = "BlogUrl", Value = blog.GetSystemName() });
        }

        public virtual void AddProductTokens(IList<DataToken> tokens, Product product)
        {
            tokens.Add(new DataToken() { Name = "Product Name", SystemName = "ProductName", Value = product.Name });
            tokens.Add(new DataToken() { Name = "Product Vendor", SystemName = "ProductVendorName", Value = (product.VendorId.HasValue ? _vendorRepository.Table.FirstOrDefault(v => v.Id == product.VendorId.Value).Name : "") });
            tokens.Add(new DataToken() { Name = "Product Id", SystemName = "ProductId", Value = product.Id.ToString() });
            tokens.Add(new DataToken() { Name = "Product Description", SystemName = "ProductDescription", Value = product.Description });
            tokens.Add(new DataToken() { Name = "Product Url", SystemName = "ProductUrl", Value = product.GetSystemName() });
        }

        public virtual void AddCommentTokens(IList<DataToken> tokens, Comment comment)
        {
            tokens.Add(new DataToken() { Name = "Comment Message", SystemName = "CommentHtml", Value = comment.CommentHtml });
            tokens.Add(new DataToken() { Name = "Comment Id", SystemName = "CommentId", Value = comment.Id.ToString() });
            tokens.Add(new DataToken() { Name = "Comment User", SystemName = "CommentUser", Value = comment.Username });
            if (comment.CreatedOn.HasValue)
                tokens.Add(new DataToken() { Name = "Comment Added On", SystemName = "CommentAddedOn", Value = CodeHelper.TimeAgo(comment.CreatedOn.Value) });
            tokens.Add(new DataToken() { Name = "Comment Approved", SystemName = "CommentApproved", Value = comment.IsApproved ? "Yes" : "No" });
            tokens.Add(new DataToken() { Name = "Comment Block Reason", SystemName = "CommentBlockReason", Value = comment.BlockReason });
            tokens.Add(new DataToken() { Name = "Comment Blocked By User Id", SystemName = "CommentBlockedByUserId", Value = comment.BlockedBy.ToString() });
            tokens.Add(new DataToken() { Name = "Comment Blocked By User", SystemName = "CommentBlockedByUser", Value = comment.BlockedBy > 0 ? _userRepository.GetByID(comment.BlockedBy)?.UserName : "Unknown" });
        }

        #region Utilities

        public string Replace(string original, string pattern, string replacement)
        {
            var stringComparison = StringComparison.Ordinal;
            if (stringComparison == StringComparison.Ordinal)
                return original.Replace(pattern, replacement);

            var count = 0;
            var position0 = 0;
            var position1 = 0;

            var inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            var chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = original.IndexOf(pattern, position0, stringComparison)) != -1)
            {
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];
                for (int i = 0; i < replacement.Length; ++i)
                    chars[count++] = replacement[i];
                position0 = position1 + pattern.Length;
            }

            if (position0 == 0)
                return original;

            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];

            return new string(chars, 0, count);
        }

        public string ReplaceTokens(string template, IEnumerable<DataToken> tokens, bool htmlEncode = false, bool stringWithQuotes = false)
        {
            foreach (var token in tokens)
            {
                var tokenValue = token.Value ?? string.Empty;

                //wrap the value in quotes
                if (stringWithQuotes && tokenValue is string)
                    tokenValue = string.Format("\"{0}\"", tokenValue);

                template = Replace(template, string.Format(@"%{0}%", token.Name), tokenValue.ToString());
            }

            return template;
        }

        public string ReplaceConditionalStatements(string template, IEnumerable<DataToken> tokens)
        {
            //define regex rules
            var regexFullConditionalSatement = new Regex(@"(?:(?'Group' %if)|(?'Condition-Group' endif%)|(?! (%if|endif%)).)*(?(Group)(?!))",
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);
            var regexCondition = new Regex(@"\s*\((?:(?'Group' \()|(?'-Group' \))|[^()])*(?(Group)(?!))\)\s*",
                RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            //find conditional statements in the original template
            var conditionalStatements = regexFullConditionalSatement.Matches(template).Cast<Match>()
                .SelectMany(match => match.Groups["Condition"].Captures.Cast<Capture>().Select(capture => new
                {
                    Index = capture.Index,
                    FullStatement = capture.Value,
                    Condition = regexCondition.Match(capture.Value).Value
                })).ToList();

            if (!conditionalStatements.Any())
                return template;

            //replace conditional statements
            foreach (var statement in conditionalStatements.OrderBy(statement => statement.Index))
            {
                var conditionIsMet = false;
                if (!string.IsNullOrEmpty(statement.Condition))
                {
                    try
                    {
                        //replace tokens (string values are wrap in quotes)
                        var conditionString = ReplaceTokens(statement.Condition, tokens, stringWithQuotes: true);
                        conditionIsMet = new[] { statement }.Where(conditionString).Any();
                    }
                    catch { }

                }
                template = template.Replace(conditionIsMet ? statement.Condition : statement.FullStatement, string.Empty);
            }
            template = template.Replace("%if", string.Empty).Replace("endif%", string.Empty);

            //return template with resolved conditional statements
            return template;
        }

        public string Replace(string template, IEnumerable<DataToken> tokens, bool htmlEncode)
        {
            if (string.IsNullOrWhiteSpace(template))
                throw new ArgumentNullException("template");

            if (tokens == null)
                throw new ArgumentNullException("tokens");

            //replace conditional statements
            template = ReplaceConditionalStatements(template, tokens);

            //replace tokens
            template = ReplaceTokens(template, tokens, htmlEncode);

            return template;
        }

        #endregion

    }
}
