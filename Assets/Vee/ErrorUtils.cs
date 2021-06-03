
using UnityEngine;

namespace Vee {
    public enum VeeResult {
        Success = 0,
        EGG_HATCH_NO_FAITH_AROUND,
        MERGE_SLOT_ALREADY_UNLOCK,
        MERGE_SLOT_UNLOCK_COIN_NOT_ENOUGH,
        
        FOLK_NOT_EXIST,
        FOLK_IN_OTHER_FACT,

        TILE_NO_FIND,
    }
    public abstract class ErrorUtils {
        public static bool CheckSuccess (VeeResult result, bool logFail = false) {
            if (logFail && result != VeeResult.Success) {
                Debug.LogError("(ERROR)" + result.ToString());
            }
            return result == VeeResult.Success;
        }
    }
}