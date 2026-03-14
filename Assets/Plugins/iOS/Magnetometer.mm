#import <CoreMotion/CoreMotion.h>

static CMMotionManager *motionManager;

extern "C"
{
    void StartMagnetometer()
    {
        motionManager = [[CMMotionManager alloc] init];

        if (motionManager.magnetometerAvailable)
        {
            motionManager.magnetometerUpdateInterval = 0.1;
            [motionManager startMagnetometerUpdates];
        }
    }

    void GetMagnetometer(float *x, float *y, float *z)
    {
        if (motionManager.magnetometerData != nil)
        {
            *x = motionManager.magnetometerData.magneticField.x;
            *y = motionManager.magnetometerData.magneticField.y;
            *z = motionManager.magnetometerData.magneticField.z;
        }
        else
        {
            *x = 0;
            *y = 0;
            *z = 0;
        }
    }

    void StopMagnetometer()
    {
        if (motionManager != nil)
        {
            [motionManager stopMagnetometerUpdates];
            motionManager = nil;
        }
    }
}
