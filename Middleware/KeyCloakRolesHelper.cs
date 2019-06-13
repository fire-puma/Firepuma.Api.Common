using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using Sentry;

namespace Firepuma.Api.Common.Middleware
{
    public static class KeyCloakRolesHelper
    {
        public static string[] GetRolesFromClaims(IEnumerable<Claim> claims)
        {
            var claimsArray = claims as Claim[] ?? claims.ToArray();

            var realmAccessClaim = claimsArray.SingleOrDefault(x => x.Type == "realm_access");
            if (realmAccessClaim == null)
            {
                return null;
            }

            if (realmAccessClaim.ValueType != "JSON")
            {
                using (SentrySdk.PushScope())
                {
                    SentrySdk.ConfigureScope(s => { s.SetTags(claimsArray.Select(x => new KeyValuePair<string, string>(x.Type, x.Value))); });

                    SentrySdk.CaptureMessage($"Expected the 'realm_access' claim to be type JSON but it was '{realmAccessClaim.ValueType}'", Sentry.Protocol.SentryLevel.Warning);
                }

                return null;
            }

            if (realmAccessClaim.Value == null)
            {
                using (SentrySdk.PushScope())
                {
                    SentrySdk.ConfigureScope(s => { s.SetTags(claimsArray.Select(x => new KeyValuePair<string, string>(x.Type, x.Value))); });

                    SentrySdk.CaptureMessage($"Expected the 'realm_access' value to be non-null", Sentry.Protocol.SentryLevel.Warning);
                }

                return null;
            }

            return JsonConvert.DeserializeObject<RealmAccessJson>(realmAccessClaim.Value)?.Roles;
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class RealmAccessJson
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            public string[] Roles { get; set; }
        }
    }
}