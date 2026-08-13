using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens.Shops;

namespace StS2AP.Patches
{
    public static class ShopPageUtility
    {
        private const float SlideDuration = 0.4f;

        private static NMerchantInventory? _vanillaPage;
        private static NMerchantInventory? _apPage;

        /// <summary>True once an AP page has actually been spawned and registered for the current shop visit.</summary>
        public static bool HasPages { get; private set; }

        /// <summary>True when the AP page is the one currently scrolled into view.</summary>
        public static bool IsApPageFront { get; private set; }

        public static NMerchantInventory? VanillaPageInstance => _vanillaPage;
        public static NMerchantInventory? ApPageInstance => _apPage;

        /// <summary>Clears any registration left over from a previous shop room, before a new spawn attempt.</summary>
        internal static void Reset()
        {
            _vanillaPage = null;
            _apPage = null;
            IsApPageFront = false;
            HasPages = false;
        }

        internal static void Register(NMerchantInventory vanillaPage, NMerchantInventory apPage)
        {
            _vanillaPage = vanillaPage;
            _apPage = apPage;
            IsApPageFront = false;
            HasPages = true;
        }

        public static void ShowApPage() => Slide(toApPage: true);

        public static void ShowVanillaPage() => Slide(toApPage: false);

        private static void Slide(bool toApPage)
        {
            if (_vanillaPage == null || _apPage == null || !GodotObject.IsInstanceValid(_vanillaPage) || !GodotObject.IsInstanceValid(_apPage))
            {
                return;
            }
            if (toApPage == IsApPageFront)
            {
                return; // Already there, or already mid-transition to there.
            }

            float width = _vanillaPage.Size.X;
            float direction = toApPage ? -1f : 1f;

            Tween tween = _vanillaPage.CreateTween().SetParallel();
            tween.TweenProperty(_vanillaPage, "position:x", _vanillaPage.Position.X + direction * width, SlideDuration)
                .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
            tween.TweenProperty(_apPage, "position:x", _apPage.Position.X + direction * width, SlideDuration)
                .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);

            IsApPageFront = toApPage;
        }
    }
}
