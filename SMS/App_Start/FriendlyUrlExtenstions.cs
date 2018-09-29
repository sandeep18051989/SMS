using System;

namespace SMS
{

    public static class FriendlyUrlExtenstions
	{
        private static int _seoCodeLength = 2;
        
        private static bool IsVirtualDirectory(this string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path is not specified");

            return applicationPath != "/";
        }

        public static string RemoveApplicationPathFromRawUrl(this string rawUrl, string applicationPath)
        {
            if (string.IsNullOrEmpty(applicationPath))
                throw new ArgumentException("Application path is not specified");

            if (rawUrl.Length == applicationPath.Length)
                return "/";

            
            var result = rawUrl.Substring(applicationPath.Length);
            //raw url always starts with '/'
            if (!result.StartsWith("/"))
                result = "/" + result;
            return result;
        }

        public static bool IsLocalizedUrl(this string url, string applicationPath, bool isRawPath)
        {
            if (string.IsNullOrEmpty(url))
                return false;

            if (isRawPath)
            {
                if (applicationPath.IsVirtualDirectory())
                {
                    url = url.RemoveApplicationPathFromRawUrl(applicationPath);
                }

                int length = url.Length;
                //too short url
                if (length < 1 + _seoCodeLength)
                    return false;

                if (length == 1 + _seoCodeLength)
                    return true;

                return (length > 1 + _seoCodeLength) && (url[1 + _seoCodeLength] == '/');
            }
            else
            {
                int length = url.Length;
                if (length < 2 + _seoCodeLength)
                    return false;
                if (length == 2 + _seoCodeLength)
                    return true;

                return (length > 2 + _seoCodeLength) && (url[2 + _seoCodeLength] == '/');
            }
        }
    }
}