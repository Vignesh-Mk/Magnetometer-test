using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
 
public class PostProcessPlist
{
    [PostProcessBuild]
    public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
    {
        // We only care if it's iOS. Obviously.
        if (target != BuildTarget.iOS)
        {
            return;
        }
 
        // Locate the plist file in the exported Xcode project
        string plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
 
        // Get the root dictionary
        PlistElementDict rootDict = plist.root;
 
        // Update the Location Usage Description. 
        // Replace the string with whatever reason you're giving users for tracking them.
        string locationDescription = "The app does not use location data. But the magnet sensor requires this data for accuracy.";
        rootDict.SetString("NSLocationWhenInUseUsageDescription", locationDescription);

        string motionDescription = "The app requires magnetometer access to detect nearby magnetic field.";
        rootDict.SetString("NSMotionUsageDescription", motionDescription);

        AddDeviceCapabilities(rootDict);
        // If you need "Always" usage, you'd add NSLocationAlwaysUsageDescription here too.
        // But you said "alone," so I'll stick to the basics.
 
        // Write the changes back to the file
        File.WriteAllText(plistPath, plist.WriteToString());
        //Debug.Log("iOS Build: Updated Info.plist with Location Usage Description.");
    }

    static void AddDeviceCapabilities(PlistElementDict rootDict)
    {
        const string capabilitiesKey = "UIRequiredDeviceCapabilities";

        PlistElementArray capabilities;

        if (rootDict.values.ContainsKey(capabilitiesKey))
        {
            capabilities = rootDict[capabilitiesKey].AsArray();
        }
        else
        {
            capabilities = rootDict.CreateArray(capabilitiesKey);
        }

        bool magnetometerAlreadyAdded = false;

        foreach (var item in capabilities.values)
        {
            if (item.AsString() == "magnetometer")
            {
                magnetometerAlreadyAdded = true;
                break;
            }
        }

        if (!magnetometerAlreadyAdded)
        {
            capabilities.AddString("magnetometer");
        }
    }
}