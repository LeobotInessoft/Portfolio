using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GpsSite
{
    public partial class Sim800 : System.Web.UI.Page
    {
        List<double[]> testPath = new List<double[]>();
        protected void Page_Load(object sender, EventArgs e)
        {
            testPath = new List<double[]>();
            //testPath.Add(new double[] { -25.7029838562012, 28.2663612365723 });
            //testPath.Add(new double[] { -25.7029838562012, 28.2663612365723 });
            //testPath.Add(new double[] { -25.7029838562012, 28.2663612365723 });
            //testPath.Add(new double[] { -25.7029838562012, 28.2663612365723 });
            //testPath.Add(new double[] { -25.7029838562012, 28.2663612365723 });
            //testPath.Add(new double[] { -25.7029838562012, 28.2663612365723 });
            //testPath.Add(new double[] { -25.703010559082, 28.2663803100586 });
            //testPath.Add(new double[] { -25.7030715942383, 28.2663421630859 });
            //testPath.Add(new double[] { -25.703182220459, 28.26637840271 });
            //testPath.Add(new double[] { -25.7033615112305, 28.266357421875 });
            //testPath.Add(new double[] { -25.7036361694336, 28.2663288116455 });
            //testPath.Add(new double[] { -25.7036781311035, 28.2658843994141 });
            //testPath.Add(new double[] { -25.7035980224609, 28.2651863098145 });
            //testPath.Add(new double[] { -25.7035217285156, 28.2645092010498 });
            //testPath.Add(new double[] { -25.7034320831299, 28.2637596130371 });
            //testPath.Add(new double[] { -25.7033348083496, 28.2629013061523 });
            //testPath.Add(new double[] { -25.7032375335693, 28.2619781494141 });
            //testPath.Add(new double[] { -25.7025737762451, 28.2619323730469 });
            //testPath.Add(new double[] { -25.7018432617188, 28.2620258331299 });
            //testPath.Add(new double[] { -25.701021194458, 28.2621288299561 });
            //testPath.Add(new double[] { -25.7003269195557, 28.2622165679932 });
            //testPath.Add(new double[] { -25.6994228363037, 28.2623405456543 });
            //testPath.Add(new double[] { -25.6983814239502, 28.2624797821045 });
            //testPath.Add(new double[] { -25.6975860595703, 28.262544631958 });
            //testPath.Add(new double[] { -25.6974620819092, 28.2617568969727 });
            //testPath.Add(new double[] { -25.6973495483398, 28.2607669830322 });
            //testPath.Add(new double[] { -25.6972408294678, 28.2598209381104 });
            //testPath.Add(new double[] { -25.6971912384033, 28.2594566345215 });


            testPath.Add(new double[] { -25.6971263885498, 28.2589225769043 });
            testPath.Add(new double[] { -25.6970558166504, 28.2582092285156 });
            testPath.Add(new double[] { -25.6969547271729, 28.2573299407959 });
            testPath.Add(new double[] { -25.6968898773193, 28.256664276123 });
            testPath.Add(new double[] { -25.6974811553955, 28.2565116882324 });
            testPath.Add(new double[] { -25.6982192993164, 28.2564811706543 });
            testPath.Add(new double[] { -25.6983509063721, 28.2572975158691 });
            testPath.Add(new double[] { -25.6984634399414, 28.2582740783691 });
            testPath.Add(new double[] { -25.6985530853271, 28.2589988708496 });
            testPath.Add(new double[] { -25.6986198425293, 28.2596454620361 });
            testPath.Add(new double[] { -25.6987018585205, 28.2603340148926 });
            testPath.Add(new double[] { -25.6987781524658, 28.2610034942627 });
            testPath.Add(new double[] { -25.698844909668, 28.2615623474121 });
            testPath.Add(new double[] { -25.6988487243652, 28.2616100311279 });
            testPath.Add(new double[] { -25.6988792419434, 28.2618942260742 });
            testPath.Add(new double[] { -25.6988639831543, 28.2624015808105 });
            testPath.Add(new double[] { -25.6982440948486, 28.2624778747559 });
            testPath.Add(new double[] { -25.6976661682129, 28.2625617980957 });
            testPath.Add(new double[] { -25.6975498199463, 28.2623691558838 });
            testPath.Add(new double[] { -25.6974067687988, 28.2614002227783 });
            testPath.Add(new double[] { -25.6972770690918, 28.2602596282959 });


            //testPath.Add(new double[] { -25.6971302032471, 28.2594871520996 });
            //testPath.Add(new double[] { -25.6971588134766, 28.2594604492188 });
            //testPath.Add(new double[] { -25.697229385376, 28.2600803375244 });
            //testPath.Add(new double[] { -25.6973190307617, 28.260892868042 });
            //testPath.Add(new double[] { -25.6974430084229, 28.261905670166 });
            //testPath.Add(new double[] { -25.6976451873779, 28.2626323699951 });
            //testPath.Add(new double[] { -25.6987380981445, 28.2624816894531 });
            //testPath.Add(new double[] { -25.6999034881592, 28.2623310089111 });
            //testPath.Add(new double[] { -25.7008209228516, 28.2622108459473 });
            //testPath.Add(new double[] { -25.7016830444336, 28.2620983123779 });
            //testPath.Add(new double[] { -25.7026824951172, 28.2619571685791 });
            //testPath.Add(new double[] { -25.7032108306885, 28.262279510498 });
            //testPath.Add(new double[] { -25.703369140625, 28.2635040283203 });
            //testPath.Add(new double[] { -25.7034912109375, 28.2645301818848 });
            //testPath.Add(new double[] { -25.7036228179932, 28.2657241821289 });
            //testPath.Add(new double[] { -25.7036056518555, 28.2663383483887 });
            //testPath.Add(new double[] { -25.7032661437988, 28.2663707733154 });
            //testPath.Add(new double[] { -25.7030544281006, 28.2663764953613 });
           
            string ret = "";


            float lon = 0;
            float lat = 0;
            string deviceId = "";
            string devicePwd = "";
            string deviceReceivedData = "";
            DateTime entryDate = DateTime.UtcNow;
            // Request.Files
            if (Request.QueryString["lon"] != null)
            {
                string mod = Request.QueryString["lon"].Replace(",", ".");
                
                float.TryParse(mod, out lon);
            }
            if (Request.QueryString["lat"] != null)
            {
                string mod = Request.QueryString["lat"].Replace(",", "."); ;
                
                float.TryParse(mod, out lat);
            }
            if (Request.QueryString["id"] != null)
            {
                deviceId = Request.QueryString["id"];
            }
            if (Request.QueryString["pwd"] != null)
            {
                devicePwd = Request.QueryString["pwd"];
            }
            if (Request.QueryString["dt"] != null)
            {
                deviceReceivedData = Request.QueryString["dt"];
            }
            using (DataClassesGpsDataContext ct = new DataClassesGpsDataContext())
            {
                RobotCommand rc = ct.RobotCommands.FirstOrDefault(x => x.DeviceID.ToLower() == deviceId.ToLower() && x.AccessPassword == devicePwd);
                if (rc != null)
                {
                    if (deviceReceivedData.Length >= 0)
                    {
                        rc.TestDataReceived = deviceReceivedData;
                    }
                    rc.LastRequestDate = System.DateTime.Now;
                    ret = rc.TestCommandToSend;
                    ct.SubmitChanges();

                    GpsDevice aDevice = ct.GpsDevices.FirstOrDefault(x => x.DeviceID.Trim().ToLower() == rc.DeviceID.Trim().ToLower());
                    if (aDevice != null)
                    {
                        if (deviceReceivedData.Length >= 0)
                        {
                            string[] dta = deviceReceivedData.Split(new string[] { "x" }, StringSplitOptions.RemoveEmptyEntries);

                            lon = float.Parse(dta[1]);
                            lat = float.Parse(dta[0]);
                            float direction = float.Parse(dta[2]);
                            if (lon != 0 && lat != 0)
                            {
                                aDevice.Lon = lon;
                                aDevice.Lat = lat;
                                ct.SubmitChanges();
                            }
                            List<DeviceRoutePoint> allRoutePoints = ct.DeviceRoutePoints.Where(x => x.ID_Device == aDevice.ID).ToList();
                            DeviceRoutePoint dest = GetNextDestination(allRoutePoints, lat, lon, 0.001f);

                            if (ret == "GPS")
                            {
                                //todo - follow gps route in test
                                string commandText = "TURN_TO_GPS " + dest.PointX.ToString("f5") + " " + dest.PointY.ToString("f5");
                                ret = commandText;
                            }


                        }
                    }
                }
            }

            literalData.Text = ret;
        }
        public DeviceRoutePoint GetNextDestination(List<DeviceRoutePoint> routePoints, double currentX, double currentY, double proximityThreshold)
        {
            DeviceRoutePoint closestPoint = null;
            double minDistance = double.MaxValue;

            foreach (var routePoint in routePoints)
            {
                double distance = CalculateDistance(currentX, currentY, routePoint.PointX, routePoint.PointY);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestPoint = routePoint;
                }
            }

            if (closestPoint != null && minDistance < proximityThreshold)
            {
                // Robot is close enough to the current point, return the next destination
                int currentIndex = routePoints.IndexOf(closestPoint);
                int nextIndex = currentIndex + 1;

                if (nextIndex < routePoints.Count)
                {
                    closestPoint= routePoints[nextIndex];
                   
                }
            }
            

            return null;
        }

        private double CalculateDistance(double x1, double y1, double x2, double y2)
        {
            // Simple Euclidean distance formula
            return Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
        }
    }
}