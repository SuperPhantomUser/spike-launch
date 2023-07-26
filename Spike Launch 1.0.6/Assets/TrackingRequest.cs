using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class TrackingRequest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        #if UNITY_IOS
        // Check the user's consent status.
        // If the status is undetermined, display the request request:
        if(ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED) {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
	#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
