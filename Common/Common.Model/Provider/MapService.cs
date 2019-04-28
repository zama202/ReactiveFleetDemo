using AzureMapsToolkit.Common;
using Common.Model.EventModel;
using System.Threading.Tasks;

namespace Common.Model.Provider
{
    public static class MapService
    {


        public async static Task<RouteResultLeg> getRoute(RouteEvent routeObj)
        {
            var am = new AzureMapsToolkit.AzureMapsServices("hl5yAHeRE4I3q_ZDT-tFiTG0lfDARF7pauzaowCJGdQ");

            RouteResultLeg res = null;
            RouteRequestDirections r = new RouteRequestDirections
            {
                Query = $"{routeObj.StatusLatitude},{routeObj.StatusLongitude}:{routeObj.GoalLatitude},{routeObj.GoalLongitude}",
                ApiVersion = "1.0",
                MaxAlternatives = 1
            };

            var resp = await am.GetRouteDirections(r);
            if (resp.Error != null)
            {
                //Handle error
            }
            else
                res = resp.Result.Routes[0].Legs[0];

            return res;
        }
    }
}
