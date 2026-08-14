using System.Threading.Tasks;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Merchant;
using StS2AP.Utils;

namespace StS2AP.Patches
{
    [HarmonyPatch(typeof(MerchantEntry), nameof(MerchantEntry.OnTryPurchaseWrapper))]
    public static class SkipUnstockedPurchaseAttempts
    {
        [HarmonyPrefix]
        public static bool Prefix(MerchantEntry __instance, ref Task<bool> __result)
        {
            if (!__instance.IsStocked)
            {
                LogUtility.Info($"ShopSanity: skipped force-purchase attempt on an empty/unstocked slot ({__instance.GetType().Name}), avoiding the FailureOutOfStock dialogue crash.");
                __result = Task.FromResult(false);
                return false; // Skip the original call 
            }
            return true;
        }
    }
}