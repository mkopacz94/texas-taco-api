using TexasTaco.Api.Gateway.Exceptions;

namespace TexasTaco.Api.Gateway.Routes
{
    public static class RouteTemplateMatcher
    {
        public static bool RouteEndMatchesTemplate(string? route, string? templateRoute)
        {
            if (string.IsNullOrWhiteSpace(route))
            {
                throw new RouteEmptyException("Route to match cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(templateRoute))
            {
                throw new RouteEmptyException("Template route cannot be empty.");
            }

            string[] routeParts = route
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            string[] templateParts = templateRoute
                .Split('/', StringSplitOptions.RemoveEmptyEntries);

            if(routeParts.Length < templateParts.Length)
            {
                return false;
            }

            Array.Reverse(routeParts);
            Array.Reverse(templateParts);

            for (int i = 0; i < templateParts.Length; i++)
            {
                if (!PartMatches(routeParts[i], templateParts[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool PartMatches(string routePart, string templatePart)
        {
            if (templatePart == "{version}")
            {
                if(!IsCorrectVersionPart(routePart))
                {
                    return false;
                }
            }
            else
            {
                if (templatePart != routePart)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsCorrectVersionPart(string routeVersionPart)
        {
            if (!routeVersionPart.StartsWith("v", StringComparison.CurrentCultureIgnoreCase))
            {
                return false;
            }

            string versionNumberString = routeVersionPart[1..].Replace(".", "");

            if (!int.TryParse(versionNumberString, out int _))
            {
                return false;
            }

            return true;
        }
    }
}
