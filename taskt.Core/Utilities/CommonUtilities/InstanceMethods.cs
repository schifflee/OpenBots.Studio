using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using taskt.Core.Infrastructure;

namespace taskt.Core.Utilities.CommonUtilities
{
    public static class InstanceMethods
    {
        public static void AddAppInstance(this object appObject, IEngine engine, string instanceName)
        {

            if (engine.AppInstances.ContainsKey(instanceName) && engine.EngineSettings.OverrideExistingAppInstances)
            {
                engine.AppInstances.Remove(instanceName);
            }
            else if (engine.AppInstances.ContainsKey(instanceName) && !engine.EngineSettings.OverrideExistingAppInstances)
            {
                throw new Exception("App Instance already exists and override has been disabled in engine settings! " +
                    "Enable override existing app instances or use unique instance names!");
            }

            try
            {
                engine.AppInstances.Add(instanceName, appObject);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static object GetAppInstance(this string instanceName, IEngine engine)
        {
            try
            {
                if (engine.AppInstances.TryGetValue(instanceName, out object appObject))
                {
                    return appObject;
                }
                else
                {
                    throw new Exception("App Instance '" + instanceName + "' not found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void RemoveAppInstance(this string instanceName, IEngine engine)
        {
            try
            {
                if (engine.AppInstances.ContainsKey(instanceName))
                {
                    engine.AppInstances.Remove(instanceName);
                }
                else
                {
                    throw new Exception("App Instance '" + instanceName + "' not found!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
