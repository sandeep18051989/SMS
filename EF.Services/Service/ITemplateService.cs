﻿using EF.Core.Data;
using System.Collections.Generic;

namespace EF.Services.Service
{
    public interface ITemplateService
    {
        void Insert(Template template);

        void Update(Template template);

        void DeleteTemplate(int id);

        void Insert(DataToken token);

        void Update(DataToken token);

        void DeleteToken(int id);

        void DeleteTemplates(IList<Template> templates);

        IList<Template> GetTemplatesByIds(int[] templateIds);


        IList<Template> GetAllTemplates(bool? onlyActive = null, bool? showSystemDefined = null);

        DataToken GetDataTokenById(int tokenId);

        DataToken GetDataTokenByName(string dataTokenName);
        IList<DataToken> GetAllDataTokens(bool? active = null, bool? showSystemDefined = null);

        IList<DataToken> GetAllDataTokensByTemplate(int templateId);

        Template GetTemplateById(int templateId);

        void ToggleTemplate(int id);

        Template GetTemplateByCustomPage(int customPageId);

        Template GetTemplateByName(string templateName);

        void AddUserTokens(IList<DataToken> tokens, User user);

        void AddFeedbackTokens(IList<DataToken> tokens, Feedback feedback);


        void AddEventTokens(IList<DataToken> tokens, Event events);

        void AddBlogTokens(IList<DataToken> tokens, Blog blog);
        void AddAssessmentTokens(IList<DataToken> tokens, Assessment assessment);

        void AddStudentAssessmentTokens(IList<DataToken> tokens, AssessmentStudent stuassessment);

        void AddProductTokens(IList<DataToken> tokens, Product product);

        void AddCommentTokens(IList<DataToken> tokens, Comment comment);

        #region Utilities

        string Replace(string original, string pattern, string replacement);

        string ReplaceTokens(string template, IEnumerable<DataToken> tokens, bool htmlEncode = false, bool stringWithQuotes = false);

        string ReplaceConditionalStatements(string template, IEnumerable<DataToken> tokens);

        string Replace(string template, IEnumerable<DataToken> tokens, bool htmlEncode);

        #endregion
    }
}
